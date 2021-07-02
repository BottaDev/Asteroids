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
    private GameObject _fireSprite;


    private void Awake()
    {
        _renderer = gameObject.GetComponent<SpriteRenderer>();
        _defaultColor = _renderer.color;
    }

    private void Start()
    {
        EventManager.Instance.Subscribe("OnPlayerDamaged", OnPlayerDamaged);
        EventManager.Instance.Subscribe("OnPlayerMove", OnPlayerMove);
    }
    
    private void OnPlayerDamaged(params object[] parameters)
    {
        if(gameObject.activeSelf)
            StartCoroutine(ShowDamageColor());
    }

    private void OnPlayerMove(params object[] parameters)
    {
        _fireSprite = (GameObject)parameters[0];

        StartCoroutine(MovementFire());
    }
    
    private IEnumerator ShowDamageColor()
    {
        _renderer.color = Color.red;
        
        yield return new WaitForSeconds(colorTime);
        
        _renderer.color = _defaultColor;
    }

    private IEnumerator MovementFire()
    {
        _fireSprite.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        _fireSprite.SetActive(false);
    }
}
