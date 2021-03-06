using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Entity : MonoBehaviour, IGridEntity
{
    public event Action<IGridEntity> OnMove;
    
    protected virtual void Start()
    {
        SpatialGrid grid = FindObjectOfType<SpatialGrid>();
        grid.Add(this);
    }

    protected void Update()
    {
        CheckBounds();
        OnMove?.Invoke(this);
    }
    
    public virtual void HitByLaser() { }

    public virtual void HitByBomb() { }
    
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
    
    public Vector3 Position 
    {
        get => transform.position;
        set => transform.position = value;
    }
}
