using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StagePanelSetup : MonoBehaviour
{

    [SerializeField] private Transform stagesParent;
    [SerializeField] private GameObject stageInfoHolder;

    void Start()
    {
        SetupUI();
    }

    private void SetupUI()
    {
        var stageData = LevelDataController.instance.GetStageData();

        foreach (var sData in stageData)
        {
            var stageHolder = Instantiate(stageInfoHolder, stagesParent).GetComponent<StageDisplay>();
            stageHolder.SetupStageDisplay(sData);
        }
        
    }
}
