using UnityEngine;

public class Powerup : ScriptableObject
{
    [SerializeField]
    public string powerupName;

    [SerializeField]
    public bool negative;
    
    public virtual void ApplyEffect(GameController controller)
    {
    }
}
