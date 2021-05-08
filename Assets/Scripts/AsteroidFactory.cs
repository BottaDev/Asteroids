using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory : IFactory<Asteroid, Vector2>
{
    public GameObject asteroidPrefab;

    public Asteroid Create(Vector2 playerPos)
    {
        GameObject asteroid = GameObject.Instantiate(asteroidPrefab);
        
        RotateAsteroid(asteroid, playerPos);

        return asteroid.GetComponent<Asteroid>();
    }
    
    /// <summary>
    /// Rotates the asteroid towards the player direction
    /// </summary>
    /// <param name="asteroid"></param>
    /// <param name="playerPos"></param>
    private void RotateAsteroid(GameObject asteroid, Vector2 playerPos)
    {
        playerPos.Normalize();
        
        float rot_z = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
        asteroid.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
