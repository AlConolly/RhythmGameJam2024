using System;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Simple struct containing the data needed to play a mania note.
/// </summary>
[CreateAssetMenu(fileName = "Score", menuName = "Score")]
// Feel free to uncomment the above line if you want to create more mania song assets.
public class Score : ScriptableObject
{
    public Dictionary<string, float> score;
}