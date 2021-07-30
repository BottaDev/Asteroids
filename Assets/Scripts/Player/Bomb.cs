using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P3
public class Bomb : MonoBehaviour
{
    public Pool<Bomb> pool;

    public void Explode()
    {
        DestroyBomb();
    }
    
    private void DestroyBomb()
    {
        pool.ReturnToPool(this);
    }

    public static void TurnOn(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
    }
    
    public static void TurnOff(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
    }
}
