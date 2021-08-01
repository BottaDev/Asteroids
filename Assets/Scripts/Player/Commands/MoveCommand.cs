using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P2
public class MoveCommand : ICommand
{
    public void Execute(params object[] parameters)
    {
        var transform = (Transform) parameters[0];
        var rb = (Rigidbody2D) parameters[1];
        var playerModel = (PlayerModel) parameters[2];
        
        rb.AddForce(transform.up * playerModel.Speed );
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -playerModel.MaxSpeed, playerModel.MaxSpeed), 
            Mathf.Clamp(rb.velocity.y, -playerModel.MaxSpeed, playerModel.MaxSpeed));
    }
}
