using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameWinUI;
    [HideInInspector]
    public bool gameOver;
    private bool gameWin;

    void Update()
    {
        if (gameOver)
        {
            gameOverUI.SetActive(true);
            if(Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(0);
            }
        }
        if (gameWin) {
            gameWinUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);

            }
        }
        //GameObject.FindObjectOfType<Collisions>().gameOver += onGameOver;
    }

    void onGameWin() {
        if (Time.time > 90) {
            gameWin = true;
        }
    }
}
