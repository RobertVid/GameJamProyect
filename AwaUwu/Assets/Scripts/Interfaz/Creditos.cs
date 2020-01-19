using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creditos : MonoBehaviour
{
    //Arreglo de elementos de interfaz que se mostraran en los creditos
    public GameObject panelFondo;
    public GameObject[] creditos;
    public int velocidadFade = 2;
    public float tiempoEspera = 3;

    //Variable que indica si se estan mostrando creditos
    public bool mostrandoCreditos = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            MostrarCreditos();
        }
    }

    //Funcion que se encarga de comenzar los creditos
    public void MostrarCreditos()
    {
        //Se comprueba si no se ha iniciado la coroutina
        if (!mostrandoCreditos)
        {
            //Se inicia la coroutina
            StartCoroutine(MostrarCreditos(panelFondo, creditos, velocidadFade, tiempoEspera));
        }
    }

    private IEnumerator MostrarCreditos(GameObject panelFondo, GameObject[] creditos, int velocidadFade, float segundosEspera)
    {
        //Se activa la variable que indica que se estan mostrando los creditos
        mostrandoCreditos = true;

        //Se activa el panel fondo
        panelFondo.SetActive(true);

        //Se hace fade in del panel de fondo
        for (int i = 0; i <= 255; i += velocidadFade)
        {
            //Se aumenta la opacidad
            Color color_panel = panelFondo.GetComponent<Image>().color;
            color_panel.a = i / 255f;
            panelFondo.GetComponent<Image>().color = color_panel;

            yield return null;
        }

        //Se recorren todos los creditos
        for (int j = 0; j < creditos.Length; j++)
        {
            //Se activan los creditos actuales
            creditos[j].SetActive(true);

            //Se hace fade in
            for (int i = 0; i <= 255; i += velocidadFade)
            {
                //Se aumenta la opacidad
                creditos[j].GetComponent<CanvasGroup>().alpha = i / 255f;

                yield return null;
            }

            //Se espera el tiempo indicado
            yield return new WaitForSeconds(segundosEspera);

            //Se hace fade out
            for (int i = 255; i >= 0; i -= velocidadFade)
            {
                //Se disminuye la opacidad
                creditos[j].GetComponent<CanvasGroup>().alpha = i / 255f;

                yield return null;
            }

            //Se desactivan los creditos actuales
            creditos[j].SetActive(false);
        }

        //Se desactiva el panel de fondo
        for (int i = 255; i >= 0; i -= velocidadFade)
        {
            //Se disminuye la opacidad
            Color color_panel = panelFondo.GetComponent<Image>().color;
            color_panel.a = i / 255f;
            panelFondo.GetComponent<Image>().color = color_panel;

            yield return null;
        }

        //Se desactiva el panel fondo
        panelFondo.SetActive(false);

        //Deja de mostrar los creditos
        mostrandoCreditos = false;

        //Detiene la corrutina
        StopCoroutine("Fade");
    }
}
