using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float MinX;
    
    [SerializeField]
    private float MaxX;
    
    void Start()
    {
        Cursor.visible = false;
    }
    
    void Update()
    {
        var screenMousePos = Input.mousePosition;
        var mousePos = Camera.main.ScreenToWorldPoint(screenMousePos);

        var x = Mathf.Clamp(mousePos.x, MinX, MaxX);
        body.MovePosition(new Vector2(x, body.position.y));
    }
}
