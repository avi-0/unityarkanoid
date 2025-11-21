using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private CapsuleCollider2D capsuleCollider;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float MinX;
    
    [SerializeField]
    private float MaxX;

    [SerializeField]
    private float DefaultLength = 2;

    [SerializeField]
    private float TiltRangeMaxSpeed = 8;

    [SerializeField]
    private float TiltRangeMaxAngleDegrees = 30;

    [SerializeField]
    private float TiltDamping = 8;

    private float length;
    private float targetAngle;

    private float Length
    {
        get => length;
        set
        {
            length = value;

            spriteRenderer.size = new Vector2(length, spriteRenderer.size.y);
            capsuleCollider.size = new Vector2(length, capsuleCollider.size.y);
        }
    }
    
    void Start()
    {
        Length = DefaultLength;
        
        Cursor.visible = false;
    }
    
    void FixedUpdate()
    {
        var screenMousePos = Input.mousePosition;
        var mousePos = Camera.main.ScreenToWorldPoint(screenMousePos);

        var min = MinX + Length / 2;
        var max = MaxX - Length / 2;
        var x = Mathf.Clamp(mousePos.x, min, max);

        var prevPosition = body.position;
        var position = new Vector2(x, body.position.y);
        body.MovePosition(position);

        var velocity = (position - prevPosition) / Time.deltaTime;
        var speed = Mathf.Clamp(velocity.x, -TiltRangeMaxSpeed, TiltRangeMaxSpeed);
        var angle = math.remap(-TiltRangeMaxSpeed, TiltRangeMaxSpeed, -TiltRangeMaxAngleDegrees,
            TiltRangeMaxAngleDegrees, speed);

        targetAngle = -angle;
        var currentAngle = body.rotation;
        currentAngle = Mathf.LerpAngle(targetAngle, currentAngle, Mathf.Exp(-TiltDamping * Time.deltaTime));
        
        body.MoveRotation(currentAngle);
    }
}
