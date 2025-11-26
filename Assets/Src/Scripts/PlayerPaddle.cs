using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerPaddle : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    
    [SerializeField]
    private PolygonCollider2D polygonCollider;
    
    [SerializeField]
    private int colliderPoints;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Transform spriteTransform;
    
    [SerializeField]
    private float minX;
    
    [SerializeField]
    private float maxX;
    
    [SerializeField]
    private float defaultLength = 2f;
    
    [SerializeField]
    private float width = 1f;

    [SerializeField]
    private float arcAngle = 15f;

    [SerializeField]
    private float minLength = 1f;
    
    [SerializeField]
    private float tiltRangeMaxSpeed = 8f;
    
    [SerializeField]
    private float tiltRangeMaxAngleDegrees = 30f;
    
    [SerializeField]
    private float tiltDamping = 8f;

    private float length;
    private float targetAngle;

    public float Length
    {
        get => length;
        set
        {
            length = Mathf.Max(value, minLength);

            spriteRenderer.size = new Vector2(length, spriteRenderer.size.y);
            CreateShape();
        }
    }

    private void CreateShape()
    {
        var arcAngleRad = arcAngle * Mathf.Deg2Rad;
        var radius = length / (2 * Mathf.Sin(arcAngleRad));
        
        var vertices = new List<Vector2>();
        for (int i = 0; i <= colliderPoints; i++)
        {
            var angle = 2 * arcAngleRad * (float)i / colliderPoints - arcAngleRad + Mathf.PI / 2;
            var vertex = radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            vertex.y -= radius - width / 2;
            
            vertices.Add(vertex);
        }

        polygonCollider.SetPath(0, vertices);
    }
    
    void Start()
    {
        Length = defaultLength;
    }
    
    void FixedUpdate()
    {
        if (Pointer.current != null)
        {
            var screenPos = Pointer.current.position.value;
            var pos = Camera.main.ScreenToWorldPoint(screenPos);

            var min = minX + Length / 2;
            var max = maxX - Length / 2;
            var x = Mathf.Clamp(pos.x, min, max);

            var prevPosition = body.position;
            var position = new Vector2(x, body.position.y);
            body.MovePosition(position);

            var velocity = (position - prevPosition) / Time.deltaTime;
            var speed = Mathf.Clamp(velocity.x, -tiltRangeMaxSpeed, tiltRangeMaxSpeed);
            var angle = math.remap(-tiltRangeMaxSpeed, tiltRangeMaxSpeed, -tiltRangeMaxAngleDegrees,
                tiltRangeMaxAngleDegrees, speed);

            targetAngle = -angle;
            var currentAngle = spriteTransform.rotation.eulerAngles.z;
            currentAngle = Mathf.LerpAngle(targetAngle, currentAngle, Mathf.Exp(-tiltDamping * Time.deltaTime));
        
            spriteTransform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }
}
