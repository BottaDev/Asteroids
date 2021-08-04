using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P4 punto 2-2
public class HealPowerUp : IPowerUp
{
    public IPowerUp next;

    public HealPowerUp(IPowerUp _next)
    {
        next = _next;
    }

    public void UsePowerUp(PlayerController player)
    {
        // Heal player

        player.HealPlayer();
        next.UsePowerUp(player);
    }
}
