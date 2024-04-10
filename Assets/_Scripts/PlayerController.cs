using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmEngine;
using UnityEngine.UI;
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
    public Image hitScreen;
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


        bool pressedDown = Input.GetKeyDown(KeyCode.W) || touchedQ3();
        bool pressedUp = Input.GetKeyDown(KeyCode.S) || touchedQ1();
        bool pressedSpace = Input.GetKeyDown(KeyCode.Space) || touchedRightHalf();

        //TODO: make keys remappable with the new Unity Input System. This works for now
        //if (downKey && currentLane > minLane)
        if (clone)
        {
            if (pressedSpace || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
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
        }
        else if (pressedDown && currentLane > minLane)
        {
            currentLane--;
            transform.position = new Vector2(transform.position.x, transform.position.y - trackWidth);
        }
        else if (pressedDown && currentLane == minLane)
        {
            currentLane += 2;
            transform.position = new Vector2(transform.position.x, transform.position.y + 2 * trackWidth);
        }
        //else if (upKey && currentLane < maxLane)
        else if (pressedUp && currentLane < maxLane)
        {
            currentLane++;
            transform.position = new Vector2(transform.position.x, transform.position.y + trackWidth);
        }
        else if (pressedUp && currentLane == maxLane)
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


    private void gotHurt()
    {
        ExampleGameManager.health -= damageOnTrap;

        var color = hitScreen.color;
        color.a = 1;

        hitScreen.color = color;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("trap"))
        {
            au.PlaySound(trapClip);
            gotHurt();
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
    /// <summary>
    /// takes pixel coordinates bottom left is (0,0); top right is (pixelWidth,pixelHeight)
    /// </summary>
    /// <param name="x1">lower x bound</param>
    /// <param name="y1">lower y bound</param>
    /// <param name="x2">upper x bound</param>
    /// <param name="y2">upper y bound</param>
    /// <returns>returns true if touch is in specified zone</returns>
    private bool touchedBound(int x1, int y1, int x2, int y2)
    {
        if (Input.touchCount <= 0)
            return false;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && touch.position.x > x1 && touch.position.y > y1 && touch.position.x < x2 && touch.position.y < y2)
            {
                return true;
            }
        }
        return false;
    }
    private bool touchedQ3()
    {
        Camera cam = Camera.main;
        return touchedBound(0, 0, cam.pixelWidth/2-1, cam.pixelHeight/2-1);
    }
    private bool touchedQ1()
    {
        Camera cam = Camera.main;
        return touchedBound(0, cam.pixelHeight / 2, cam.pixelWidth/2, cam.pixelHeight);
    }

    private bool touchedRightHalf()
    {
        Camera cam = Camera.main;
        return touchedBound(cam.pixelWidth / 2, 0, cam.pixelWidth, cam.pixelHeight);
    }

}
