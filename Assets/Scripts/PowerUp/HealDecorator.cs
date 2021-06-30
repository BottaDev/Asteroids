using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealDecorator : PowerUp
{
    public IPowerUp NextPowerUp { get; set; }
    
    public void Use(PlayerModel playerModel)
    {
        playerModel.lifes++;
        
        NextPowerUp.Use(playerModel);
    }
}
