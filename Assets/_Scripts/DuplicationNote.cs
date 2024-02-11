using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicationNote : MonoBehaviour
{
    private GameObject NoteLines;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            NoteLines.SetActive(true);
        }
    }
}
