using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteFactory : IFactory<SateliteEnemy>
{
    public SateliteEnemy Create()
    {
        SateliteEnemy satelite = ResourceManager.instance.ResourceTable.GetValue("Satelite").GetComponent<SateliteEnemy>();

        return Object.Instantiate(satelite);
    }

}