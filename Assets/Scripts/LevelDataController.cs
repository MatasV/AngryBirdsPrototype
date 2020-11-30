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
        for(int i = 2; i < SceneManager.sceneCountInBuildSettings; i++)
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

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
    }

    private void SceneManager_sceneUnloaded(Scene arg0)
    {
        if (ScoreManager.instance != null) {
            Debug.Log("Scene unloaded fond ScoreManager instance");
            ScoreManager.instance.onStageEnded -= UpdateLevelData; }
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (ScoreManager.instance != null) {
            Debug.Log("Scene loaded found ScoreManager instance");
            ScoreManager.instance.onStageEnded += UpdateLevelData; }
    }

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }

        LoadAvailableStagesFromBuildMenu();
        LoadLevelData();
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateLevelData(string stageName, float completionPercentage, int maxScore)
    {
        var foundStage = stagesData.FirstOrDefault(x => x.stageName == stageName);

        if(!EqualityComparer<StageData>.Default.Equals(foundStage, default))
        {
            if (foundStage.stageCompletePercentage < completionPercentage) foundStage.stageCompletePercentage = completionPercentage;
            if (foundStage.maxScore < maxScore) foundStage.maxScore = maxScore;
        } else
        {
            Debug.LogWarning("Tried to save stage data, but couldn't find the required stage in the list");
        }
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
            var foundStage = stagesData.FirstOrDefault(x => x.stageName == stageData.stageName);

            if (!EqualityComparer<StageData>.Default.Equals(foundStage, default))
            {
                foundStage.stageName = stageData.stageName;
                foundStage.stageCompletePercentage = stageData.stageCompletePercentage;
                foundStage.maxScore = stageData.maxScore;
            } else
            {
                Debug.LogWarning("Save data contains a level name that no longer exists in the game, dismissing...");
            }
        }
    }


}
