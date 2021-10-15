using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IGridEntity
{
    protected void Update()
    {
        CheckBounds();
    }
    
    private void CheckBounds()
    {
        if (transform.position.y > LevelManager.instance.globalYLimit / 2) 
            transform.position = new Vector2(transform.position.x, -LevelManager.instance.globalYLimit / 2);
        
        if (transform.position.y < -LevelManager.instance.globalYLimit / 2) 
            transform.position = new Vector2(transform.position.x, LevelManager.instance.globalYLimit / 2);

        if (transform.position.x > LevelManager.instance.globalXLimit / 2)
            transform.position = new Vector2(-LevelManager.instance.globalXLimit / 2, transform.position.y);
        
        if (transform.position.x < -LevelManager.instance.globalXLimit / 2) 
            transform.position = new Vector2(LevelManager.instance.globalXLimit / 2, transform.position.y);
    }

    public event Action<IGridEntity> OnMove;
    public Vector3 Position 
    {
        get => transform.position;
        set => transform.position = value;
    }
}
