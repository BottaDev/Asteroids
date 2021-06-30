using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerUp
{
    IPowerUp NextPowerUp { get; set; }
    
    void Use(PlayerModel playerModel);
}
