using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class Asteroid : Entity, IReminder
{   
    public Pool<Asteroid> pool;

    private Memento<ObjectSnapshot> _memento = new Memento<ObjectSnapshot>();
    
    private void Start()
    {
        EventManager.Instance.Subscribe("OnGameFinished", OnGameFinished);
        EventManager.Instance.Subscribe("OnSave", SaveAsteroid);
        EventManager.Instance.Subscribe("OnRewind", OnRewind);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubscribe("OnGameFinished", OnGameFinished);
        EventManager.Instance.Unsubscribe("OnSave", SaveAsteroid);
        EventManager.Instance.Unsubscribe("OnRewind", OnRewind);
    }

    public void Configure(float speed) 
    {
        AsteroidFlyweightPoint.normal.speed = speed;
    }
	
    protected void Update()
    {
        base.Update();
        
        transform.position += transform.up * (AsteroidFlyweightPoint.normal.speed * Time.deltaTime);
    }
    
    public static void TurnOn(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
    }
    
    public static void TurnOff(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
    }

    public void HitByLaser()
    {
        DestroyAsteroid();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet
        if (other.gameObject.layer == 8)
            DestroyAsteroid();
    }
    
    private void DestroyAsteroid(bool hasScore = true)
    {
        if(hasScore)
            EventManager.Instance.Trigger("OnAsteroidDestroyed", AsteroidFlyweightPoint.normal.points);
        pool.ReturnToPool(this);
    }

    private void OnGameFinished(params object[] parameters)
    {
        DestroyAsteroid(false);
    }

    private void OnRewind(params object[] parameters)
    {
        Rewind();   
    }

    private void SaveAsteroid(params object[] parameters)
    {
        SavestateManager.Instance.saveState.asteroidList.Add(new AsteroidData(this));
    }

    public void MakeSnapshot()
    {
        var snapshot = new ObjectSnapshot();
        snapshot.position = transform.position;
        snapshot.rotation = transform.localRotation;
        
        _memento.Record(snapshot);
    }

    public void Rewind()
    {
        if (!_memento.CanRemember()) 
            return;
        
        var snapshot = _memento.Remember();

        transform.position = snapshot.position;
        transform.rotation = snapshot.rotation;
    }

    public IEnumerator StartToRecord()
    {
        while (true) 
        {
            MakeSnapshot();
            
            yield return new WaitForSeconds(.1f);
        }
    }
}

[Serializable]
public class AsteroidData
{
    public float x;
    public float y;
    public float z;
    public float zRotation;

    public AsteroidData(Asteroid asteroid)
    {
        x = asteroid.transform.position.x;
        y = asteroid.transform.position.y;
        z = asteroid.transform.position.z;
        zRotation = asteroid.transform.eulerAngles.z;
    }
}
