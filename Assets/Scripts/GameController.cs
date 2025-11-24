using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public AudioSource GlobalAudioSource;

    [SerializeField]
    private int actionQueueCooldownFrames = 1;
    
    private Queue<Func<bool>> actionQueue = new();
    private int actionQueueCooldown = 0;

    public bool ActionQueueEmpty => actionQueue.Count == 0;
    
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
}
