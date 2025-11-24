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
    private float speed = 4;

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
    
    void Start()
    {
        body.velocity = new Vector2(1, 1);
    }

    private void FixedUpdate()
    {
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

        // защита для случая, когда скорость почти горизонтальна/вертикальна и шарик очень долго скачет между стенами
        var angleToX = Vector2.SignedAngle(body.velocity, Vector2.right);
        angleToX = Math.Min(angleToX, 180 - angleToX);
        if (angleToX < minAngleToXAxis)
        {
            var angle = Random.Range(-bounceDeviationAngle, bounceDeviationAngle);
            body.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * body.velocity;
        }
    }
}
