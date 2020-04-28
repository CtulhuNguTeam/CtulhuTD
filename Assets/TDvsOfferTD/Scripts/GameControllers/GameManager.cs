using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public static bool gameIsOver;

    void Start()
    {
        gameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver) return;
        if (PlayerStats.lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameIsOver = true;
        gameOverUI.SetActive(true);
    }
}
