using UnityEngine;

public class Bullet : Entity
{
    public float Speed;
    public float TimeToDestroy;
    public Pool<Bullet> pool;
    
    private float _spawnTime;

    private void OnEnable()
    {
        _spawnTime = Time.time;
    }
    
    public void Configure(float speed) 
    {
        Speed = speed;
    }

    private void Update()
    {
        base.Update();
        
        transform.position += transform.up * (Speed * Time.deltaTime);
        
        if (_spawnTime + TimeToDestroy <= Time.time)
            DestroyBullet();
    }

    private void DestroyBullet()
    {
        pool.ReturnToPool(this);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Asteroid
        if (other.gameObject.layer == 9)
            DestroyBullet();
    }
    
    public static void TurnOn(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    
    public static void TurnOff(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
