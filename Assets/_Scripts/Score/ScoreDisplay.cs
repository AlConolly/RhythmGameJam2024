using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;
    public ScoreStorer ScoreStorer;
    private void Start()
    {
        ScoreStorer = GameObject.Find("ScoreStorer").GetComponent<ScoreStorer>();
        text = GetComponent<TextMeshProUGUI>();
    }
    public void testDisplayScore(string levelname)
    {
        text.text = "HighScore: " + ScoreStorer.score.score[levelname] + "%";
    }
    public void testHover()
    {
        print("Hovered Over button");
    }
}
