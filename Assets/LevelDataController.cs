using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelDataController : MonoBehaviour
{

    public List<string> availableSceneNames = new List<string>();

    public List<StageData> levelData = new List<StageData>();

    public StageData currentStageData;

    void Start()
    {
        var path = SceneUtility.GetScenePathByBuildIndex(2);
        var lastPart = path.Split('/').Last();
        Debug.Log(lastPart);

        LoadAvailableScenes();
        LoadLevelData();
        SetupUI();
    }

    public void EnterStage(string stageName)
    {

    }


    private void SetupUI()
    {
        
    }

    private void LoadAvailableScenes()
    {
        foreach (var obj in Resources.LoadAll("Stages/"))
        {
            availableSceneNames.Add(obj.name);
        }
    }

    private void LoadLevelData()
    {
        levelData = (List<StageData>)LevelParser.LoadStageData();
    }


}
