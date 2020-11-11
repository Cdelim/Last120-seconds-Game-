using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    //public event System.Action gameOver;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.FindObjectOfType<GameOver>().gameOver = true;
    }
}
