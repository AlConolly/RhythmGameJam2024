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
    public float damageOnTrap = 25;
    public int minLane;
    public int maxLane;
    public int currentLane = 1;
    public bool clone;
    private GameObject NoteLines;
    private Vector2 noteLinesEndPos;
    private Vector2 underScreen = new Vector2(0, -13);
    public float dupeCooldown = 2;
    private bool canDupe = true;
    public AudioClip trapClip;
    private AudioSystem au;
    private void Start()
    {
        au = GameObject.Find("Audio System").GetComponent<AudioSystem>();
        NoteLines = noteManager.Track2;
        noteLinesEndPos = NoteLines.transform.position;
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

        if (clone)
        {
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
        }

        //TODO: make keys remappable with the new Unity Input System. This works for now
        //if (downKey && currentLane > minLane)
        if (clone && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(downKey) || Input.GetKeyDown(upKey)))
        {
            if (currentLane == minLane)
            {
                currentLane++;
                transform.position = new Vector2(transform.position.x, transform.position.y + trackWidth);
            }
            else if (currentLane == maxLane)
            {
                currentLane--;
                transform.position = new Vector2(transform.position.x, transform.position.y - trackWidth);
            }

        }
        else if (Input.GetKeyDown(downKey) && currentLane > minLane)
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
        else if (Input.GetKeyDown(upKey) && currentLane == maxLane)
        {
            currentLane -= 2;
            transform.position = new Vector2(transform.position.x, transform.position.y - 2 * trackWidth);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {
            ExampleGameManager.score++;
            noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
            ExampleGameManager.health += 2;
            //increase score and health, count hit notes
        }
        if (collision.CompareTag("trap"))
        {
            //noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
        }
        if (collision.CompareTag("DuplicationNote"))
        {
            if (canDupe && !clone)
            {
                if (NoteLines.activeSelf)
                {
                    StartCoroutine(MoveFunction(underScreen));
                    Invoke("ToggleNoteLines", 2);
                }
                else
                {
                    NoteLines.SetActive(!NoteLines.activeSelf);
                    NoteLines.transform.position = new Vector2(0, transform.position.y);
                    StartCoroutine(MoveFunction(noteLinesEndPos));
                }
                canDupe = false;
                Invoke("resetCooldown", dupeCooldown); // after cooldown seconds can interact with dupeNote again
            }
            noteManager.DespawnNote(noteManager.GetClosestNoteToInput(currentLane).Value);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("trap"))
        {
            au.PlaySound(trapClip);
            ExampleGameManager.health -= damageOnTrap;
        }
    }
    private void resetCooldown()
    {
        canDupe = true;
    }
    private void ToggleNoteLines()
    {
        NoteLines.SetActive(!NoteLines.activeSelf);
    }
    IEnumerator MoveFunction(Vector2 moveTo)
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime / 10;
            NoteLines.transform.position = Vector2.Lerp(NoteLines.transform.position, moveTo, timeSinceStarted);

            // If the object has arrived, stop the coroutine
            if ((Vector2)NoteLines.transform.position == moveTo)
            {
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }
    }
}
