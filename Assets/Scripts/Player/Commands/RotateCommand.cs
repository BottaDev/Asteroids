using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P2
public class RotateCommand : ICommand
{
    public void Execute(params object[] parameters)
    {
        var transform = (Transform) parameters[0];
        var playerModel = (PlayerModel) parameters[1];
        var auxAxisX = (float) parameters[2];
        
        transform.Rotate(Vector3.forward * playerModel.RotationSpeed * Time.deltaTime * -auxAxisX);
    }
}
