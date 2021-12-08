using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteFactory : IFactory<EliteEnemy>
{
    public EliteEnemy Create()
    {
        EliteEnemy satelite = ResourceManager.instance.ResourceTable.GetValue("Elite").GetComponent<EliteEnemy>();

        return Object.Instantiate(satelite);
    }
}
