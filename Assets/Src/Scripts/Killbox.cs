using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Killbox : MonoBehaviour
{
    [Inject]
    private GameController gameController;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }
}
