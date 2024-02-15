using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmEngine;

public class PlayerController : MonoBehaviour
{
    public ExampleGameManager gameManager;
    public RhythmEngineCore rhythmEngine;
    public NoteManager noteManager;
    public float trackWidth;
    public int minLane;
    public int maxLane;
    public int currentLane = 1;
    public bool clone;
    private GameObject NoteLines;
    private void Start()
    {
        NoteLines = noteManager.Track2;
        gameManager = GameObject.Find("Game Manager").GetComponent<ExampleGameManager>();
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (gameManager.State == GameState.Paused || gameManager.State == GameState.Win || gameManager.State == GameState.Lose)
            return;

        bool upKey = User_Input.instance.MainMoveUp;
        bool downKey = User_Input.instance.MainMoveDown;
        if (clone)
        {
             upKey = User_Input.instance.CloneMoveUp;
             downKey = User_Input.instance.CloneMoveDown;
        }

            
        //TODO: make keys remappable with the new Unity Input System. This works for now
        if (downKey && currentLane > minLane)
        {
            currentLane--;
            transform.position = new Vector2(transform.position.x, transform.position.y - trackWidth);
        }
        else if (upKey && currentLane < maxLane)
        {
            currentLane++;
            transform.position = new Vector2(transform.position.x, transform.position.y + trackWidth);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            ExampleGameManager.score++;
            noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
            //increase score and health, count hit notes
        }
        if(collision.CompareTag("trap"))
        {
            ExampleGameManager.health-=25;
            //noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
        }
        if(collision.CompareTag("DuplicationNote"))
        {
            NoteLines.SetActive(true);
            noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
        }
    }
}
