using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Heal : PowerUp
{
    protected override void Use(PlayerModel playerModel)
    {
        playerModel.HealPlayer();
        base.Use(playerModel);
    }
}
