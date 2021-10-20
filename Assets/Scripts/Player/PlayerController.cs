using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IReminder
{
    public Pool<Bullet> bulletPool;
    public Pool<Bomb> bombPool;
    
    [HideInInspector]
    public List<Bomb> activeBombs;
    [HideInInspector]
    public bool exploding;
    
    private Rigidbody2D _rb;
    private PlayerModel _playerModel;
    private float _auxAxisX;
    private float _auxAxisY;
    private Memento<ObjectSnapshot> _memento = new Memento<ObjectSnapshot>();
    private MoveCommand _moveCommand = new MoveCommand();
    private ShootCommand _shootCommand = new ShootCommand();
    private ChangeWeaponCommand _weaponCommand = new ChangeWeaponCommand();
    private RotateCommand _rotateCommand = new RotateCommand();
    private ExplodeCommand _explodeCommand = new ExplodeCommand();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _playerModel = GetComponent<PlayerModel>();
        
        _playerModel.currentFireRate = 0;
    }

    private void Start()
    {
        MementoManager.instance.Add(this);
        
        BulletBuilder bulletBuilder = new BulletBuilder();
        bulletBuilder.SetSpeed(_playerModel.bulletSpeed);
        
        bulletPool = new Pool<Bullet>(bulletBuilder.Build, Bullet.TurnOn, Bullet.TurnOff, 5);
        
        //MyA1-P3
        BombBuilder bombBuilder = new BombBuilder();
        bombBuilder.Configure(_playerModel.chainTime, _playerModel.radius);
        bombPool = new Pool<Bomb>(bombBuilder.Build, Bomb.TurnOn, Bomb.TurnOff, 5);
        
        _playerModel.currentFireRate = 0;
        
        EventManager.Instance.Subscribe("OnRewind", OnRewind);
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
        EventManager.Instance.Subscribe("OnPlayerHealed", OnPlayerHealed);
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
            _rotateCommand.Execute(transform, _playerModel, _auxAxisX);     //MyA1-P2
            
        if (Input.GetKey(KeyCode.Space) && _playerModel.currentFireRate <= 0)
            _shootCommand.Execute(_playerModel);    //MyA1-P2
        else if (Input.GetKeyDown(KeyCode.E))
            _weaponCommand.Execute(_playerModel);   //MyA1-P2
        else if (Input.GetKeyDown(KeyCode.Q) && !exploding)
            _explodeCommand.Execute(this);     //MyA1-P2 and MyA1-P3
        else
            _playerModel.currentFireRate -= Time.deltaTime;

    }

    private void Move()
    {
        if (_auxAxisY > 0)
        {
            StopCoroutine(Decelerate());
            
            //MyA1-P2
            _moveCommand.Execute(transform, _rb, _playerModel);

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
            EventManager.Instance.Unsubscribe("OnPlayerHealed", OnPlayerHealed);
            EventManager.Instance.Trigger("OnGameFinished");
            EventManager.Instance.Trigger("OnPlayerDead");
        }
    }

    public void HealPlayer()
    {
        if (_playerModel.lifes < 3)
            _playerModel.lifes++;
        
        EventManager.Instance.Trigger("OnPlayerHealed", _playerModel.lifes);
    }
    
    private void OnPlayerHealed(params object[] parameters)
    {
        // Do nothing...
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Asteroid, Enemy
        if (other.gameObject.layer == 9 || other.gameObject.layer == 13)
        {
            _playerModel.lifes--;
            EventManager.Instance.Trigger("OnPlayerDamaged", _playerModel.lifes);
        }
    }
}
