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
    //public src_Overlay src_over;

    public float radio = 0.3f;
    public float inicioTiempoAtaque;
    public float rangoAtaque = 3.0f;
    public float inicioCooldownCombo;
    public float velocidadSuperDash = 350.0f;
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
    private bool canMove = true;

    private float tiempoAtaque;
    private float tiempoCombo;
    private float tiempoDash;
    private float recargaSuperDash;
    private float direccion;
    private float cooldownCombo;
    private float CoeficienteFuerzaVelocidad = 1.0f;
    private float EscalaOriginal;
    private float velocidad = 5.0f;
    private float fuerzaSalto = 25.0f;
    private float inicioTiempoDash = 0.1f;

    private int dir;

    // START
    void Start ()
    {
        // Poner variables en su estado inicial
        EscalaOriginal = transform.localScale.y;
        combo = new int[3];
        vaciarArreglo();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        tiempoDash = inicioTiempoDash;
        //src_over = GetComponent<src_Overlay>();
	}

    // Update is called once per frame
    private void Update()
    {
        //Salto y DobleSalto
        IsGrounded(); // Checa si se esta tocando suelo
        recargarCombo(); // Cooldown para hacer otro combo
        recargarAtaqueDebil(); // Cooldown ataque debil
        mantenerCombo(); // Tiempo en el que se puede seguir con la cadena de combo
        CalcularValorCoeficienteFV(); // Este coeficiente hace la relacion entre el tamaño, la fuerza y la velocidad

        bool aButton = Input.GetButtonDown("A Button");
        bool xButton = Input.GetButton("X Button");

        // Tecla L para recibir daño (Temporal para hacer pruebas)
        if (Input.GetKeyDown(KeyCode.L))
        {
            takeDamage();
        }
        else
        {
            anim.SetBool("Damage", false);
        }

        //Ataque debil con la Tecla D
        if (canAttackDebil && canMove)
        {
            if ((Input.GetKeyDown(KeyCode.D) || xButton) && canCombo)
            {
                ataqueDebil();
            }
        }
    }

    //Usar preferentemete para físicas
    void FixedUpdate()
    {
        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");

        MoverJugador();

        float rTrigger = Input.GetAxis("Right Trigger");
        bool rBumper = Input.GetButtonDown("Right Bumper");
        bool aButton = Input.GetButtonDown("A Button");

        //SuperDash
        if (tiempoDash < 0f)
        {
            tiempoDash = inicioTiempoDash;
            recargaSuperDash = 0f;
            rb2d.velocity = Vector2.zero;
            superDash = false;
            canDash = false;
            anim.SetBool("isDashing", false);
            anim.SetBool("isDashingDU", false);
        }

        //Movimiento en X control
        if (moveH > 0.0f)
        {
            anim.SetBool("isRunning", true);
            moveH = 8f;
            direccion = 1f;
        }
        else if (moveH < 0.0f)
        {
            anim.SetBool("isRunning", true);
            moveH = -8f;
            direccion = -1f;
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        UnityEngine.Vector3 ctrlMove = new UnityEngine.Vector3(moveH, 0f, 0f);
        transform.position += ctrlMove * velocidad * Time.deltaTime;

        //Salto y doble salto
        if (grounded)
        {
            anim.SetBool("Grounded", true);
            anim.SetBool("isJumping", false);
            dobleSalto = true;
        }
        else
        {
            anim.SetBool("Grounded", false);
            anim.SetBool("isJumping", false);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || aButton) && dobleSalto)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("Grounded", false);
            rb2d.velocity = UnityEngine.Vector2.up * fuerzaSalto;

            if ((Input.GetKeyDown(KeyCode.Space) || aButton) && !grounded && dobleSalto)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("Grounded", false);
                rb2d.velocity = UnityEngine.Vector2.up * fuerzaSalto;
                dobleSalto = false;
            }
        }

        //Dash control
        if (moveH > 0 && moveV > 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = new UnityEngine.Vector2(1f, 1f) * 200;
            superDash = true;
            anim.SetBool("isDashingDU", true);
        }
        else if (moveH < 0 && moveV > 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = new UnityEngine.Vector2(-1f, 1f) * 200;
            superDash = true;
            anim.SetBool("isDashingDU", true);
        }
        else if (moveH > 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.right * velocidadSuperDash;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if (moveH < 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.left * velocidadSuperDash;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if (moveV > 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.up * velocidadSuperDash;
            superDash = true;
            anim.SetBool("isDashingDU", true);
        }

        //Dash teclado
        if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = new UnityEngine.Vector2(1f, 1f) * 200;
            superDash = true;
            anim.SetBool("isDashing", true);
            anim.SetBool("Grounded", false);
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = new UnityEngine.Vector2(-1f, 1f) * 200;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.right * velocidadSuperDash;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.left * velocidadSuperDash;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.up * velocidadSuperDash;
            superDash = true;
            anim.SetBool("isDashing", true);
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
        Collider2D[] dañoEnemigos = Physics2D.OverlapCircleAll(posicionAtaque.position, rangoAtaque, esEnemigo);
        for (int i = 0; i < dañoEnemigos.Length; i++)
        {
            dañoEnemigos[i].GetComponent<BasicEnemyController>().takeDamage(_daño*CoeficienteFuerzaVelocidad);
            if(transform.localScale.x > limiteEscalaInferior)
            {
                transform.localScale = new Vector3(transform.localScale.x - 0.2f, transform.localScale.y - 0.2f, transform.localScale.z);
                //src_over.adjustOverlayColor(-0.07f);
            }
        }
    }

    public void takeDamage()
    {
        if (transform.localScale.x < limiteEscalaSuperior)
        {
            anim.SetBool("Damage", true);
            CoeficienteFuerzaVelocidad += 0.1f;
            transform.localScale = new Vector3(transform.localScale.x + 0.2f, transform.localScale.y + 0.2f, transform.localScale.z);
            //src_over.adjustOverlayColor(0.07f);
        }
        else
        {
            anim.SetBool("Damage", false);
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
                anim.SetBool("Attack", true);

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

    // Lista de combos
    void ataqueCombo()
    {
        // Debil, debil: daño total 11
        if (combo[0] == 1 && combo[1] == 1 && combo[2] == 0)
        {
            anim.SetBool("Attack", true);
            HacerDaño(daño * 1.2f, 1);
        }

        // Debil, debil, debil: daño total 20
        if (combo[0] == 1 && combo[1] == 1 && combo[2] == 1)
        {
            anim.SetBool("Attack", true);
            HacerDaño(daño * 1.8f, 1);
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
        anim.SetBool("isDead", true);
        //src_over.adjustOverlayColor(-1.12f);
    }

    void reestablecer()
    {
        anim.SetBool("isDead", false);
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(tiempoEspera(1.0f));
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
