using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] private StageManager stageManager;
    [SerializeField] private StageBirdData stageBirds;
    
    public List<Rigidbody2D> movingEntities = new List<Rigidbody2D>();
    private void Awake()
    {
        stageManager.entityController = this;
        stageManager.birdPrefabs = stageBirds.birds.ToArray();
    }

    private void Update()
    {
        stageManager.Update();
    }
}
    

