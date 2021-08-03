using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P4 punto 2-2
public class RewindPowerUp : IPowerUp
{
    public IPowerUp next;

    public RewindPowerUp(IPowerUp _next)
    {
        next = _next;
    }

    public void UsePowerUp(PlayerController player)
    {
        player.Rewind();
        next.UsePowerUp(player);
    }
}
