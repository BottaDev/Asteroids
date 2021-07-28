using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P3
public class BombBuilder
{
    public Bomb Build()
    {
        BombFactory factory = new BombFactory();
        Bomb bomb = factory.Create();

        return bomb;
    }
}
