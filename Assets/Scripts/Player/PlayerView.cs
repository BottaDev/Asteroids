using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
    private PlayerModel _playerModel;
    private SpriteRenderer _renderer;
    private Color _defaultColor;
    
    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _playerModel = gameObject.GetComponent <PlayerModel>();
        _defaultColor = _renderer.color;
    }

    private void Start()
    {
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
    }
    
    private void OnPlayerDamaged(params object[] parameters)
    {
        if (gameObject.activeSelf)
            StartCoroutine(ShowDamageColor());
    }
    
    private IEnumerator ShowDamageColor()
    {
        _renderer.color = Color.red;
        
        yield return new WaitForSeconds(_playerModel.damageColorTime);
        
        _renderer.color = _defaultColor;
    }
}
