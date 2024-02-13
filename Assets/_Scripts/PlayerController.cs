using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmEngine;

public class PlayerController : MonoBehaviour
{
    public RhythmEngineCore rhythmEngine;
    public NoteManager noteManager;
    public float trackWidth;
    public int minLane;
    public int maxLane;
    public int currentLane = 1;
    public bool clone;
    private Note nextNote;
    private GameObject NoteLines;
    private void Start()
    {
        NoteLines = noteManager.Track2;
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        KeyCode upKey = KeyCode.W;
        KeyCode downKey = KeyCode.S;
        if (clone)
        {
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
        }
        //TODO: make keys remappable with the new Unity Input System. This works for now
        //TODO: retrict movement to tracks
        if (Input.GetKeyDown(downKey) && currentLane > minLane)
        {
            currentLane--;
            transform.position = new Vector2(transform.position.x, transform.position.y - trackWidth);
        }
        else if (Input.GetKeyDown(upKey) && currentLane < maxLane)
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
            noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
        }
        if(collision.CompareTag("DuplicationNote"))
        {
            NoteLines.SetActive(true);
            noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
        }
    }
}
