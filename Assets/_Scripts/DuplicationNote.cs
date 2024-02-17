using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RhythmEngine.Examples
{
    public class DuplicationNote : MonoBehaviour
    {
        private GameObject NoteLines;
        private void Start()
        {
            NoteLines = GetComponentInParent<NoteManager>().Track2;
        }
        private void OnDisable()
        {
            NoteLines.SetActive(!NoteLines.activeSelf);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                print("player triggered duplication");
                NoteLines.SetActive(!NoteLines.activeSelf);
            }
        }
        
    }
}
