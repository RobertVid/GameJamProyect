using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //VariablesPublicas
    public Transform groundCheck;
    public LayerMask isGround;
    public Transform posicionAtaque;
    public LayerMask esEnemigo;
    public Animator anim;
    public CheckpointManager checkpointManager;
    public src_Overlay src_over;

    public float velocidad;
    public float fuerzaSalto;
    public float radio;
    public float velocidadDash;
    public float inicioTiempoAtaque;
    public float inicioTiempoAtaqueFuerte;
    public float rangoAtaque;
    public float inicioTiempoDash;
    public float inicioCooldownCombo;
    public float velocidadSuperDash;
    public float daño;
    public float limiteEscalaInferior;
    public float limiteEscalaSuperior;

    public int comboIndex;
    public int[] combo;

    public bool perdiste = false;

    //VariablesPrivadas
    private Rigidbody2D rb2d;

    private bool derecha = true;
	private bool grounded;
    private bool dobleSalto;
    private bool superDash = false;
    private bool canDash;
    private bool inCombo = false;
    private bool canCombo = true;
    private bool canAttackDebil = true;
    private bool canAttackFuerte = true;
    private bool canMove = true;

    private float tiempoAtaque;
    private float tiempoCombo;
    private float tiempoAtaqueFuerte;
    private float tiempoDash;
    private float recargaSuperDash;
    private float direccion;
    private float cooldownCombo;
    private float CoeficienteFuerzaVelocidad = 1.0f;
    private float EscalaOriginal;

    private int dir;

    // START
    void Start ()
    {
        // Poner variables en su estado inicial
        EscalaOriginal = transform.localScale.y;
        combo = new int[3];
        vaciarArreglo();
        rb2d = GetComponent<Rigidbody2D>();
        tiempoDash = inicioTiempoDash;
        src_over = GetComponent<src_Overlay>();
	}

    // Update is called once per frame
    private void Update()
    {
        //Salto y DobleSalto
        IsGrounded(); // Checa si se esta tocando suelo
        recargarCombo(); // Cooldown para hacer otro combo
        recargarAtaqueDebil(); // Cooldown ataque debil
        recargarAtaqueFuerte(); // Cooldown ataque fuerte
        mantenerCombo(); // Tiempo en el que se puede seguir con la cadena de combo
        estadosAnimacion(); // Calculando variables que sirven de parametros para las animaciones
        CalcularValorCoeficienteFV(); // Este coeficiente hace la relacion entre el tamaño, la fuerza y la velocidad

        // Espacio para saltar
        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            Saltar();
        }

        // Tecla L para recibir daño (Temporal para hacer pruebas)
        if (Input.GetKeyDown(KeyCode.L))
        {
            takeDamage();
        }

        //Ataque debil con la Tecla D
        if (canAttackDebil && canMove)
        {
            if (Input.GetKeyDown(KeyCode.D) && canCombo)
            {
                ataqueDebil();
            }
        }

        //Ataque Fuerte con la tecla S
        if (canAttackFuerte && canMove)
        {
            if (Input.GetKeyDown(KeyCode.S) && canCombo)
            {
                ataqueFuerte();
            }
        }
    }

    //Usar preferentemete para físicas
    void FixedUpdate()
    {
        MoverJugador();

        //Dash
        if (dir == 1 && Input.GetKey(KeyCode.D) && dobleSalto && !grounded && canMove)
        {
            rb2d.AddForce(Vector3.right * velocidadDash);
            rb2d.velocity = Vector2.up * 5;
            dobleSalto = false;
        }
        else if (dir == 2 && Input.GetKey(KeyCode.D) && dobleSalto && !grounded && canMove)
        {
            rb2d.AddForce(Vector3.left * velocidadDash);
            rb2d.velocity = Vector2.up * 5;
            dobleSalto = false;
        }

        //SuperDash
        if (tiempoDash < 0f)
        {
            tiempoDash = inicioTiempoDash;
            recargaSuperDash = 0f;
            rb2d.velocity = Vector2.zero;
            superDash = false;
            canDash = false;
        }
        if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow) && canDash && canMove)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rb2d.velocity = new Vector2(1f, 1f) * velocidadSuperDash;
            superDash = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) && canDash && canMove)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rb2d.velocity = new Vector2(-1f, 1f) * velocidadSuperDash;
            superDash = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow) && canDash && canMove)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rb2d.velocity = new Vector2(-1f, -1f) * velocidadSuperDash;
            superDash = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow) && canDash && canMove)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rb2d.velocity = new Vector2(1f, -1f) * velocidadSuperDash;
            superDash = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && canDash && canMove)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rb2d.velocity = Vector2.right * velocidadSuperDash;
            superDash = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && canDash && canMove)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rb2d.velocity = Vector2.left * velocidadSuperDash;
            superDash = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && canDash && canMove)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rb2d.velocity = Vector2.up * velocidadSuperDash;
            superDash = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && canDash && canMove)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rb2d.velocity = Vector2.down * velocidadSuperDash;
            superDash = true;
        }

        // Recarga de super dash
        if (canDash == false)
        {
            recargaSuperDash += Time.deltaTime;

            if(recargaSuperDash >= 0.5f)
            {
                Physics2D.IgnoreLayerCollision(9, 10, false);
            }
            if (recargaSuperDash >= 1.5f)
            {
                canDash = true;
            }
        }

        if (superDash)
        {
            tiempoDash -= Time.deltaTime;
        }

        // Voltear jugador
        if (derecha == false && direccion > 0f && canMove)
        {
            Voltear();
        }
        if (derecha == true && direccion < 0f && canMove)
        {
            Voltear();
        }
    }

    void MoverJugador()
    {
        //MovimientoJugador
        if (Input.GetKey(KeyCode.RightArrow) && canMove)
        {
            direccion = 1f;
            dir = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && canMove)
        {
            direccion = -1f;
            dir = 2;
        }
        else
        {
            direccion = 0f;
        }

        Vector3 moveDir = new Vector3(direccion, 0);
        transform.position += ((moveDir * velocidad * Time.deltaTime)/ CoeficienteFuerzaVelocidad);
    }

	void Voltear()
    {
        derecha = !derecha;
        transform.Rotate(0f, 180f, 0f);
	}

    void Saltar()
    {
        // Primer salto
        if (grounded)
        {
            rb2d.velocity = new Vector2(0f, 1f) * fuerzaSalto;
        }

        // Doble salto
        if (!grounded && dobleSalto && CoeficienteFuerzaVelocidad < 1.2f)
        {
            rb2d.velocity = Vector2.up * fuerzaSalto;
            dobleSalto = false;
        }
    }

    void IsGrounded()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, radio, isGround);
        if (grounded == true)
        {
            dobleSalto = true;
        }
    }

    void HacerDaño(float _daño, int _tipo)
    {
        if(_tipo == 1)
        {
            anim.SetBool("Attack", true);
        }
        if(_tipo == 2)
        {
            anim.SetBool("StrongAttack", true);
        }
        Collider2D[] dañoEnemigos = Physics2D.OverlapCircleAll(posicionAtaque.position, rangoAtaque, esEnemigo);
        for (int i = 0; i < dañoEnemigos.Length; i++)
        {
            dañoEnemigos[i].GetComponent<BasicEnemyController>().takeDamage(_daño*CoeficienteFuerzaVelocidad);
            if(transform.localScale.x > limiteEscalaInferior)
            {
                transform.localScale = new Vector3(transform.localScale.x - 0.2f, transform.localScale.y - 0.2f, transform.localScale.z);
                src_over.adjustOverlayColor(-0.07f);
            }
        }
    }

    public void takeDamage()
    {
        if (transform.localScale.x < limiteEscalaSuperior)
        {
            CoeficienteFuerzaVelocidad += 0.1f;
            transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f, transform.localScale.z);
            src_over.adjustOverlayColor(0.07f);
        }
        if(transform.localScale.x >= limiteEscalaSuperior)
        {
            explotar();
        }
    }

    void ataqueDebil()
    {
        switch (inCombo)
        {
            case false:
                // Ataque debil normal
                inCombo = true;
                tiempoCombo = 2.0f;
                tiempoAtaque = inicioTiempoAtaque;

                // Llamar a la funcion que hace daño
                combo[comboIndex] = 1;
                HacerDaño(daño, 1);
                break;
            case true:
                if (comboIndex < 2)
                {
                    comboIndex = comboIndex + 1;
                }
                combo[comboIndex] = 1;
                tiempoCombo = 2.0f;
                tiempoAtaque = inicioTiempoAtaque;
                ataqueCombo();
                break;
        }
    }

    void ataqueFuerte()
    {
        switch (inCombo)
        {
            case false:
                // Ataque Fuerte
                inCombo = true;
                tiempoCombo = 2.0f;
                tiempoAtaqueFuerte = inicioTiempoAtaqueFuerte;

                // Llamar a la funcion que hace daño
                combo[comboIndex] = 2;
                HacerDaño(daño * 2, 2);
                break;
            case true:
                if (comboIndex < 2)
                {
                    comboIndex = comboIndex + 1;
                }
                combo[comboIndex] = 2;
                tiempoCombo = 2.0f;
                tiempoAtaqueFuerte = inicioTiempoAtaqueFuerte;
                ataqueCombo();
                break;
        }
    }

    // Lista de combos
    void ataqueCombo()
    {
        // Debil, debil: daño total 11
        if (combo[0] == 1 && combo[1] == 1 && combo[2] == 0)
        {
            HacerDaño(daño * 1.2f, 1);
        }

        // Debil, debil, debil: daño total 20
        if (combo[0] == 1 && combo[1] == 1 && combo[2] == 1)
        {
            HacerDaño(daño * 1.8f, 1);
            iniciarCooldownCombo();
            vaciarArreglo();
        }

        // Debil, fuerte: daño total 30
        if (combo[0] == 1 && combo[1] == 2 && combo[2] == 0)
        {
            HacerDaño(daño * 5f, 2);
            iniciarCooldownCombo();
            vaciarArreglo();
        }

        // Debil, debil, fuerte: daño total 35
        if (combo[0] == 1 && combo[1] == 1 && combo[2] == 2)
        {
            HacerDaño((daño * 5f)-1, 2);
            iniciarCooldownCombo();
            vaciarArreglo();
        }

        // Fuerte, fuerte: daño total 40
        if (combo[0] == 2 && combo[1] == 2 && combo[2] == 0)
        {
            HacerDaño(daño * 6f, 2);
            iniciarCooldownCombo();
            vaciarArreglo();
        }

        // Fuerte, debil: daño total 17
        if (combo[0] == 2 && combo[1] == 1 && combo[2] == 0)
        {
            HacerDaño(daño * 1.4f, 1);
        }

        // Fuerte, debil, debil: daño total 25
        if (combo[0] == 2 && combo[1] == 1 && combo[2] == 1)
        {
            HacerDaño(daño * 1.6f, 1);
            iniciarCooldownCombo();
            vaciarArreglo();
        }
    }

    void vaciarArreglo()
    {
        // Vaciando el arreglo de combos
        for (int i = 0; i < combo.Length; i++)
        {
            combo[i] = 0;
        }
        // Reiniciando el index del combo
        comboIndex = 0;
        inCombo = false;
    }

    void iniciarCooldownCombo()
    {
        cooldownCombo = inicioCooldownCombo;
        canCombo = false;
    }

    void recargarCombo()
    {
        if(cooldownCombo > 0)
        {
            cooldownCombo -= Time.deltaTime;
        }
        if(cooldownCombo <= 0 && !canCombo)
        {
            canCombo = true;
        }
    }

    void recargarAtaqueDebil()
    {
        if (tiempoAtaque > 0)
        {
            tiempoAtaque -= Time.deltaTime;
            canAttackDebil = false;
        }
        if (tiempoAtaque <= 0)
        {
            canAttackDebil = true;
            anim.SetBool("Attack", false);
        }
    }

    void recargarAtaqueFuerte()
    {
        if (tiempoAtaqueFuerte > 0)
        {
            tiempoAtaqueFuerte -= Time.deltaTime;
            canAttackFuerte = false;
        }
        if (tiempoAtaqueFuerte <= 0)
        {
            canAttackFuerte = true;
            anim.SetBool("StrongAttack", false);
        }
    }

    void mantenerCombo()
    {
        // Comenzar el tiempo en que esta el combo
        if (tiempoCombo > 0f)
        {
            tiempoCombo -= Time.deltaTime;
        }
        // Quitar el incombo
        if (tiempoCombo <= 0f && inCombo == true)
        {
            vaciarArreglo();
            comboIndex = 0;
        }
    }

    void explotar()
    {
        canMove = false;
        transform.localScale = new Vector3(EscalaOriginal, EscalaOriginal, 1.0f);
        anim.SetBool("Explode", true);
        src_over.adjustOverlayColor(-1.12f);
    }

    void reestablecer()
    {
        anim.SetBool("Explode", false);
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(tiempoEspera(1.0f));
    }

    void estadosAnimacion()
    {
        if(grounded == true)
        {
            anim.SetBool("Grounded", true);
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", false);
        }
        if (grounded == false)
        {
            anim.SetBool("Grounded", false);
        }
        if (direccion == 0)
        {
            anim.SetBool("isRunning", false);
        }
        if (direccion != 0)
        {
            anim.SetBool("isRunning", true);
        }
        if(rb2d.velocity.y > 0 && !grounded)
        {
            anim.SetBool("Jump", true);
        }
        if (rb2d.velocity.y < 0)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", true);
        }
    }

    void CalcularValorCoeficienteFV()
    {
        CoeficienteFuerzaVelocidad = (transform.localScale.y / EscalaOriginal);
    }

    IEnumerator tiempoEspera(float _tiempo)
    {
        yield return new WaitForSeconds(_tiempo);
        GetComponent<SpriteRenderer>().enabled = true;
        checkpointManager.Reaparecer();
        canMove = true;
    }
}
