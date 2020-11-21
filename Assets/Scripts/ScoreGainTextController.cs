using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreGainTextController : MonoBehaviour
{
    private TMP_Text scoreText;

    private void Start()
    {
        scoreText = GetComponentInChildren<TMP_Text>();
        scoreText.enabled = false;
    }

    public void DamageTaken(float damageTaken)
    {
        scoreText.enabled = true;
        scoreText.text = (damageTaken * 10).ToString();

        Invoke(nameof(Disable), 2f);
    }

    private void Disable()
    {
        scoreText.enabled = false;
    }
}
