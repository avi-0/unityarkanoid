using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OrangeBrick : BaseBrick
{
    [SerializeField]
    private Collider2D explosionAreaCollider;

    [SerializeField]
    private ContactFilter2D contactFilter;

    [SerializeField]
    private AudioClip explosionSound;
    
    [SerializeField]
    private float explosionSoundVolume;

    private List<Collider2D> overlapColliderResults = new();

    private bool canExplode = true;

    protected override bool CanBeDamaged => canExplode;

    void FixedUpdate()
    {
        if (!canExplode && gameController.ActionQueueEmpty)
        {
            canExplode = true;
        }
    }

    protected override void OnDamaged()
    {
        if (canExplode)
        {
            canExplode = false;
            
            gameController.GlobalAudioSource.PlayOneShot(explosionSound, explosionSoundVolume);
        
            Physics2D.OverlapCollider(explosionAreaCollider, contactFilter, overlapColliderResults);
            foreach (var collider in overlapColliderResults)
            {
                var brick = collider.GetComponent<BaseBrick>();
                if (brick != null && brick != this)
                {
                    gameController.QueueAction(() =>
                    {
                        if (brick == null)
                        {
                            return false;
                        }
                    
                        brick.Damage();
                        return true;
                    });
                }
            }
        }
    }

    protected override void OnDestroyed()
    {
        OnDamaged();
    }
}