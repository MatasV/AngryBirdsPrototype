using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class ScoreManager : ScriptableObject
{
    [SerializeField] private LevelDataController levelDataController;
    [SerializeField] private ScoreDatabase scoreDatabase;

    private int currentScore = 0;
    public int CurrentScore {  get { return currentScore;  } set { currentScore = value; ChangeScore(); } }

    private string currentStageName;
    public string CurrentStageName { get { return currentStageName; } set { currentStageName = value; Debug.Log($"Going To {value} "); } }

    public delegate void ScoreChanged(int score);
    public static ScoreChanged onScoreChanged;

    public delegate void OnStageEnded(string stageName, float percentageCompleted, int maxScore);
    public static OnStageEnded onStageEnded;

    public void ChangeScore()
    {
        onScoreChanged?.Invoke(currentScore);
    }

    public void EnemyDamaged(float damage)
    {
        CurrentScore += (int)(scoreDatabase.damageToScoreMultiplier * damage);
    }

    public void EnemyKilled(EnemyType enemy)
    {
        if (scoreDatabase.deadEnemyToScorePairs.TryGetValue(enemy, out int value) == false) return;
        CurrentScore += value;
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += SetCurrentStageName;
    }

    private void SetCurrentStageName(Scene scene, LoadSceneMode sceneLoadMode)
    {
        CurrentStageName = scene.name;
    }

    public void ReportScore(int startingEnemies,int currentEnemies)
    {
        onStageEnded?.Invoke(CurrentStageName, 1f-(float)currentEnemies / (float)startingEnemies, CurrentScore);
    }

}
