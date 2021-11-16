using UnityEngine;

public class EnemyBullet : Entity
{
    public Pool<EnemyBullet> pool;
    
    public float speed;
    public float timeToDestroy;
    
    private float _spawnTime;

    private void OnEnable()
    {
        _spawnTime = Time.time;
    }
    
    public void Configure(float speed, float timeToDestroy) 
    {
        this.speed = speed;
        this.timeToDestroy = timeToDestroy;
    }

    private new void Update()
    {
        base.Update();
        
        transform.position += transform.up * (speed * Time.deltaTime);
        
        if (_spawnTime + timeToDestroy <= Time.time)
            DestroyBullet();
    }

    private void DestroyBullet()
    {
        pool.ReturnToPool(this);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Enemy Bullet
        if (other.gameObject.layer == 10)
            DestroyBullet();
    }
    
    public static void TurnOn(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }
    
    public static void TurnOff(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}