using UnityEngine;

[CreateAssetMenu]
public class ChangeBallSpeedPowerup : Powerup
{
    [SerializeField]
    private float delta = 1f;
    
    public override void ApplyEffect(GameController controller)
    {
        controller.BallSpeed += delta;
    }
}