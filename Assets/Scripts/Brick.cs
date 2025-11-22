using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Brick : MonoBehaviour
{
    private static readonly int Hit = Animator.StringToHash("hit");

    
    [Inject]
    private GameController gameController;
    
    
    [SerializeField]
    private Animator animator;
    
    [SerializeField]
    private List<Sprite> sprites;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private AudioClip impactSound;

    
    private int health;

    
    private int Health
    {
        get => health;
        set
        {
            health = value;
            
            UpdateSprite();
        }
    }
    
    
    void Start()
    {
        Health = sprites.Count;
        
        UpdateSprite();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameController.QueueAction(Damage);
    }

    void Damage()
    {
        Health--;
        gameController.GlobalAudioSource.PlayOneShot(impactSound);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger(Hit);
        }
    }

    void UpdateSprite()
    {
        var spriteIndex = Math.Clamp(sprites.Count - health, 0, sprites.Count - 1);
        var sprite = sprites[spriteIndex];
        spriteRenderer.sprite = sprite;
    }
}
