using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float speed = 4;
    
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
}
