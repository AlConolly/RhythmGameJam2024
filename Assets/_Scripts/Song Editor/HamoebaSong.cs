using System.Collections.Generic;
using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// This version of a Song contains all the data needed to play the mania demo.
    /// </summary>
    [CreateAssetMenu(fileName = "HamoebaSong", menuName = "RhythmEngine/Songs/HamoebaSong")]
    // Feel free to uncomment the above line if you want to create more mania song assets.
    public class HamoebaSong : BeatSequencedSong
    {
        [Header("Mania Notes")]
        public List<Note> Notes = new();

        [Header("Mania Settings")]
        [Tooltip("Time (in seconds) in which a note will lerp from the top to the bottom of the screen")]
        public float NoteFallTime = 1;

        [Tooltip("Input time offset (in milliseconds) in which a note will be considered 'perfect'")]
        public double PerfectTimeMs = 50;

        [Tooltip("Input time offset (in milliseconds) in which a note will be considered 'good'")]
        public double GoodTimeMs = 100;
    }
}
