using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteRotator : MonoBehaviour
{
    public float RotationSpeed = 10;

    private bool left;

    private void Awake()
    {
        left = Random.value > 0.5f;
    }

    private void Update()
    {
        if (left)
            transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime * -1);
        else
            transform.Rotate(Vector3.forward * RotationSpeed * Time.deltaTime * 1);
    }
}
