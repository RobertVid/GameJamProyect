using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Variable que indica el contador de la escena
    public float cronometro = 5;
    public bool finalizado = false;

    private void Start()
    {
        //Se convierten los minutos en segundos
        cronometro *= 60;
    }

    //Funcion para reestablecer el contador
    public void ReestablecerContador(float minutos)
    {
        //Se vuelve a reiniciar el contador
        cronometro = minutos * 60;

        //Se reestablece la variable de finalizado
        finalizado = false;
    }

    //Funcion que se encarga de reducir el contador
    private void Contador()
    {
        //Se comprueba si aun queda tiempo por reducir
        if (cronometro > 0)
        {
            //Se reduce el tiempo
            cronometro -= Time.deltaTime;
        }
        else
        {
            //Ha finalizado el cronometro
            finalizado = true;
        }
    }
}
