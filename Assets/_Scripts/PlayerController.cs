using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float trackWidth;
    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        //TODO: make keys remappable with the new Unity Input System. This works for now
        //TODO: retrict movement to tracks
        if (Input.GetKeyDown(KeyCode.DownArrow))
            transform.position = new Vector2(transform.position.x, transform.position.y - trackWidth);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            transform.position = new Vector2(transform.position.x, transform.position.y + trackWidth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            //increase score and health, count hit notes
        }
    }
}
