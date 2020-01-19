using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialJugador : MonoBehaviour
{
    //El tiempo que tardan en aparecer las imagenes entre ellas
    public float tiempoEntreImagenes = 0.2f;

    //Variables de los sonidos
    public AudioSource teclaPresionada;
    public AudioSource tutorialLogrado;
    public AudioSource tutorialFinalizado;

    //Referencia del jugador
    public GameObject jugador;


    //Teclas de movimiento teclado
    public GameObject teclaDerechaTeclado;
    public GameObject teclaIzquierdaTeclado;
    public GameObject triggerMovimiento;

    //Condiciones para las teclas de movimiento, se han presionado?
    private bool presionado_Izquierda = false;
    private bool presionado_Derecha = false;



    //Teclas de salto teclado
    public GameObject teclaSaltoTeclado;
    public GameObject triggerSalto;

    //Condiciones para las teclas de salto, se han presionado?
    private bool presionado_Salto = false;



    //Teclas de dash teclado
    public GameObject teclaDashTeclado;
    public GameObject teclasMovimientoTeclado;
    public GameObject triggerDash;

    //Condiciones para las teclas de dash, se han presionado?
    private bool presionado_Dash = false;
    private bool presionado_DirDash = false;



    //Tecla de ataque 1 teclado
    public GameObject teclaAtaqueDebilTeclado;
    public GameObject triggerAtaqueDebil;

    //Condiciones para las teclas de ataque 1, se han presionado?
    private bool presionado_AtaqueDebil = false;



    //Tecla de ataque 2 teclado
    public GameObject teclaAtaqueFuerteTeclado;
    public GameObject triggerAtaqueFuerte;

    //Condiciones para las teclas de ataque 2, se han presionado?
    private bool presionado_AtaqueFuerte = false;



    //Inputs de movimiento del control
    public GameObject teclaDerechaControl;
    public GameObject teclaIzquierdaControl;

    //Input de salto del control
    public GameObject teclaSaltoControl;

    //Inputs de dash para el control
    public GameObject teclaDashControl;
    public GameObject teclaDireccionControl;

    //input de ataque debiñ del control
    public GameObject teclaAtaqueDebilControl;

    //input de ataque fuerte del control
    public GameObject teclaAtaqueFuerteControl;

    //Variable para los sonidos
    private bool reproducido = false;

    //Variables privadas
    private Camera cam; //Camara del jugador

    private float tiempoDesdeAparicion = 0; //El momento en que aparecio la imagen
    private float tiempoDesdeFinal = 0; //El momento en el que termina el tutorial

    private Vector3 posicionPantalla = Vector2.zero; //Coordenadas en pantalla del jugador
    private float margen; //Cuanto mide el sprite desde el origen hasta su punto maximo en el eje Y

    private int tutorial = -1; //Indica en que tutorial esta ahora mismo


    private void Start()
    {
        //Se inicializa la camara
        cam = Camera.main;

        //Se obtiene cuanto mide el sprite desde el origen hasta su punto maximo en el eje Y
        margen = jugador.GetComponent<SpriteRenderer>().bounds.extents.y;
    }


    private void Update()
    {
        //Se obtienen las coordenadas del jugador en coordenadas de la camara
        posicionPantalla = cam.WorldToScreenPoint(jugador.transform.position + new Vector3(0, margen, 0));

        Debug.Log(Input.GetJoystickNames());

        //Si no tiene conectado un control entonces usa la configuracion del teclado
        if(Input.GetJoystickNames().Length == 0)
        {
            switch (tutorial)
            {
                case 0:
                    //Se actualizan las teclas de movimiento
                    TeclasMovimientoTeclado();
                    break;

                case 1:
                    //Se actualizan las teclas de salto
                    TeclaSaltoTeclado();
                    break;

                case 2:
                    //Se actualizan las teclas de dash
                    TeclaAtaqueDebilTeclado();
                    break;

                case 3:
                    //Se actualizan las teclas de ataque
                    TeclaAtaqueFuerteTeclado();
                    break;
                case 4:
                    //Se actualizan las teclas de ataque
                    TeclasDashTeclado();
                    break;

                case 5:
                    //Se reproduce el sonido despues de cierto tiempo
                    if (Time.time > tiempoDesdeFinal + 3)
                    {
                        //Se reproduce el sonido para terminar el tutorial
                        tutorialFinalizado.Play();

                        //Se termina el tutorial
                        tutorial++;
                    }
                    break;
            }
        }
        else
        {
            switch (tutorial)
            {
                case 0:
                    //Se actualizan las teclas de movimiento
                    TeclasMovimientoControl();
                    break;

                case 1:
                    //Se activan las teclas de salto
                    TeclaSaltoControl();
                    break;

                case 2:
                    //Se activan las teclas de ataque debil
                    TeclaAtaqueDebilControl();
                    break;

                case 3:
                    //Se activan las teclas de ataque fuerte
                    TeclaAtaqueFuerteControl();
                    break;

                case 4:
                    //Se activan las teclas de dash
                    TeclasDashControl();
                    break;

                case 5:
                    //Se reproduce el sonido despues de cierto tiempo
                    if (Time.time > tiempoDesdeFinal + 3)
                    {
                        //Se reproduce el sonido para terminar el tutorial
                        tutorialFinalizado.Play();

                        //Se termina el tutorial
                        tutorial++;
                    }
                    break;
            }
        }
        


        //Prueba movimiento del jugador
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(20, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-20, 0));
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de movimiento
    private void TeclasMovimientoTeclado()
    {
        //Si no esta activa la tecla A
        if (!teclaIzquierdaTeclado.activeSelf)
        {
            //Se activa la tecla
            teclaIzquierdaTeclado.SetActive(true);

            //Se guarda el tiempo en el que se guardo
            tiempoDesdeAparicion = Time.time;
        }
        else if(!teclaDerechaTeclado.activeSelf && Time.time>tiempoDesdeAparicion+ tiempoEntreImagenes)
        {
            //Se activa la tecla
            teclaDerechaTeclado.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaDerechaTeclado.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(20, 25, 0);
        teclaIzquierdaTeclado.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(-50, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaDerechaTeclado.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaDerechaTeclado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaDerechaTeclado.GetComponent<Animator>().SetBool("presionado", false);
            }
        }

        //Se comprueba si esta activa la imagen
        if (teclaIzquierdaTeclado.activeSelf)
        {
            if (teclaIzquierdaTeclado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaIzquierdaTeclado.GetComponent<Animator>().SetBool("presionado", false);
            }
        }
        

        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (!presionado_Izquierda && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaIzquierdaTeclado.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaIzquierdaTeclado.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_Izquierda = true;
        }

        //Se comprueba si aun no se ha presionado la tecla D y comprueba si la esta presionando
        if (!presionado_Derecha && Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaDerechaTeclado.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaDerechaTeclado.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_Derecha = true;
        }



        //Se comprueba si ya se han presionado las teclas
        if(presionado_Izquierda && presionado_Derecha)
        {
            //Se comprueba si ya han terminado las animaciones
            if(!teclaDerechaTeclado.GetComponent<Animator>().GetBool("presionado") && !teclaIzquierdaTeclado.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_A = teclaDerechaTeclado.GetComponent<Image>().color;
                color_A.a -= 0.1f;
                teclaDerechaTeclado.GetComponent<Image>().color = color_A;

                ///Se degradan las imagenes
                Color color_D = teclaIzquierdaTeclado.GetComponent<Image>().color;
                color_D.a -= 0.1f;
                teclaIzquierdaTeclado.GetComponent<Image>().color = color_D;

                //Se comprueba si ya se degradaron
                if(color_A.a<=0 && color_D.a<=0)
                {
                    

                    //Se desactivan las teclas
                    teclaDerechaTeclado.SetActive(false);
                    teclaIzquierdaTeclado.SetActive(false);
                }
            }
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de salto
    private void TeclaSaltoTeclado()
    {
        //Si no esta activa la tecla A
        if (!teclaSaltoTeclado.activeSelf)
        {
            //Se activa la tecla
            teclaSaltoTeclado.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaSaltoTeclado.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(0, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaSaltoTeclado.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaSaltoTeclado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaSaltoTeclado.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (!presionado_Salto && Input.GetKeyDown(KeyCode.Space))
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaSaltoTeclado.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaSaltoTeclado.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_Salto = true;
        }


        //Se comprueba si ya se han presionado las teclas
        if (presionado_Salto)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaSaltoTeclado.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_Espacio = teclaSaltoTeclado.GetComponent<Image>().color;
                color_Espacio.a -= 0.1f;
                teclaSaltoTeclado.GetComponent<Image>().color = color_Espacio;

                //Se comprueba si ya se degradaron
                if (color_Espacio.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaSaltoTeclado.SetActive(false);
                }
            }
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de dash
    private void TeclasDashTeclado()
    {
        //Si no esta activa la tecla A
        if (!teclaDashTeclado.activeSelf)
        {
            //Se activa la tecla
            teclaDashTeclado.SetActive(true);

            //Se guarda el tiempo en el que se guardo
            tiempoDesdeAparicion = Time.time;
        }
        else if (!teclasMovimientoTeclado.activeSelf && Time.time > tiempoDesdeAparicion + tiempoEntreImagenes)
        {
            //Se activa la tecla
            teclasMovimientoTeclado.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaDashTeclado.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(-50, 25, 0);
        teclasMovimientoTeclado.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(20, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaDashTeclado.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaDashTeclado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaDashTeclado.GetComponent<Animator>().SetBool("presionado", false);
            }
        }

        //Se comprueba si esta activa la imagen
        if (teclasMovimientoTeclado.activeSelf)
        {
            if (teclasMovimientoTeclado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclasMovimientoTeclado.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (Input.GetKey(KeyCode.A) && !presionado_DirDash)
        {
            //Si es el primer frame de la tecla
            if (Input.GetKeyDown(KeyCode.A))
            {
                //Sonido de tecla
                teclaPresionada.Play();
            }
            
            //Se cambia el color de la tecla
            teclaDashTeclado.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaDashTeclado.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_Dash = true;

            //Se comprueba si aun no se ha presionado la tecla D y comprueba si la esta presionando
            if (!presionado_DirDash && Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.UpArrow))
            {
                //Sonido de tecla
                teclaPresionada.Play();

                //Se cambia el color de la tecla
                teclasMovimientoTeclado.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

                //Inicia la animacion
                teclasMovimientoTeclado.GetComponent<Animator>().SetBool("presionado", true);

                //Ahora se ha presionado la tecla
                presionado_DirDash = true;
            }
        }
        else if(!presionado_DirDash)
        {
            //Se cambia el color de la tecla al original (blanco)
            teclaDashTeclado.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            //Reinicia la animacion
            teclaDashTeclado.GetComponent<Animator>().SetBool("presionado", false);

            //Ya no se presiona la tecla
            presionado_Dash = false;
        }



        //Se comprueba si ya se han presionado las teclas
        if (presionado_Dash && presionado_DirDash)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaDashTeclado.GetComponent<Animator>().GetBool("presionado") && !teclasMovimientoTeclado.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_Dash = teclaDashTeclado.GetComponent<Image>().color;
                color_Dash.a -= 0.1f;
                teclaDashTeclado.GetComponent<Image>().color = color_Dash;

                ///Se degradan las imagenes
                Color color_Movimiento = teclasMovimientoTeclado.GetComponent<Image>().color;
                color_Movimiento.a -= 0.1f;
                teclasMovimientoTeclado.GetComponent<Image>().color = color_Movimiento;

                //Se comprueba si ya se degradaron
                if (color_Dash.a <= 0 && color_Movimiento.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaDashTeclado.SetActive(false);
                    teclasMovimientoTeclado.SetActive(false);

                    //Se cambia al estado final para reproducir sonido
                    tutorial++;

                    //Se actualiza el tiempo del final
                    tiempoDesdeFinal = Time.time;
                }
            }
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de ataque 1
    private void TeclaAtaqueDebilTeclado()
    {
        //Si no esta activa la tecla A
        if (!teclaAtaqueDebilTeclado.activeSelf)
        {
            //Se activa la tecla
            teclaAtaqueDebilTeclado.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaAtaqueDebilTeclado.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(0, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaAtaqueDebilTeclado.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaAtaqueDebilTeclado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaAtaqueDebilTeclado.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (!presionado_AtaqueDebil && Input.GetKeyDown(KeyCode.S))
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaAtaqueDebilTeclado.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaAtaqueDebilTeclado.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_AtaqueDebil = true;
        }


        //Se comprueba si ya se han presionado las teclas
        if (presionado_AtaqueDebil)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaAtaqueDebilTeclado.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_S = teclaAtaqueDebilTeclado.GetComponent<Image>().color;
                color_S.a -= 0.1f;
                teclaAtaqueDebilTeclado.GetComponent<Image>().color = color_S;

                //Se comprueba si ya se degradaron
                if (color_S.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaAtaqueDebilTeclado.SetActive(false);
                }
            }
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de ataque 1
    private void TeclaAtaqueFuerteTeclado()
    {
        //Si no esta activa la tecla A
        if (!teclaAtaqueFuerteTeclado.activeSelf)
        {
            //Se activa la tecla
            teclaAtaqueFuerteTeclado.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaAtaqueFuerteTeclado.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(0, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaAtaqueFuerteTeclado.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaAtaqueFuerteTeclado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaAtaqueFuerteTeclado.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (!presionado_AtaqueFuerte && Input.GetKeyDown(KeyCode.D))
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaAtaqueFuerteTeclado.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaAtaqueFuerteTeclado.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_AtaqueFuerte = true;
        }


        //Se comprueba si ya se han presionado las teclas
        if (presionado_AtaqueFuerte)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaAtaqueFuerteTeclado.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_S = teclaAtaqueFuerteTeclado.GetComponent<Image>().color;
                color_S.a -= 0.1f;
                teclaAtaqueFuerteTeclado.GetComponent<Image>().color = color_S;

                //Se comprueba si ya se degradaron
                if (color_S.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaAtaqueFuerteTeclado.SetActive(false);
                }
            }
        }
    }



    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de movimiento
    private void TeclasMovimientoControl()
    {
        //Si no esta activa la tecla A
        if (!teclaIzquierdaControl.activeSelf)
        {
            //Se activa la tecla
            teclaIzquierdaControl.SetActive(true);

            //Se guarda el tiempo en el que se guardo
            tiempoDesdeAparicion = Time.time;
        }
        else if (!teclaDerechaControl.activeSelf && Time.time > tiempoDesdeAparicion + tiempoEntreImagenes)
        {
            //Se activa la tecla
            teclaDerechaControl.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaDerechaControl.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(20, 25, 0);
        teclaIzquierdaControl.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(-50, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaDerechaControl.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaDerechaControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaDerechaControl.GetComponent<Animator>().SetBool("presionado", false);
            }
        }

        //Se comprueba si esta activa la imagen
        if (teclaIzquierdaControl.activeSelf)
        {
            if (teclaIzquierdaControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaIzquierdaControl.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Si se mueve la palanca hacia la izquierda
        if (!presionado_Izquierda && Input.GetAxis("Horizontal")<-0.7f)
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaIzquierdaControl.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaIzquierdaControl.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_Izquierda = true;
        }

        //Si se mueve la palanca a la derecha
        if (!presionado_Derecha && Input.GetAxis("Horizontal") > 0.7f)
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaDerechaControl.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaDerechaControl.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_Derecha = true;
        }



        //Se comprueba si ya se han presionado las teclas
        if (presionado_Izquierda && presionado_Derecha)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaDerechaControl.GetComponent<Animator>().GetBool("presionado") && !teclaIzquierdaControl.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_Derecha = teclaDerechaControl.GetComponent<Image>().color;
                color_Derecha.a -= 0.1f;
                teclaDerechaControl.GetComponent<Image>().color = color_Derecha;

                ///Se degradan las imagenes
                Color color_Izquierda = teclaIzquierdaControl.GetComponent<Image>().color;
                color_Izquierda.a -= 0.1f;
                teclaIzquierdaControl.GetComponent<Image>().color = color_Izquierda;

                //Se comprueba si ya se degradaron
                if (color_Derecha.a <= 0 && color_Izquierda.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaDerechaControl.SetActive(false);
                    teclaIzquierdaControl.SetActive(false);
                }
            }
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de salto
    private void TeclaSaltoControl()
    {
        //Si no esta activa la tecla A
        if (!teclaSaltoControl.activeSelf)
        {
            //Se activa la tecla
            teclaSaltoControl.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaSaltoControl.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(0, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaSaltoControl.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaSaltoControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaSaltoControl.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (!presionado_Salto && Input.GetKey("joystick button 0"))
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaSaltoControl.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaSaltoControl.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_Salto = true;
        }


        //Se comprueba si ya se han presionado las teclas
        if (presionado_Salto)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaSaltoControl.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_Espacio = teclaSaltoControl.GetComponent<Image>().color;
                color_Espacio.a -= 0.1f;
                teclaSaltoControl.GetComponent<Image>().color = color_Espacio;

                //Se comprueba si ya se degradaron
                if (color_Espacio.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaSaltoControl.SetActive(false);
                }
            }
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de dash
    private void TeclasDashControl()
    {
        //Si no esta activa la tecla A
        if (!teclaDashControl.activeSelf)
        {
            //Se activa la tecla
            teclaDashControl.SetActive(true);

            //Se guarda el tiempo en el que se guardo
            tiempoDesdeAparicion = Time.time;
        }
        else if (!teclaDireccionControl.activeSelf && Time.time > tiempoDesdeAparicion + tiempoEntreImagenes)
        {
            //Se activa la tecla
            teclaDireccionControl.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaDashControl.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(-50, 25, 0);
        teclaDireccionControl.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(20, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaDashControl.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaDashControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaDashControl.GetComponent<Animator>().SetBool("presionado", false);
            }
        }

        //Se comprueba si esta activa la imagen
        if (teclaDireccionControl.activeSelf)
        {
            if (teclaDireccionControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaDireccionControl.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (Input.GetKey("joystick button 4") && !presionado_DirDash)
        {
            //Si es el primer frame de la tecla
            if (Input.GetKeyDown(KeyCode.A))
            {
                //Sonido de tecla
                teclaPresionada.Play();
            }

            //Se cambia el color de la tecla
            teclaDashControl.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaDashControl.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_Dash = true;

            //Se comprueba si aun no se ha presionado la tecla D y comprueba si la esta presionando
            if (!presionado_DirDash && Input.GetAxis("Horizontal")>0.2 && Input.GetAxis("Vertical")>0.2)
            {
                //Sonido de tecla
                teclaPresionada.Play();

                //Se cambia el color de la tecla
                teclaDireccionControl.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

                //Inicia la animacion
                teclaDireccionControl.GetComponent<Animator>().SetBool("presionado", true);

                //Ahora se ha presionado la tecla
                presionado_DirDash = true;
            }
        }
        else if (!presionado_DirDash)
        {
            //Se cambia el color de la tecla al original (blanco)
            teclaDashControl.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

            //Reinicia la animacion
            teclaDashControl.GetComponent<Animator>().SetBool("presionado", false);

            //Ya no se presiona la tecla
            presionado_Dash = false;
        }



        //Se comprueba si ya se han presionado las teclas
        if (presionado_Dash && presionado_DirDash)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaDashControl.GetComponent<Animator>().GetBool("presionado") && !teclaDireccionControl.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_Dash = teclaDashControl.GetComponent<Image>().color;
                color_Dash.a -= 0.1f;
                teclaDashControl.GetComponent<Image>().color = color_Dash;

                ///Se degradan las imagenes
                Color color_Movimiento = teclaDireccionControl.GetComponent<Image>().color;
                color_Movimiento.a -= 0.1f;
                teclaDireccionControl.GetComponent<Image>().color = color_Movimiento;

                //Se comprueba si ya se degradaron
                if (color_Dash.a <= 0 && color_Movimiento.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaDashControl.SetActive(false);
                    teclaDireccionControl.SetActive(false);

                    //Se cambia al estado final para reproducir sonido
                    tutorial++;

                    //Se actualiza el tiempo del final
                    tiempoDesdeFinal = Time.time;
                }
            }
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de ataque 1
    private void TeclaAtaqueDebilControl()
    {
        //Si no esta activa la tecla A
        if (!teclaAtaqueDebilControl.activeSelf)
        {
            //Se activa la tecla
            teclaAtaqueDebilControl.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaAtaqueDebilControl.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(0, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaAtaqueDebilControl.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaAtaqueDebilControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaAtaqueDebilControl.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (!presionado_AtaqueDebil && Input.GetKey("joystick button 1"))
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaAtaqueDebilControl.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaAtaqueDebilControl.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_AtaqueDebil = true;
        }


        //Se comprueba si ya se han presionado las teclas
        if (presionado_AtaqueDebil)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaAtaqueDebilControl.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_S = teclaAtaqueDebilControl.GetComponent<Image>().color;
                color_S.a -= 0.1f;
                teclaAtaqueDebilControl.GetComponent<Image>().color = color_S;

                //Se comprueba si ya se degradaron
                if (color_S.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaAtaqueDebilControl.SetActive(false);
                }
            }
        }
    }

    //Funcion que se encarga de actualizar la posicion de los elementos de la interfaz
    //que muestran las teclas de ataque 1
    private void TeclaAtaqueFuerteControl()
    {
        //Si no esta activa la tecla A
        if (!teclaAtaqueFuerteControl.activeSelf)
        {
            //Se activa la tecla
            teclaAtaqueFuerteControl.SetActive(true);
        }

        //Se actualiza la posicion de los inputs a la indicada por 'posicionPantalla' en su componente Rect Transform
        teclaAtaqueFuerteControl.GetComponent<RectTransform>().position = posicionPantalla + new Vector3(0, 25, 0);

        //Se comprueba si esta activa la imagen
        if (teclaAtaqueFuerteControl.activeSelf)
        {
            //Se comprueba si esta en el estado idle para que se anule la animacion
            if (teclaAtaqueFuerteControl.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                //Se desactiva la animacion de presionado
                teclaAtaqueFuerteControl.GetComponent<Animator>().SetBool("presionado", false);
            }
        }


        //Se comprueba si aun no se ha presionado la tecla A y comprueba si la esta presionando
        if (!presionado_AtaqueFuerte && Input.GetKeyDown("joystick button 2"))
        {
            //Sonido de tecla
            teclaPresionada.Play();

            //Se cambia el color de la tecla
            teclaAtaqueFuerteControl.GetComponent<Image>().color = new Color32(161, 212, 41, 255);

            //Inicia la animacion
            teclaAtaqueFuerteControl.GetComponent<Animator>().SetBool("presionado", true);

            //Ahora se ha presionado la tecla
            presionado_AtaqueFuerte = true;
        }


        //Se comprueba si ya se han presionado las teclas
        if (presionado_AtaqueFuerte)
        {
            //Se comprueba si ya han terminado las animaciones
            if (!teclaAtaqueFuerteControl.GetComponent<Animator>().GetBool("presionado"))
            {
                //Si aun no se ha reproducido el sonido de exito
                if (!reproducido)
                {
                    //Se reproduce el sonido
                    tutorialLogrado.Play();

                    //Se cambia la variable
                    reproducido = true;
                }

                //Se degradan las imagenes
                Color color_S = teclaAtaqueFuerteControl.GetComponent<Image>().color;
                color_S.a -= 0.1f;
                teclaAtaqueFuerteControl.GetComponent<Image>().color = color_S;

                //Se comprueba si ya se degradaron
                if (color_S.a <= 0)
                {
                    //Se desactivan las teclas
                    teclaAtaqueFuerteControl.SetActive(false);
                }
            }
        }
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        //Se comprueba si entra en el trigger de movimiento
        if (collision.name == triggerMovimiento.name && collision.GetComponent<BoxCollider2D>().enabled)
        {
            //Se cambia al tutorial de movimiento
            tutorial = 0;

            //Se desactiva el trigger
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }

        //Se comprueba si entra en el trigger de salto
        if (collision.name == triggerSalto.name && collision.GetComponent<BoxCollider2D>().enabled && tutorial==0)
        {
            //Se comprueba si ya estan desactivadas las teclas del tutorial anterior
            if(!teclaDerechaTeclado.activeSelf && !teclaIzquierdaTeclado.activeSelf)
            {
                //Se cambia al tutorial de movimiento
                tutorial = 1;

                //Se desactiva el trigger
                collision.GetComponent<BoxCollider2D>().enabled = false;

                //Se reinicia la variable
                reproducido = false;
            }
        }

        //Se comprueba si entra en el trigger de ataque 1
        if (collision.name == triggerAtaqueDebil.name && collision.GetComponent<BoxCollider2D>().enabled && tutorial==1)
        {
            //Se comprueba si ya estan desactivadas las teclas del tutorial anterior
            if (!teclaDashTeclado.activeSelf)
            {
                //Se cambia al tutorial de movimiento
                tutorial = 2;

                //Se desactiva el trigger
                collision.GetComponent<BoxCollider2D>().enabled = false;

                //Se reinicia la variable
                reproducido = false;
            }
        }

        //Se comprueba si entra en el trigger de ataque 2
        if (collision.name == triggerAtaqueFuerte.name && collision.GetComponent<BoxCollider2D>().enabled && tutorial == 2)
        {
            //Se comprueba si ya estan desactivadas las teclas del tutorial anterior
            if (!teclaAtaqueDebilTeclado.activeSelf)
            {
                //Se cambia al tutorial de movimiento
                tutorial = 3;

                //Se desactiva el trigger
                collision.GetComponent<BoxCollider2D>().enabled = false;

                //Se reinicia la variable
                reproducido = false;
            }
        }

        //Se comprueba si entra en el trigger de dash
        if (collision.name == triggerDash.name && collision.GetComponent<BoxCollider2D>().enabled && tutorial == 3)
        {
            //Se comprueba si ya estan desactivadas las teclas del tutorial anterior
            if (!teclaSaltoTeclado.activeSelf)
            {
                //Se cambia al tutorial de movimiento
                tutorial = 4;

                //Se desactiva el trigger
                collision.GetComponent<BoxCollider2D>().enabled = false;

                //Se reinicia la variable
                reproducido = false;
            }
        }
    }
}