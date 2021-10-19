using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFactory : IFactory<ShipEnemy>
{
    public ShipEnemy Create()
    {
        ShipEnemy ship = ResourceManager.instance.ResourceTable.GetValue("Ship").GetComponent<ShipEnemy>();

        return Object.Instantiate(ship);
    }
}
