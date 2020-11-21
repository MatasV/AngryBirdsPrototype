using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeDisplay : MonoBehaviour
{

    public TMP_Text healthDisplayText;
    private void Start()
    {
        StageManager.instance.onBirdCountChanged += SetHealthText;
    }

    private void SetHealthText(int healthRemaining)
    {
        healthDisplayText.text = "X" + healthRemaining.ToString();
    }
}
