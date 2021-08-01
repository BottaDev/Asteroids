using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P2
public class ChangeWeaponCommand : ICommand
{
    public void Execute(params object[] parameters)
    {
        var playerModel = (PlayerModel) parameters[0];
        
        playerModel.currentWeaponIndex++;
        if (playerModel.currentWeaponIndex >= playerModel.weapons.Count)
            playerModel.currentWeaponIndex = 0;
    }
}
