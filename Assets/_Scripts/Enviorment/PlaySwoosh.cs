using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySwood : MonoBehaviour
{
    public AudioClip swoosh;
    private AudioSystem au;

    void Start()
    {
        au = GameObject.Find("Audio System").GetComponent<AudioSystem>();
        au.PlaySound(swoosh);
    }
}
