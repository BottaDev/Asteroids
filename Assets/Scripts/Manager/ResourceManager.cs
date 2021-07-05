using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance = null;
    
    public LookupTable<string, GameObject> ResourceTable;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        ResourceTable = new LookupTable<string, GameObject>(GetFromResources);
    }
    
    public static GameObject GetFromResources(string objName)
    {
        GameObject newObj = new GameObject();
        
        switch (objName)
        {
            case "Bullet":
                newObj = Resources.Load<GameObject>("Prefabs/Bullet");
                break;
                
            case "SmallAsteroid":
                newObj = Resources.Load<GameObject>("Prefabs/Asteroid (Small)");
                break;;
                
            case "MediumAsteroid":
                newObj  = Resources.Load<GameObject>("Prefabs/Asteroid (Medium)");
                break;;
                
            case "LargeAsteroid":
                newObj = Resources.Load<GameObject>("Prefabs/Asteroid (Large)");
                break;;
            
            case "Rewind":
                newObj = Resources.Load<GameObject>("Prefabs/Rewind");
                break;
        }

        return newObj;
    }
}
