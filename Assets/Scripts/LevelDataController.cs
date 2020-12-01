using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO;

public class LevelDataController : MonoBehaviour
{
    [SerializeField] private List<StageData> stagesData = new List<StageData>();

    public static LevelDataController instance;

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

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        LoadAvailableStagesFromBuildMenu();
        LoadLevelData();
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (ScoreManager.instance != null)
        {
            Debug.Log("Scene loaded found ScoreManager instance");
            ScoreManager.instance.onStageEnded += UpdateLevelData;
        }
        else Debug.Log("Couldn't find ScoreManager instance");
    }

    private void UpdateLevelData(string stageName, float completionPercentage, int maxScore)
    {
        Debug.Log(stageName);
        StageData foundStage;

        try { foundStage = stagesData.First(x => x.stageName == stageName); }
        catch
        {
            Debug.LogWarning("Tried to save stage data, but couldn't find the required stage in the list");
            return;
        }

        Debug.Log(gameObject.name);
        Debug.Log(stageName + " " + completionPercentage.ToString() + " " + maxScore.ToString());
        Debug.Log(foundStage.maxScore + " " + foundStage.stageCompletePercentage + " " + foundStage.stageName);

        if (foundStage.stageCompletePercentage < completionPercentage) foundStage.stageCompletePercentage = completionPercentage;
        if (foundStage.maxScore < maxScore) foundStage.maxScore = maxScore;

        LevelParser.SaveStageData(stagesData);

    }

    private void LoadLevelData()
    {
        var loadedStageData = LevelParser.LoadStageData();

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
