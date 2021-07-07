using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IReminder
{
    [HideInInspector]
    public Pool<Bullet> bulletPool;

    private Rigidbody2D _rb;
    private PlayerModel _playerModel;
    private float _auxAxisX;
    private float _auxAxisY;
    private Memento<ObjectSnapshot> _memento = new Memento<ObjectSnapshot>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _playerModel = GetComponent<PlayerModel>();
        
        _playerModel.currentFireRate = 0;
    }

    private void Start()
    {
        MementoManager.instance.Add(this);
        
        BulletBuilder builder = new BulletBuilder();
        builder.SetSpeed(_playerModel.bulletSpeed);
        
        bulletPool = new Pool<Bullet>(builder.Build, Bullet.TurnOn, Bullet.TurnOff, 5);
        
        _playerModel.currentFireRate = 0;
        
        EventManager.Instance.Subscribe("OnRewind", OnRewind);
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
    }

    private void FixedUpdate()
    {
        _auxAxisX = Input.GetAxis(_playerModel.inputAxisX);
        _auxAxisY = Input.GetAxis(_playerModel.inputAxisY);

        Move();
    }

    private void Update()
    {
        if (_auxAxisX != 0)
            transform.Rotate(Vector3.forward * _playerModel.RotationSpeed * Time.deltaTime * -_auxAxisX);
            
        if (Input.GetKey(KeyCode.Space) && _playerModel.currentFireRate <= 0)
            _playerModel.weapons[_playerModel.currentWeaponIndex].Shoot();
        else if (Input.GetKeyDown(KeyCode.E))
            NextWeapon();
        else
            _playerModel.currentFireRate -= Time.deltaTime;
    }
    
    public void NextWeapon()
    {
        _playerModel.currentWeaponIndex++;
        if (_playerModel.currentWeaponIndex >= _playerModel.weapons.Count)
            _playerModel.currentWeaponIndex = 0;
    }

    private void Move()
    {
        if (_auxAxisY > 0)
        {
            StopCoroutine(Decelerate());

            _rb.AddForce(transform.up * _playerModel.Speed );
            _rb.velocity = new Vector2(Mathf.Clamp(_rb.velocity.x, -_playerModel.MaxSpeed, _playerModel.MaxSpeed), 
                                        Mathf.Clamp(_rb.velocity.y, -_playerModel.MaxSpeed, _playerModel.MaxSpeed));

            EventManager.Instance.Trigger("OnPlayerMove", _playerModel.spriteFire);
        }
        else
        {
            StartCoroutine(Decelerate());
        }
    }

    private IEnumerator Decelerate()
    {
        float t = 0;
        Vector3 fromVelocity = _rb.velocity;

        while (t < _playerModel.decelerationTime)
        {
            _rb.velocity = Vector3.Lerp(fromVelocity, Vector3.zero, t);
            t += Time.deltaTime / _playerModel.decelerationTime;

            yield return null;
        }
    }
    
    public IEnumerator StartToRecord() 
    {
        while (true) 
        {
            MakeSnapshot();
            
            yield return new WaitForSeconds(.1f);
        }
    }
    
    public void Rewind() 
    {
        if (!_memento.CanRemember()) 
            return;
        
        EventManager.Instance.Trigger("OnRewind");
    }

    private void OnRewind(params object[] parameters)
    {
        var snapshot = _memento.Remember();

        transform.position = snapshot.position;
        transform.rotation = snapshot.rotation;
    }
    
    public void MakeSnapshot() 
    {
        var snapshot = new ObjectSnapshot();
        snapshot.position = transform.position;
        snapshot.rotation = transform.localRotation;
        
        _memento.Record(snapshot);
    }
    
    private void OnPlayerDamaged(params object[] parameters)
    {
        if (_playerModel.lifes == 0)
        {
            gameObject.SetActive(false);
            EventManager.Instance.Unsubscribe("OnPlayerDamaged", OnPlayerDamaged);
            EventManager.Instance.Trigger("OnGameFinished");
            EventManager.Instance.Trigger("OnPlayerDead");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Asteroid
        if (other.gameObject.layer == 9)
        {
            _playerModel.lifes--;
            EventManager.Instance.Trigger("OnPlayerDamaged", _playerModel.lifes);
        }
        
        // PowerUp
        if (other.gameObject.layer == 11)
        {
            Rewind();
        }
    }
}
