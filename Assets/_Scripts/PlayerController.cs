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
    public float damageOnTrap =25;
    public int minLane;
    public int maxLane;
    public int currentLane = 1;
    public bool clone;
    private GameObject NoteLines;
    public float dupeCooldown = 2;
    private bool canDupe = true;
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

        /*
        bool upKey = User_Input.instance.MainMoveUp;
        bool downKey = User_Input.instance.MainMoveDown;
        if (clone)
        {
             upKey = User_Input.instance.CloneMoveUp;
             downKey = User_Input.instance.CloneMoveDown;
        }
        */
        KeyCode upKey = KeyCode.W;
        KeyCode downKey = KeyCode.S;

        if(clone)
        {
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
        }
        
        //TODO: make keys remappable with the new Unity Input System. This works for now
        //if (downKey && currentLane > minLane)
        if(clone && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(downKey) || Input.GetKeyDown(upKey)))
        {
            if (currentLane == minLane)
            {
                currentLane++;
                transform.position = new Vector2(transform.position.x, transform.position.y + trackWidth);
            }
            else if(currentLane == maxLane)
            {
                currentLane--;
                transform.position = new Vector2(transform.position.x, transform.position.y - trackWidth);
            }
            
        }
        else if(Input.GetKeyDown(downKey) && currentLane > minLane)
        {
            currentLane--;
            transform.position = new Vector2(transform.position.x, transform.position.y - trackWidth);
        }
        else if (Input.GetKeyDown(downKey) && currentLane == minLane)
        {
            currentLane += 2;
            transform.position = new Vector2(transform.position.x, transform.position.y + 2 * trackWidth);
        }
        //else if (upKey && currentLane < maxLane)
        else if (Input.GetKeyDown(upKey) && currentLane < maxLane)
        {
            currentLane++;
            transform.position = new Vector2(transform.position.x, transform.position.y + trackWidth);
        }
        else if(Input.GetKeyDown(upKey) && currentLane == maxLane)
        {
            currentLane -= 2;
            transform.position = new Vector2(transform.position.x, transform.position.y -2* trackWidth);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            ExampleGameManager.score++;
            noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
            ExampleGameManager.health += 2;
            //increase score and health, count hit notes
        }
        if(collision.CompareTag("trap"))
        {
            ExampleGameManager.health-=damageOnTrap;
            //noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
        }
        if(collision.CompareTag("DuplicationNote"))
        {
            if (canDupe && !clone)
            {
                NoteLines.SetActive(!NoteLines.activeSelf);
                canDupe = false;
                Invoke("resetCooldown", dupeCooldown); // after cooldown seconds can interact with dupeNote again
            }
            noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
        }
    }
    private void resetCooldown()
    {
        canDupe = true;
    }

}
