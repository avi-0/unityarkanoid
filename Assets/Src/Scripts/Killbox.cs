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
        if (other.gameObject.GetComponent<Ball>() is Ball ball)
        {
            gameController.DestroyBall(ball);
        } else {
            Destroy(other.gameObject);
        }
    }
}
