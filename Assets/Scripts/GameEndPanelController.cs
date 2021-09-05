   using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameEndPanelController : MonoBehaviour
{
    [SerializeField] private GameObject medalOne;
    [SerializeField] private GameObject medalTwo;
    [SerializeField] private GameObject medalThree;

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private GameObject holder;

    [SerializeField] private TMP_Text titleText;
    [SerializeField] private SceneLoader sceneLoader;

    [SerializeField] private GameObject nextSceneButtonHolder;

    private string nextSceneName;
    private bool nextScenePresent;
    private void Start()
    {
        
        holder.SetActive(false);

        medalOne.SetActive(false);
        medalTwo.SetActive(false);
        medalThree.SetActive(false);
        
        ScoreManager.onStageEnded += ShowScreen;
    }

    private void SetupNextScene()
    {
        var nextStageName = FindObjectOfType<StageManager>().GetNextStage();
        if (nextStageName == "")
        {
            nextScenePresent = false;
        }
        else
        {
            nextScenePresent = true;
            nextSceneName = nextStageName;
        }
    }

    public void ShowScreen(string stageName, float completionPercentage, int maxScore)
    {
        SetupNextScene();

        titleText.text = completionPercentage < 1f ? "Lost :(" : "Win!";

        holder.SetActive(true);
        if (completionPercentage >= 0.33f) medalOne.SetActive(true);
        if (completionPercentage >= 0.66f) medalTwo.SetActive(true);
        if (completionPercentage >= 1f) medalThree.SetActive(true);

        scoreText.text = "Score: " + maxScore.ToString();
        nextSceneButtonHolder.SetActive(nextScenePresent);
    }
    public void GotoNextStage()
    {
        if(nextScenePresent) sceneLoader.StartScene(nextSceneName);
        else Debug.LogError("Couldn't load next Scene, Scene is missing");
    }
    public void GoToSceneLoader()
    {
        sceneLoader.StartScene("SceneLoader");
    }
    public void Restart()
    {
        sceneLoader.StartScene(SceneManager.GetActiveScene().name);
    }
    private void OnDestroy()
    {
        ScoreManager.onStageEnded -= ShowScreen;
    }
}
