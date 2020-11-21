using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{


    public static ScoreManager instance;

    [SerializeField] private ScoreDatabase scoreDatabase;

    private int currentScore = 0;

    public int CurrentScore {  get { return currentScore;  } set { currentScore = value; ChangeScore(); } }

    public delegate void ScoreChanged(int score);
    public ScoreChanged onScoreChanged;


    public void ChangeScore()
    {
        onScoreChanged.Invoke(currentScore);

    }

    public void EnemyDamaged(float damage)
    {
        CurrentScore += (int)(scoreDatabase.damageToScoreMultiplier * damage);
    }
    public void EnemyKilled(EnemyType enemy)
    {
        int value;
        if (scoreDatabase.deadEnemyToScorePairs.TryGetValue(enemy, out value) == false) return;

        CurrentScore += value;
    }
    private void Awake()
    {
        instance = this;
    }
}
