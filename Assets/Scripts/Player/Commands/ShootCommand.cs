using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P2
public class ShootCommand : ICommand
{
    public void Execute(params object[] parameters)
    {
        var playerModel = (PlayerModel) parameters[0];
        
        playerModel.weapons[playerModel.currentWeaponIndex].Shoot();
    }
}
