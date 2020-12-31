using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ScoreDisplay : MonoBehaviour
{
    private int numberCount = 8;
    private TMP_Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        ScoreManager.onScoreChanged += DisplayScore;

        DisplayScore(0);
    }
    public void DisplayScore(int score)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("SCORE:");

        var scoreString = score.ToString();
        

        for (int i =0; i < numberCount - scoreString.Length; i++)
        {
            sb.Append("0");
        }

        sb.Append(scoreString);

        scoreText.text = sb.ToString();
    }
}
