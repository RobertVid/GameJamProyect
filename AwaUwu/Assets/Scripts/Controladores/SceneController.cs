using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //Pantalla de carga
    public GameObject pantallaCarga;

    //Tiempo de espera en pantalla de carga
    public float tiempoEspera = 2f;

    //Ya cargo la escena?
    public bool cargado = false;

   

    //Funcion que se encarga de cambiar de escena
    public void CambiarNivel(string escena)
    {
        //Se manda a llamar a la corrutina para pantalla de carga y carga de escena
        StartCoroutine(CargarEscena(tiempoEspera, escena));
    }

    //Corrutina para pantalla de carga
    private IEnumerator CargarEscena(float espera, string escena)
    {
        //Esta cargando
        cargado = true;

        //Se activa la pantalla de carga
        pantallaCarga.SetActive(true);

        //Se espera el tiempo de carga
        yield return new WaitForSeconds(espera);

        //No se destruye el objeto controlador de la escena
        DontDestroyOnLoad(gameObject);

        //Se carga la escena
        AsyncOperation async = SceneManager.LoadSceneAsync(escena);

        //Se desactiva la pantalla de carga
        pantallaCarga.SetActive(false);

        //Mientras no se carge la escena
        if (async.isDone)
        {
            //ya no esta cargando
            cargado = false;

            yield return null;
        }
    }
}
