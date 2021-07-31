using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P3
public class BombFactory : IFactory<Bomb>
{
    public Bomb Create()
    {
        var bomb = ResourceManager.instance.ResourceTable.GetValue("Bomb").GetComponent<Bomb>();

        return Object.Instantiate(bomb);
    }
}
