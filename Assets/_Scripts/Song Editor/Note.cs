using System;
using UnityEngine;

namespace RhythmEngine.Examples
{
    /// <summary>
    /// Simple struct containing the data needed to play a mania note.
    /// </summary>
    [Serializable]
    public struct Note
    {
        [Tooltip("The time in seconds at which to play the note.")]
        public double Time;

        [Tooltip("Lane to play the note in.")]
        public int Lane;

        [Tooltip("Specify whether this is a note to avoid or collect.")]
        public NoteType noteType;

        public bool Equals(Note other)
        {
            // We use a small epsilon to account for floating point errors.
            return Math.Abs(Time - other.Time) < 0.0001d && Lane == other.Lane;
        }
    }
    public enum NoteType
    {
        Cheese = 0,
        Obstacle = 1,
        Duplicate =2
    }
}
