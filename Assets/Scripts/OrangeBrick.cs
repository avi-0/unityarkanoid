using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            var bricks = overlapColliderResults
                .Select(collider => collider.GetComponent<BaseBrick>())
                .Where(brick => brick != null && brick != this)
                .ToList();
            
            while (bricks.Count > 0)
            {
                var i = Random.Range(0, bricks.Count);
                var brick = bricks[i];
                
                gameController.QueueAction(() =>
                {
                    if (brick == null)
                    {
                        return false;
                    }
                    
                    brick.Damage();
                    return true;
                });

                bricks.RemoveAt(i);
            }
        }
    }

    private void Explode()
    {
        
    }

    protected override void OnDestroyed()
    {
        OnDamaged();
    }
}