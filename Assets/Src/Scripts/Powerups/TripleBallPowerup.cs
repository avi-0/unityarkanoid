using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class TripleBallPowerup : Powerup
{
    public override void ApplyEffect(GameController controller)
    {
        foreach (var ball in controller.Balls.ToList())
        {
            var velocity = ball.Velocity;
            controller.SpawnBall(ball.Position, Quaternion.AngleAxis(120, Vector3.forward) * velocity, velocity.magnitude);
            controller.SpawnBall(ball.Position, Quaternion.AngleAxis(-120, Vector3.forward) * velocity, velocity.magnitude);
        }
    }
}
