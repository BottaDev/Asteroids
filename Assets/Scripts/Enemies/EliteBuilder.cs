using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBuilder
{
    public EliteEnemy Build()
    {
        EliteFactory factory = new EliteFactory();
        EliteEnemy ship = factory.Create();

        return ship;
    }
}
