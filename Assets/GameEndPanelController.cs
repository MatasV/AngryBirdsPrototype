using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameEndPanelController : MonoBehaviour
{
    private void Start()
    {
        ScoreManager.instance.onStageEnded += Init;
    }
    public void Init()
    {

    }
    
}
