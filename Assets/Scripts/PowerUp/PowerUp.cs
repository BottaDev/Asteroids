using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PowerUp : MonoBehaviour, IPowerUp
{
    public IPowerUp NextPowerUp { get; set; }

    public void Use(PlayerModel playerModel)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
            Use(other.GetComponent<PlayerModel>());
    }
}
