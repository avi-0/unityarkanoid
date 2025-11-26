using UnityEngine;

[CreateAssetMenu]
public class ChangePaddleSizePowerup : Powerup
{
    [SerializeField]
    private float delta = 1f;
    
    public override void ApplyEffect(GameController controller)
    {
        controller.PlayerPaddle.Length += delta;
    }
}