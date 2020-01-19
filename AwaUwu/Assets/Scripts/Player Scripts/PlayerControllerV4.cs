using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControllerV4 : MonoBehaviour
{

    //VariablesPublicas
    public Transform groundCheck;
    public LayerMask isGround;
    public Transform wallCheck;
    public LayerMask isWall;
    public Transform attackPos;
    public LayerMask isEnemy;
    public Transform shotPos;
    public GameObject Projectile;
    //VariableTipoFloat
    public float speed;
    public float jumpForce;
    public float radius;
    public float dashSpeed;
    public float startTimeAttack  ;
    public float attackRange;    
    public float startDashTime;
    public float scnDashSpeed;
    public float startTimeBtwShots;
    public float sprintSpeed;
    public float startSprintTime;
    //VariableTipoInt
    public int dir = 0;
    public int damage;

    //VariablesPrivadas
    private Rigidbody2D rb2d;
    private Animator anim;
    //VariableTipoBool
    private bool facingRight = true;
    private bool grounded;
    private bool movement = true;
    private bool superDash = false;
    private bool canDash;
    private bool canSprint;
    public bool canJump;
    public bool doubleJump;
    //VariableTipoFloat
    private float attackTime;
    private float dashTime;
    private float timeBtwShots;
    private float rechargeSuperDash;
    private float jumpPressed;
    //TipoInt
    public int jumps;

    // Use this for initialization
    void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        bool aButton = Input.GetButtonDown("A Button");
        /*bool xButton = Input.GetButton("X Button");
        bool yButton = Input.GetButton("Y Button");
        bool startButton = Input.GetButton("Start");*/
        
        /*float moveV = Input.GetAxis("Vertical") * speed;
        float moveHR = Input.GetAxis("Mouse X") * speed;
        float moveVR = Input.GetAxis("Mouse Y") * speed;*/

        //Salto y DobleSalto
        if (grounded)
         {
             anim.SetBool("Grounded", true);
             anim.SetBool("isJumping",false);
             doubleJump = true;
         }
         else
         {
             anim.SetBool("Grounded", false);
             anim.SetBool("isJumping", false);
         }

         if ((Input.GetKeyDown(KeyCode.Space) || aButton) && doubleJump)
         {
             anim.SetBool("isJumping", true);
            anim.SetBool("Grounded", false);
            rb2d.velocity = UnityEngine.Vector2.up * jumpForce;

             if((Input.GetKeyDown(KeyCode.Space) || aButton) && !grounded && doubleJump)
             {
                 anim.SetBool("isJumping", true);
                anim.SetBool("Grounded", false);
                rb2d.velocity = UnityEngine.Vector2.up * jumpForce;
                 doubleJump = false;
             }
         }

        //Ataque
        if (attackTime <= 0)
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                anim.SetBool("Attack", true);                

                Collider2D[] enemiesDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isEnemy);

                attackTime = startTimeAttack;

                for (int i = 0; i < enemiesDamage.Length; i++)
                {
                    enemiesDamage[i].GetComponent<BasicEnemyController>().takeDamage(damage);
                }                
            }
        }
        else
        {
            anim.SetBool("Attack", false);
            attackTime -= Time.deltaTime;
        }

        //Dash
        if (dir == 1 && Input.GetKeyDown(KeyCode.D) && doubleJump && !grounded)
        {
            rb2d.AddForce(UnityEngine.Vector3.right * dashSpeed);
            rb2d.velocity = UnityEngine.Vector2.up * 5;
            doubleJump = false;
        }
        else if (dir == 2 && Input.GetKeyDown(KeyCode.D) && doubleJump && !grounded)
        {
            rb2d.AddForce(UnityEngine.Vector3.left * dashSpeed);
            rb2d.velocity = UnityEngine.Vector2.up * 5;
            doubleJump = false;
        }

    }

    void FixedUpdate()
    {
        float direction = 0f;

        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, isGround);

        float moveH = Input.GetAxis("Horizontal");
        float moveV = Input.GetAxis("Vertical");
        float rTrigger = Input.GetAxis("Right Trigger");
        bool rBumper = Input.GetButtonDown("Right Bumper");

        //SuperDash
        if(dashTime < 0f)
        {
            dashTime = startDashTime;
            rechargeSuperDash = 0f;
            rb2d.velocity = UnityEngine.Vector2.zero;
            superDash = false;
            canDash = false;
            anim.SetBool("isDashing", false);
            anim.SetBool("isDashingDU", false);
        }

        //Dash control
        if(moveH > 0 && moveV > 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = new UnityEngine.Vector2(1f, 1f) * 200;
            superDash = true;
            anim.SetBool("isDashingDU", true);
        }
        else if(moveH < 0 && moveV > 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = new UnityEngine.Vector2(-1f, 1f) * 200;
            superDash = true;
            anim.SetBool("isDashingDU", true);
        }
        else if(moveH > 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.right * scnDashSpeed;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if(moveH < 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.left * scnDashSpeed;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if(moveV > 0 && rTrigger > 0 && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.up * scnDashSpeed;
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
        else if(Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = new UnityEngine.Vector2(-1f, 1f) * 200;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.right * scnDashSpeed;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if (Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.left * scnDashSpeed;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        else if(Input.GetKeyDown(KeyCode.A) && Input.GetKey(KeyCode.UpArrow) && canDash)
        {
            rb2d.velocity = UnityEngine.Vector2.up * scnDashSpeed;
            superDash = true;
            anim.SetBool("isDashing", true);
        }
        
        //Recarga dash
        if (canDash == false)
        {
            rechargeSuperDash += Time.deltaTime;

            if (rechargeSuperDash >= 1.5f)
            {
                canDash = true;
                Debug.Log("Dash recargado");
            }
        }

        if(superDash)
        {
            dashTime -= Time.deltaTime;
        }           

        //Movimiento en X teclado
        if ((Input.GetKey(KeyCode.RightArrow)) && movement)
        {
            anim.SetBool("isRunning", true);
            direction = 1f;
            dir = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && movement)
        {
            anim.SetBool("isRunning", true);
            direction = -1f;
            dir = 2;
        }        
        else
        {
            anim.SetBool("isRunning", false);
            dir = 0;
        }

        //Debug.Log(moveH);

        UnityEngine.Vector3 moveDir = new UnityEngine.Vector3(direction, 0);
        transform.position += moveDir * speed * Time.deltaTime;

        //Movimiento en X control
        if(moveH > 0.0f)
        {
            anim.SetBool("isRunning", true);
            moveH =8f;
            direction = 1f;
        }
        else if(moveH < 0.0f)
        {
            anim.SetBool("isRunning", true);
            moveH = -8f;
            direction = -1f;
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        UnityEngine.Vector3 ctrlMove = new UnityEngine.Vector3(moveH, 0f, 0f);
        transform.position += ctrlMove * speed * Time.deltaTime;

        //VoltearJugador
        if (facingRight == false && direction > 0)
        {
            Flip();
        }
        else if (facingRight == true && direction < 0)
        {
            Flip();
        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

}
