using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//MyA1-P2
public class ExplodeCommand : ICommand
{
    public void Execute(params object[] parameters)
    {
        PlayerController playerController = (PlayerController) parameters[0];

        playerController.exploding = true;
        playerController.activeBombs[0].Explode(playerController);
    }
}
