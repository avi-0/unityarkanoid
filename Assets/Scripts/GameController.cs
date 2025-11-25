using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public PlayerPaddle PlayerPaddle;

    [SerializeField]
    public float BallBaseSpeed = 1.0f;

    [SerializeField]
    public float BallMinSpeed = 1.0f;
    
    [SerializeField]
    public AudioSource GlobalAudioSource;

    [SerializeField]
    private int actionQueueCooldownFrames = 1;
    
    private Queue<Func<bool>> actionQueue = new();
    private int actionQueueCooldown = 0;
    
    [SerializeField]
    private PowerupContainer powerupPrefab;

    private float ballSpeed = 1f;

    public float BallSpeed
    {
        get => ballSpeed;
        set
        {
            ballSpeed = Math.Max(value, BallMinSpeed);
        }
    }

    public bool ActionQueueEmpty => actionQueue.Count == 0;

    private void Start()
    {
        BallSpeed = BallBaseSpeed;
    }

    private void FixedUpdate()
    {
        actionQueueCooldown--;
        
        TryInvokeAction();
    }
    
    public void QueueAction(Action action)
    {
        actionQueue.Enqueue(() =>
        {
            action.Invoke();

            return true;
        });
    }

    public void QueueAction(Func<bool> action)
    {
        actionQueue.Enqueue(action);
        TryInvokeAction();
    }

    private void TryInvokeAction()
    {
        if (actionQueueCooldown <= 0 && actionQueue.Count > 0)
        {
            var action = actionQueue.Dequeue();
            var success = action.Invoke();

            if (success)
            {
                actionQueueCooldown = actionQueueCooldownFrames;
            }
        }
    }

    public void SpawnPowerup(Vector2 position, Powerup powerup)
    {
        var container = Instantiate(powerupPrefab, new Vector3(position.x, position.y, 0f), Quaternion.identity, transform);
        container.Setup(powerup);
    }
}
