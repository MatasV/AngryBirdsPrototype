using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
public class StageDisplay : MonoBehaviour
{
    [SerializeField] private Image checkMarkImage;

    [SerializeField] private GameObject medalOne;
    [SerializeField] private GameObject medalTwo;
    [SerializeField] private GameObject medalThree;

    [SerializeField] private TMP_Text stageName;
    [SerializeField] private TMP_Text maxScoreText;

    [SerializeField] private Button enterSceneButton;
    [SerializeField] private SceneLoader sceneLoader;
    public void SetupStageDisplay(StageData stage)
    {
        checkMarkImage.fillAmount = stage.stageCompletePercentage;

        if (stage.stageCompletePercentage > 0.33f) medalOne.SetActive(true);
        else medalOne.SetActive(false);
        if (stage.stageCompletePercentage > 0.66f) medalTwo.SetActive(true);
        else medalTwo.SetActive(false);
        if (stage.stageCompletePercentage >= 1f) medalThree.SetActive(true);
        else medalThree.SetActive(false);

        stageName.text = stage.stageName;
        maxScoreText.text = stage.maxScore != 0 ? stage.maxScore.ToString() : "Not Played Yet!";
        enterSceneButton.onClick.AddListener(() => sceneLoader.StartScene(stage.stageName));
    }

}
