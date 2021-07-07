using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpFactory : IFactory<PowerUp>
{
    public PowerUp Create()
    {
        int random = Random.Range(0, 2);
        
        PowerUp powerUp = null;
        
        if (random == 0)
            powerUp = ResourceManager.instance.ResourceTable.GetValue("Rewind").GetComponent<PowerUp>();
        else if (random == 1)
            powerUp = ResourceManager.instance.ResourceTable.GetValue("Heal").GetComponent<PowerUp>();

        return Object.Instantiate(powerUp);
    }
}
