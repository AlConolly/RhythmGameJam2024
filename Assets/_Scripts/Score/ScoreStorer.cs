using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreStorer : MonoBehaviour
{
    public Score score;
    public List<string> levelNames;
    public void SaveScore(string levelName, float clearPercent)
    {
        if(!score.score.ContainsKey(levelName))
            score.score.Add(levelName, clearPercent);
        else if(score.score[levelName] > clearPercent)
        {
            score.score[levelName] = clearPercent;
        }
                
    }
    public float GetScore(string levelName)
    {
        return score.score[levelName];
    }
    private void Start()
    {
        foreach(string n in levelNames)
        {
            SaveScore(n, 0);
        }
    }
}
