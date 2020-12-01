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

    private void Start()
    {
        holder.SetActive(false);

        medalOne.SetActive(false);
        medalTwo.SetActive(false);
        medalThree.SetActive(false);

        if(ScoreManager.instance != null) ScoreManager.instance.onStageEnded += Init;
    }
    public void Init(string stageName, float completionPercentage, int maxScore)
    {
        if (completionPercentage < 1f) titleText.text = "Lost :(";
        else titleText.text = "Win!";

        holder.SetActive(true);
        Debug.Log(completionPercentage);
        if (completionPercentage >= 0.33f) medalOne.SetActive(true);
        if (completionPercentage >= 0.66f) medalTwo.SetActive(true);
        if (completionPercentage >= 1f) medalThree.SetActive(true);

        scoreText.text = "Score: " + maxScore.ToString();
    }
    public void GoToSceneLoader()
    {
        SceneLoader.instance?.StartScene("SceneLoader");
    }
    public void Restart()
    {
        SceneLoader.instance?.StartScene(SceneManager.GetActiveScene().name);
    }
}
