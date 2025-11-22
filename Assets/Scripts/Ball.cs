using System;
using UnityEngine;
using Zenject;


public class Ball : MonoBehaviour
{
    [Inject]
    private GameController gameController;
    
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float speed = 4;

    [SerializeField]
    private LayerMask wallsLayerMask;

    [SerializeField]
    private AudioClip bounceSound;
    
    [SerializeField]
    private float bounceSoundVolume = 1.0f;
    
    void Start()
    {
        body.velocity = new Vector2(2, 1);
    }
    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        body.velocity = body.velocity.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((1 << other.gameObject.layer & wallsLayerMask ) != 0)
        {
            gameController.GlobalAudioSource.PlayOneShot(bounceSound, bounceSoundVolume);
        } 
    }
}
