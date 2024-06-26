using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmEngine;
public class SongStarter : MonoBehaviour
{
    private RhythmEngineCore rhythmEngine;
    public HamoebaSong SongToPlay;
    public void Awake()
    {
        rhythmEngine = GetComponent<RhythmEngineCore>();
        rhythmEngine.SetSong(SongToPlay);
    }
    public void StartSong()
    {
        rhythmEngine.InitTime();
        rhythmEngine.Play();
    }
}
