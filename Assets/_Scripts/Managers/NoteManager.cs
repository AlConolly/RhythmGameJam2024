using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// This class is responsible for spawning and despawning the visual notes(cheese and obstacles) in game.
    /// 99% of this is copied from the Mania demo of the Rhythm Engine
    /// </summary>
    public class NoteManager : MonoBehaviour
    {
        [SerializeField] private RhythmEngineCore RhythmEngine;
        [SerializeField] private Transform CheeseNotePrefab, ObstaclePrefab, DuplicationPrefab;

        [Space]
        [SerializeField] private float[] LanePositions = {-2.25f, -0.75f, 0.75f, 2.25f}; // Y positions of the lanes
        [SerializeField] private float SpawnX = 6f;  // X position of the notes when they spawn
        [SerializeField] private float KeysX = -8f;  // X position of the notes when they are meant to be pressed

        //TODO: change SimpleManiaSong struct to differenciate between cheese and obstacles
        private HamoebaSong Song => RhythmEngine.Song as HamoebaSong; // We need to cast the song to our custom type to access the notes, note fall time, etc
        private float NoteFallTime => Song.NoteFallTime;

        // Notes that haven't been spawned yet, it's a queue because we only need the access to the first element when spawning new notes
        private Queue<Note> _unspawnedNotes;
        private List<SpawnedNote> _spawnedNotes;

        public event Action OnMiss; // This event is invoked when a note is missed (i.e. it's 100ms after the note's end time)
        public GameObject Track2; //this is what is enabled when duplication notes are hit

        /// <summary>
        /// A simple way of storing the spawned notes.
        /// All the properties are readonly because we don't want to change them after the note has been spawned.
        /// </summary>
        private class SpawnedNote
        {
            public readonly Transform Transform;
            public readonly Note Note;

            public readonly double StartTime;
            public readonly double EndTime;

            public SpawnedNote(Transform transform, Note note, double startTime, double noteFallTime)
            {
                Transform = transform;
                Note = note;

                StartTime = startTime;
                EndTime = startTime + noteFallTime;
            }
        }

        private void Awake()
        {
            _unspawnedNotes = new Queue<Note>(Song.Notes.OrderBy(note => note.Time)); // We order the notes by time so we can spawn them in order
            _spawnedNotes = new List<SpawnedNote>();
            Track2 = Helpers.FindInActiveObjectByTag("secondTrack");
        }

        private void Update()
        {
            var time = RhythmEngine.GetCurrentAudioTime(); // Use the RhythmEngine to get the accurate current audio time
            TrySpawningNotes(time);
            HandleSpawnedNotes(time);
        }

        private void TrySpawningNotes(double currentTime)
        {
            if (_unspawnedNotes.Count == 0) return;

            // If there's a note left to spawn...
            var closestUnspawnedNote = _unspawnedNotes.Peek();
            if (currentTime > closestUnspawnedNote.Time - NoteFallTime) // ...and it's time to spawn it...
            {
                _unspawnedNotes.Dequeue(); // ...remove it from the queue...
                SpawnNote(closestUnspawnedNote, currentTime); // ...and spawn it
            }

            // Note: we spawn the notes {NoteFallTime} seconds before their actual time so they can fall from the top of the screen to the bottom.
        }

        private void SpawnNote(Note note, double currentTime)
        {
            Transform notePrefab = CheeseNotePrefab; // set to CheeseNotePreab cuz the compiler complains otherwise
            switch (note.noteType)
            {
                case NoteType.Cheese:
                    notePrefab = CheeseNotePrefab;
                    break;
                case NoteType.Obstacle:
                    notePrefab = ObstaclePrefab;
                    break;
                case NoteType.Duplicate:
                    notePrefab = DuplicationPrefab;
                    break;
            }
            var noteTransform = Instantiate(notePrefab, transform);
            noteTransform.localPosition = new Vector3(SpawnX, LanePositions[note.Lane], 0); // Set the note's position to the correct lane and the spawn x position
            _spawnedNotes.Add(new SpawnedNote(noteTransform, note, currentTime, NoteFallTime)); // Add the note to the list of spawned notes
        }

        /// <summary>
        /// Despawn a specific note. This overload is only called from the ManiaGameplayManager when a note is hit.
        /// </summary>
        public void DespawnNote(Note note)
        {
            // Since we don't have access to the spawned notes from the purely logical note itself, we need to find it in the list
            var spawnedNote = _spawnedNotes.FirstOrDefault(n => Equals(n.Note, note));
            if (spawnedNote == null) return;

            Destroy(spawnedNote.Transform.gameObject);
            _spawnedNotes.Remove(spawnedNote);
        }

        /// <summary>
        /// Despawn a specific note. This overload is only called when a note is missed.
        /// </summary>
        private void DespawnNote(SpawnedNote note)
        {
            Destroy(note.Transform.gameObject);
            _spawnedNotes.Remove(note);
        }

        /// <summary>
        /// Move the spawned notes across the screen and despawn them if they're missed (100ms after the note's end time).
        /// </summary>
        private void HandleSpawnedNotes(double currentTime)
        {
            if (_spawnedNotes.Count == 0) return;

            // We can't remove notes from the {_spawnedNotes} list while iterating over it
            var notesToRemove = new List<SpawnedNote>();

            foreach (var spawnedNote in _spawnedNotes)
            {
                // Miss only happens if the note's time is further than the lowest possible judgement (Good, 100ms)
                // We also need to convert the GoodTimeMs to seconds by dividing it by 1000
                // We likely won't have GoodTime scoring for this game, but I'm keeping this in so we can control the time when notes are removed
                if (currentTime > spawnedNote.EndTime + Song.GoodTimeMs / 1000)  
                {
                    notesToRemove.Add(spawnedNote);
                    OnMiss?.Invoke();
                    continue;
                }

                // Inverse lerp is used to get the note's progress (if currentTime is StartTime, the note's progress is 0, if it's EndTime, the note's progress is 1)
                // We use unclamped versions of lerp and inverse lerp because we want the note to be able to go past the end position and not stop on the KeysHeight
                double noteProgress = InverseLerpUnclamped(spawnedNote.StartTime, spawnedNote.EndTime, currentTime);

                // We use lerp to move the note across the screen
                // Side note: ALWAYS prefer to use lerp when changing visual properties of a rhythm aligned object, otherwise it may fall out of sync with the audio
                // It's possible to use Coroutines and other methods for this, but lerp and inverse lerp don't require much more extra fiddling.
                var xPosition = Mathf.LerpUnclamped(KeysX, SpawnX, (float)noteProgress);
                spawnedNote.Transform.localPosition = new Vector3(-xPosition, spawnedNote.Transform.localPosition.y, 0);
            }

            foreach (var note in notesToRemove)
            {
                DespawnNote(note);
            }
        }

        private static double InverseLerpUnclamped(double a, double b, double value)
        {
            return (value - a) / (b - a);
        }

        /// <summary>
        /// Used by the ManiaGameplayManager to find the closest note to the input.
        /// Since SimpleManiaNote is a struct, and there might not be any more notes on a lane, we need to return a nullable type. (SimpleManiaNote?)
        /// </summary>
        /// <param name="key">Note lane index</param>
        /// <param name="currentTime">Current time in seconds</param>
        /// <returns></returns>
        public Note? GetClosestNoteToInput(int key, double currentTime)
        {
            // We order the notes by their time offset to the current time, and get the first one (the closest one)
            var closestNote = _spawnedNotes.Where(n => n.Note.Lane == key).OrderBy(n => Math.Abs(n.EndTime - (float)currentTime)).FirstOrDefault();
            return closestNote?.Note;
        }
    }
}
