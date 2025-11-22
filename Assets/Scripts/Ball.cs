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
    private float bounceSoundVolume = 1f;
    
    [SerializeField]
    private float bounceDeviationAngle = 5f;
    
    void Start()
    {
        body.velocity = new Vector2(2, 1);
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

        var angle = Random.Range(-bounceDeviationAngle, bounceDeviationAngle);
        body.velocity = Quaternion.AngleAxis(angle, Vector3.forward) * body.velocity;
    }
}
