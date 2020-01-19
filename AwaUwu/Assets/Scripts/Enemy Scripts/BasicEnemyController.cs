using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    // Variables publicas
    public float vidaEnemigo = 100f;

    // Variables Privadas

    public void takeDamage(float _daño)
    {
        vidaEnemigo -= _daño;
    }
}
