using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
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

    [FormerlySerializedAs("MinX")]
    [SerializeField]
    private float minX;
    
    [FormerlySerializedAs("MaxX")]
    [SerializeField]
    private float maxX;

    [FormerlySerializedAs("DefaultLength")]
    [SerializeField]
    private float defaultLength = 2;

    [SerializeField]
    private float width = 1;

    [FormerlySerializedAs("TiltRangeMaxSpeed")]
    [SerializeField]
    private float tiltRangeMaxSpeed = 8;

    [FormerlySerializedAs("TiltRangeMaxAngleDegrees")]
    [SerializeField]
    private float tiltRangeMaxAngleDegrees = 30;

    [FormerlySerializedAs("TiltDamping")]
    [SerializeField]
    private float tiltDamping = 8;

    private float length;
    private float targetAngle;

    private float Length
    {
        get => length;
        set
        {
            length = value;

            spriteRenderer.size = new Vector2(length, spriteRenderer.size.y);
            //capsuleCollider.size = new Vector2(length, capsuleCollider.size.y);
            CreateShape();
        }
    }

    private void CreateShape()
    {
        var vertices = new List<Vector2>();
        for (int i = 0; i <= colliderPoints; i++)
        {
            var angle = Mathf.PI * (float)i / colliderPoints;
            var vertex = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            vertex.x *= Length / 2;
            vertex.y *= width;
            vertex.y -= width / 2;
            
            vertices.Add(vertex);
        }

        polygonCollider.SetPath(0, vertices);
    }
    
    void Start()
    {
        Length = defaultLength;
        
        Cursor.visible = false;
    }
    
    void FixedUpdate()
    {
        var screenMousePos = Input.mousePosition;
        var mousePos = Camera.main.ScreenToWorldPoint(screenMousePos);

        var min = minX + Length / 2;
        var max = maxX - Length / 2;
        var x = Mathf.Clamp(mousePos.x, min, max);

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
