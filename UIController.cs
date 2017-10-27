using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private GameObject winPanel; // Display when win
    [SerializeField] private GameObject losePanel; // display when lose

    [SerializeField] private Text scoreText;
    public int score = 0;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("UIController -- More than one instance");
        }
    }

    public void IncreaseScore(int toIncrease)
    {
        score += toIncrease;
        scoreText.text = "Score: " + score;

        if (score >= GameController.instance.pointsToWin)
        {
            GameController.instance.OnVictory();
            winPanel.SetActive(true);
        }
    }

    public void OnLose()
    {
        losePanel.SetActive(true);
    }
}