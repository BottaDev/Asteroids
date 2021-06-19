using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerView : MonoBehaviour
{
    public float colorTime = 0.3f;
    
    private SpriteRenderer _renderer;
    private Color _defaultColor;
    
    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _defaultColor = _renderer.color;
    }

    private void Start()
    {
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
    }
    
    private void OnPlayerDamaged(params object[] parameters)
    {
        StartCoroutine(ShowDamageColor());
    }
    
    private IEnumerator ShowDamageColor()
    {
        _renderer.color = Color.red;
        
        yield return new WaitForSeconds(colorTime);
        
        _renderer.color = _defaultColor;
    }
}
