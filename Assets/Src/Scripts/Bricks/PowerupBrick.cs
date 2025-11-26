using System.Collections.Generic;
using UnityEngine;

public class PowerupBrick : BaseBrick
{
    [SerializeField]
    private List<Powerup> possiblePowerups;
    
    
    protected override void OnDestroyed()
    {
        var powerup = possiblePowerups[Random.Range(0, possiblePowerups.Count)];
        gameController.SpawnPowerup(transform.position, powerup);
    }
}