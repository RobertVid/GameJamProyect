using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    //Guarda el punto de guardado actual
    public GameObject puntoActual;

    //particulas a utilizar
    public ParticleSystem particulasReaparicion;
    public ParticleSystem particulasGuardado;

    //Sonidos a utilizar
    public AudioSource sonidoReaparicion;
    public AudioSource sonidoGuardado;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Reaparecer();
        }
    }

    //Funcion que se encarga de transportar al jugador al checkpoint
    public void Reaparecer()
    {
        //Se comprueba que haya un checkpoint guardado
        if (puntoActual)
        {
            //Sonido de reaparicion
            sonidoReaparicion.Play();

            //Se teletransporta el jugador a su ultimo checkpoints
            transform.position = puntoActual.transform.position;

            //Se crean las particulas de reaparicion
            Instantiate(particulasReaparicion, puntoActual.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se comprueba que el objeto trigger con el que colisiono es un checkpoint
        if(collision.tag == "Respawn")
        {
            //Sonido de guardado
            sonidoGuardado.Play();

            //Se guarda el checkpoint
            puntoActual = collision.gameObject;

            //Se desactiva el checkpoint
            collision.GetComponent<CircleCollider2D>().enabled = false;

            //Se crean las particulas de guardado
            Instantiate(particulasGuardado, transform);
        }
    }
}
