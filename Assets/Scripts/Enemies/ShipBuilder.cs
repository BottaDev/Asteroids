using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuilder 
{
    public ShipEnemy Build()
    {
        ShipFactory factory = new ShipFactory();
        ShipEnemy ship = factory.Create();

        return ship;
    }
}
