using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    void Setup();
    void OnDeath();
    void OnPlayerLoss();
    void OnDamaged();
}
