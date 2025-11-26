using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public PlayerPaddle PlayerPaddle;

    [SerializeField]
    public float BallBaseSpeed = 1.0f;

    [SerializeField]
    public float BallMinSpeed = 1.0f;
    
    [SerializeField]
    public float BallInitialSpeed = 1.0f;
    
    [SerializeField]
    public AudioSource GlobalAudioSource;

    [SerializeField]
    private int actionQueueCooldownFrames = 1;
    
    [SerializeField]
    private PowerupContainer powerupPrefab;

    [SerializeField]
    private Ball ballPrefab;

    [SerializeField]
    private Vector2 defaultBallPosition;

    [SerializeField]
    private TMP_Text scoreText;
    
    [SerializeField]
    private TMP_Text speedText;

    [SerializeField]
    private TMP_Text ballsText;

    [SerializeField]
    private RectTransform restartPanel;

    [SerializeField]
    private Button restartButton;
    

    private bool isGameOver = false;
    private List<Ball> balls = new();
    private float ballSpeed = 1f;
    private int score = 0;
    private Queue<Func<bool>> actionQueue = new();
    private int actionQueueCooldown = 0;

    
    public IEnumerable<Ball> Balls => balls;
    
    public float BallSpeed
    {
        get => ballSpeed;
        set
        {
            ballSpeed = Math.Max(value, BallMinSpeed);
            
            UpdateUi();
        }
    }

    public bool ActionQueueEmpty => actionQueue.Count == 0;
    
    
    private void Start()
    {
        Cursor.visible = false;
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
        
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        
        BallSpeed = BallBaseSpeed;
        SpawnBall(defaultBallPosition, Vector2.down, BallInitialSpeed);
    }

    private void FixedUpdate()
    {
        actionQueueCooldown--;
        
        TryInvokeAction();

        if (!isGameOver && balls.Count == 0)
        {
            GameOver();
        }
    }

    public void SpawnBall(Vector2 position, Vector2 direction, float? speed = null)
    {
        var ball = Instantiate(ballPrefab, position, Quaternion.identity, transform);
        ball.Setup(position, direction, speed);

        balls.Add(ball);
        
        UpdateUi();
    }

    public void BallDestroyed(Ball ball)
    {
        balls.Remove(ball);
        
        UpdateUi();
    }
    
    public void SpawnPowerup(Vector2 position, Powerup powerup)
    {
        var container = Instantiate(powerupPrefab, position, Quaternion.identity, transform);
        container.Setup(powerup);
    }

    public void AddScore(int delta)
    {
        score += delta;
        
        UpdateUi();
    }

    private void UpdateUi()
    {
        scoreText.text = score.ToString();
        speedText.text = ballSpeed.ToString();
        ballsText.text = balls.Count.ToString();
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        isGameOver = true;
        
        restartPanel.gameObject.SetActive(true);
        Cursor.visible = true;
    }
    
    public void QueueAction(Action action)
    {
        actionQueue.Enqueue(() =>
        {
            action.Invoke();

            return true;
        });
    }

    public void QueueAction(Func<bool> action)
    {
        actionQueue.Enqueue(action);
        TryInvokeAction();
    }

    private void TryInvokeAction()
    {
        if (actionQueueCooldown <= 0 && actionQueue.Count > 0)
        {
            var action = actionQueue.Dequeue();
            var success = action.Invoke();

            if (success)
            {
                actionQueueCooldown = actionQueueCooldownFrames;
            }
        }
    }
}
