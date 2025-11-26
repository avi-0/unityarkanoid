using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


public class Ball : MonoBehaviour
{
    [Inject]
    private GameController gameController;
    
    
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float SpeedChangeAcceleration = 1f;

    [SerializeField]
    private LayerMask wallsLayerMask;

    [SerializeField]
    private AudioClip bounceSound;
    
    [SerializeField]
    private float bounceSoundVolume = 1f;
    
    [SerializeField]
    private float bounceDeviationAngle = 5f;

    [SerializeField]
    private float minAngleToXAxis = 5f;
    

    public Vector2 Position => body.position;
    public Vector2 Velocity => body.velocity;
    
    
    public void Setup(Vector2 position, Vector2 direction, float? speed = null)
    {
        body.position = position;
        body.velocity = direction.normalized * (speed ?? gameController.BallSpeed);
    }

    private void FixedUpdate()
    {
        var speed = body.velocity.magnitude;
        speed = Mathf.MoveTowards(speed, gameController.BallSpeed, SpeedChangeAcceleration * Time.fixedDeltaTime);
        body.velocity = body.velocity.normalized * speed;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if ((1 << other.gameObject.layer & wallsLayerMask ) != 0)
        {
            Bounced();
        }
    }

    private void Bounced()
    {
        gameController.GlobalAudioSource.PlayOneShot(bounceSound, bounceSoundVolume);

        body.velocity = body.velocity.normalized * gameController.BallSpeed;

        // защита для случая, когда скорость почти горизонтальна/вертикальна и шарик очень долго скачет между стенами
        var angleToX = Vector2.SignedAngle(body.velocity, Vector2.right);
        angleToX = Math.Min(angleToX, 180 - angleToX);
        if (angleToX < minAngleToXAxis)
        {
            var angle = bounceDeviationAngle * Math.Sign(angleToX);
            body.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * body.velocity;
        }
    }

    private void OnDestroy()
    {
        gameController.BallDestroyed(this);
    }
}
