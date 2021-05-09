using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Hp;
    public float Speed;
    
    [Range(min: 0, max: 1)]
    public float FireRate;
    
}
