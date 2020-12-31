using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class StageManager : ScriptableObject
{
    public delegate void OnBirdCountChanged(Queue<GameObject> birdQueue);
    public static OnBirdCountChanged onBirdCountChanged;

    public delegate void OnBirdLaunched(GameObject bird);
    public static OnBirdLaunched onBirdLaunched;
    
    [HideInInspector] public Sling sling;
    [HideInInspector] public GameObject[] birdPrefabs;
    private Queue<GameObject> birdQueue = new Queue<GameObject>();

    private Bird currentBird = null;
    private int startingEnemies = 0;

    public bool DebugMode = false;

    [SerializeField] private ScoreManager scoreManager;
    [HideInInspector] public EntityController entityController = null;
    private void BirdCountChanged()
    {
        onBirdCountChanged?.Invoke(birdQueue);
    }

    public void StartUp(Sling sling)
    {
        this.sling = sling;
        onBirdLaunched += Launched;
        Setup();
        SpawnNextBird();
    }

    public bool AnyBirdsLeft => birdQueue.Count > 0;
    
    private void SpawnNextBird()
    {
        var birdGO = Instantiate(birdQueue.Dequeue(), sling.transform.position, Quaternion.identity);
        var bird = birdGO.GetComponent<Bird>();
        bird.Setup(this);
        currentBird = bird;
        sling.birdTransform = bird.transform;
        BirdCountChanged();
    }
    private void Setup(){
    
        birdQueue.Clear();
        for (int i = 0; i < birdPrefabs.Length; i++)
        {
            birdQueue.Enqueue(birdPrefabs[i]);
        }
        startingEnemies = 0;
    }
    
    public void Launched(GameObject obj)
    {
        sling.StopRenderingLines();
        sling.launchTransform.gameObject.SetActive(false);
        entityController.StartVelocityChecking();
    }

    public void Win()
    {
        scoreManager.ReportScore(startingEnemies, entityController.activeEnemies.Count);
    }
    
    public void UnregisterMovingEntity([NotNull] Rigidbody2D rb)
    {
        entityController.movingEntities.Remove(rb);
    }
    
    public void RegisterActiveEnemy([NotNull] Enemy enemy)
    {
        entityController.activeEnemies.Add(enemy);
        ++startingEnemies;
    }
    public void RegisterMovingEntity([NotNull] Rigidbody2D rb)
    {
        entityController.movingEntities.Add(rb);
    }

    public void Continue()
    {
        if (currentBird != null)
        {
            UnregisterMovingEntity(currentBird.gameObject.GetComponent<Rigidbody2D>());
            Destroy(currentBird.gameObject);
        }
        sling.launchTransform.gameObject.SetActive(true);
        SpawnNextBird();
    }

    public void Lose()
    {
        scoreManager.ReportScore(startingEnemies, entityController.activeEnemies.Count);
    }

    public void UnregisterActiveEnemy([NotNull] Enemy enemy)
    {
        entityController.activeEnemies.Remove(enemy);
    }
   
}
