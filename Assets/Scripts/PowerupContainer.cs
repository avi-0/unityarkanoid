using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PowerupContainer : MonoBehaviour
{
    [Inject]
    protected GameController gameController;
    
    [NonSerialized]
    public Powerup Powerup;

    
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private TMP_Text nameText;
    
    [SerializeField]
    private LayerMask paddleLayerMask;

    [SerializeField]
    private float gravity = 1f;

    [SerializeField]
    private float verticalVelocity = 0f;

    [SerializeField]
    private float horizontalVelocitySpread = 0f;
    

    public void Setup(Powerup powerup)
    {
        Powerup = powerup;

        if (Powerup != null)
        {
            nameText.text = Powerup.powerupName;

            if (Powerup.negative)
            {
                nameText.color = Color.red;
            }
        }
        
        var velocity = new Vector2(Random.Range(-horizontalVelocitySpread, horizontalVelocitySpread), verticalVelocity);
        body.AddForce(velocity, ForceMode2D.Impulse);
    }
    
    void FixedUpdate()
    {
        body.AddForce(Vector2.down * gravity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((1 << other.gameObject.layer & paddleLayerMask) != 0)
        {
            if (Powerup != null)
            {
                Powerup.ApplyEffect(gameController);
            }

            Destroy(gameObject);
        }
    }
}
