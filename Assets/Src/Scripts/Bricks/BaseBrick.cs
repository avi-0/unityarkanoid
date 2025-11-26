using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseBrick : MonoBehaviour
{
    private static readonly int Hit = Animator.StringToHash("hit");
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");


    [Inject]
    protected GameController gameController;
    
    
    [SerializeField]
    private Animator animator;
    
    [SerializeField]
    private List<Sprite> sprites;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    private ParticleSystem debrisParticleSystem;
    
    [SerializeField]
    private Renderer debrisParticleRenderer;
    
    [SerializeField]
    private Texture2D debrisSprite;

    [SerializeField]
    private AudioClip impactSound;
    
    [SerializeField]
    private int score = 10;

    
    private int health;
    private bool beingDestroyed = false;

    
    protected int Health
    {
        get => health;
        set
        {
            health = value;
            
            UpdateSprite();
        }
    }

    protected virtual bool CanBeDamaged => true;
    
    
    private void Start()
    {
        Health = sprites.Count;
        
        UpdateSprite();
        UpdateDebrisSprite();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameController.QueueAction(Damage);
    }

    public void Damage()
    {
        if (!CanBeDamaged || beingDestroyed)
        {
            return;
        }
        
        Health--;
        gameController.GlobalAudioSource.PlayOneShot(impactSound);
        debrisParticleSystem.Play();

        if (Health <= 0)
        {
            gameController.AddScore(score);
            OnDestroyed();
            
            debrisParticleSystem.transform.parent = null;

            var main = debrisParticleSystem.main;
            main.stopAction = ParticleSystemStopAction.Destroy;

            beingDestroyed = true;
            Destroy(gameObject);
        }
        else
        {
            animator.SetTrigger(Hit);
            
            OnDamaged();
        }
    }

    protected virtual void OnDamaged()
    {
    }
    
    protected virtual void OnDestroyed()
    {
    }

    private void UpdateSprite()
    {
        var spriteIndex = Math.Clamp(sprites.Count - health, 0, sprites.Count - 1);
        var sprite = sprites[spriteIndex];
        spriteRenderer.sprite = sprite;
    }

    private void UpdateDebrisSprite()
    {
        var block = new MaterialPropertyBlock();
        block.SetTexture(MainTex, debrisSprite);
        debrisParticleRenderer.SetPropertyBlock(block);
    }
}
