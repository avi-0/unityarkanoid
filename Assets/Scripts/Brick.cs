using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Brick : MonoBehaviour
{
    [Inject]
    private GameController gameController;
    
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
    
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Damage();
    }

    void Damage()
    {
        Health--;
        gameController.GlobalAudioSource.PlayOneShot(impactSound);

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void UpdateSprite()
    {
        var spriteIndex = Math.Clamp(sprites.Count - health, 0, sprites.Count - 1);
        var sprite = sprites[spriteIndex];
        spriteRenderer.sprite = sprite;
    }
}
