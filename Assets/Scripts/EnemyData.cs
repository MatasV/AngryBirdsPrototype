using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Enemy Database", menuName = "New Enemy Database")]
public class EnemyData : ScriptableObject
{
    public Sprite halfHPSprite;
    public Sprite deathSprite;

    public Sprite PlayerLossSprite;
    public Sprite StartingSprite;

    public GameObject deathParticles;

    public float MaxHP = 5f;

    public Sprite ReturnPlayerLossSprite()
    {
        return PlayerLossSprite;
    }
    
    public Sprite ReturnSpriteFromHealth(float health, float maxHealth)
    {
        Sprite sprite = Sprite.Create(new Texture2D(100, 100), new Rect(), Vector2.zero);
        

        if(health <= 0)
        {
            return deathSprite;
        }
        if(health <= maxHealth / 2)
        {
            return halfHPSprite;
        }

        return ReturnInitialSprite();
    }

    public Sprite ReturnInitialSprite()
    {
        Sprite sprite = StartingSprite;
        return sprite;
    }

    public GameObject ReturnDeathParticleSystem()
    {
        return deathParticles;
    }
}
