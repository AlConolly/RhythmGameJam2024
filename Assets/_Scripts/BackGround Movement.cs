using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMovement : MonoBehaviour
{
    public float startX;
    public float endX;
    private ExampleGameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<ExampleGameManager>();
    }
    void Update()
    {
        float xPos = Mathf.Lerp(startX, endX, (float)gameManager.songProgress);
        transform.position = new Vector2(xPos, transform.position.y);
    }
}
