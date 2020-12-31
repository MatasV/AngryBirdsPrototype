using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;

    public List<Rigidbody2D> movingEntities = new List<Rigidbody2D>();
    public List<Enemy> activeEnemies = new List<Enemy>();

    [SerializeField] private StageBirdData stageBirds;
    private void Awake()
    {
        stageManager.entityController = this;
        stageManager.birdPrefabs = stageBirds.birds.ToArray();
    }

    public void StartVelocityChecking()
    {
        StartCoroutine(nameof(CheckVelocityForRoundEnd));
    }
    private IEnumerator CheckVelocityForRoundEnd()
    {
        Debug.Log("cHECKING");
        var movingEntityCount = 0;

        foreach (var movingEntity in movingEntities)
        {
            if (Math.Abs(movingEntity.velocity.magnitude) > 0.2f || movingEntity.angularVelocity > 0.2f)
            {
                movingEntityCount++;
            }
        }

        if (movingEntityCount < 1)
        {
            yield return new WaitForSeconds(2f);

            movingEntityCount = 0;

            foreach (var movingEntity in movingEntities)
            {
                if (movingEntity.velocity.magnitude > 0.2f || movingEntity.angularVelocity > 0.2f)
                {
                    movingEntityCount++;
                }
            }

            if (movingEntityCount < 1) //game ended, lets check results
            {
                StopAllCoroutines();
                CheckWinConditions();
            }
        }

        yield return new WaitForSeconds(2f);
        
        StartCoroutine(CheckVelocityForRoundEnd());
    }
    
    public void CheckWinConditions()
    {
        Debug.Log("CheckWinConditions");

        if (activeEnemies.Count > 0 && stageManager.AnyBirdsLeft || stageManager.sling.IsBirdOnSling())
        {
            stageManager.Continue();
        }
        else if(activeEnemies.Count > 0 && !stageManager.AnyBirdsLeft && !stageManager.sling.IsBirdOnSling())
        {
            stageManager.Lose();
        }
        else
        {
            stageManager.Win();
        }
    }
}
    

