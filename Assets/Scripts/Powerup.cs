using UnityEngine;

public class Powerup : ScriptableObject
{
    [SerializeField]
    public string powerupName;
    
    public virtual void ApplyEffect(GameController controller)
    {
    }
}
