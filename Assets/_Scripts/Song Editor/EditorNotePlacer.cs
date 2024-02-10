using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// Class responsible for getting the exact note index from the world position of the mouse.
    /// </summary>
    public class EditorNotePlacer : MonoBehaviour
    {
        [SerializeField] private SongEditor songEditor;
        [SerializeField] private Transform BeatsParent;

        [Space]
        [SerializeField] public float[] LanePositions = { -2.25f, -0.75f, 0.75f, 2.25f }; // We need the lane positions to check if the mouse is inside any of them
        [SerializeField] private float LaneWidth = 1.5f; // For checking if the mouse is outside any lanes

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Mouse0)) // Right mouse for cheese, left mouse for trap
            {
                // This could be also done by comparing screen positions, but this easier.
                var mousePos = Input.mousePosition;
                var worldPos = _camera.ScreenToWorldPoint(mousePos);

                var currentLane = GetLaneFromWorldPos(worldPos);
                var currentBeat = GetBeatFromWorldPos(worldPos);
                NoteType noteType = Input.GetKeyDown(KeyCode.Mouse1) ? NoteType.Cheese : NoteType.Obstacle;

                songEditor.ToggleNote(new Vector2Int(currentBeat, currentLane),noteType);
            }
        }

        private int GetLaneFromWorldPos(Vector3 worldPos)
        {
            // We don't want to place notes outside the lanes
            if (worldPos.y < LanePositions[0] - LaneWidth / 2f) return -1;
            if (worldPos.y > LanePositions[LanePositions.Length - 1] + LaneWidth / 2f) return -1;

            // We just look for the closest distance from the mouse to the lanes
            var lane = 0;
            var minDist = float.MaxValue;

            for (var i = 0; i < LanePositions.Length; i++)
            {
                var lanePos = LanePositions[i];
                var dist = Mathf.Abs(worldPos.y - lanePos); // d(A,B) = |B - A|

                if (dist < minDist)
                {
                    minDist = dist;
                    lane = i;
                }
            }

            return lane;
        }

        private int GetBeatFromWorldPos(Vector3 worldPos)
        {
            // We can use the beats parent as sort of an "anchor" for the beats
            var xPos = worldPos.x - BeatsParent.position.x;
            // And then we just round to the nearest beat, no need to check if it's outside the bounds because the editor will do that
            var closestBeat = Mathf.Round(xPos / songEditor.BeatSpacing) * songEditor.BeatSpacing;
            print("Beat:" + closestBeat);
            return (int)closestBeat;
        }
    }
}

