using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird_Yellow : Bird
{
    [SerializeField] private GameObject birdCloneGO;

    protected sealed override void ActivateSpecial()
    {
        if (specialActivated) return;
        specialActivated = true;
        Instantiate(birdCloneGO, transform.position, transform.rotation).GetComponent<Bird_Yellow_Clone>().Init(rigidBody);
        
    }
}
