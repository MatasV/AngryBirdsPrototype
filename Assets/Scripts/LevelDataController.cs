using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;

[CreateAssetMenu]
public class LevelDataController : ScriptableObject
{
    [SerializeField] private List<StageData> stagesData = new List<StageData>();

    private void LoadAvailableStagesFromBuildMenu()
    {
        for (int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var path = SceneUtility.GetScenePathByBuildIndex(i);
            var lastPart = Path.GetFileNameWithoutExtension(path.Split('/').Last());

            Debug.Log(path);
            stagesData.Add(new StageData(lastPart, 0f, 0));
        }
    }

    public List<StageData> GetStageData()
    {
        return stagesData;
    }

    public void Awake()
    {
        LoadAvailableStagesFromBuildMenu();
        LoadLevelData();
        ScoreManager.onStageEnded += UpdateLevelData;
    }
    public void UpdateLevelData(string stageName, float completionPercentage, int maxScore)
    {
        StageData foundStage;

        try { foundStage = stagesData.First(x => x.stageName == stageName); }
        catch
        {
            return;
        }

        if (foundStage.stageCompletePercentage < completionPercentage) foundStage.stageCompletePercentage = completionPercentage;
        if (foundStage.maxScore < maxScore) foundStage.maxScore = maxScore;

        LevelParser.WriteToBinaryFile<List<StageData>>(Application.persistentDataPath + "/LevelData.dat", stagesData, false);
    }

    private void LoadLevelData()
    {
        var loadedStageData = LevelParser.ReadFromBinaryFile<List<StageData>>(Application.persistentDataPath + "/LevelData.dat");

        if (loadedStageData == null)
        {
            Debug.Log("No Save Data available");
            return;
        }

        foreach (var stageData in loadedStageData)
        {
            StageData foundStage;

            try { foundStage = stagesData.FirstOrDefault(x => x.stageName == stageData.stageName); }
            catch
            {
                Debug.LogWarning("Save data contains a level name that no longer exists in the game, dismissing...");
                continue;
            }

            foundStage.stageName = stageData.stageName;
            foundStage.stageCompletePercentage = stageData.stageCompletePercentage;
            foundStage.maxScore = stageData.maxScore;

        }
    }
}
