using Enemies;
using Input;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public InputManager imso;

    [SerializeField]
    PauseManager pm;

    public PlayerStatsHandler psh;

    /// <summary>
    /// The rate at which the player accelerates when they move
    /// </summary>
    [SerializeField, Tooltip("The rate at which the player accelerates when they move")]
    private int acceleration;

    /// <summary>
    /// The modifier to player acceleration if they move in the opposite direction (a value of 2 means they slow down 2x as fast as they speed up)
    /// </summary>
    [SerializeField, Tooltip("The modifier to player acceleration if they move in the opposite direction (a value of 2 means they slow down 2x as fast as they speed up)")]
    private float reverseMod;

    public bool isTouchingGround;
    
    public float dashForce;

    public float dashCooldown;
    public float dashTimer;

    public float respawnHeight;

    public float fallThroughHoleTimer;
    public float fallThroughHoleTime;

    public MaskObject arenaFloor;

    public static bool gameOvered = false;

    public ParticleSystem particles;

    [SerializeField]
    public Animator anim;

    private bool inAir = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        pm = FindAnyObjectByType<PauseManager>();
        gameOvered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (imso.pause.action.WasPressedThisFrame() && pm != null && psh != null && psh.health != 0)
        {
            if (PauseManager.paused)
            {
                pm.Unpause();
            }
            else
            {
                pm.Pause();
            }
        }

        if (PauseManager.paused) return;

        if (imso.jump.action.WasPressedThisFrame()) Jump();

        dashTimer = Mathf.Max(dashTimer - Time.deltaTime, 0);

        if (imso.dash.action.WasPressedThisFrame() &&
            (imso.xAxis.action.IsPressed() || imso.yAxis.action.IsPressed()) &&
            dashTimer <= 0)
        {

            Dash();
            dashTimer = dashCooldown;
        }

        if (gameOvered) {
            fallThroughHoleTimer -= Time.deltaTime;
        }

        Vector2 horizVel = new(rb.linearVelocity.x, rb.linearVelocity.z);
        if (horizVel.magnitude >= 0.8f)
        {
            anim.SetBool("isMoving", true);
        }
        else { 
            anim.SetBool("isMoving", false);
        }
    }

    void FixedUpdate()
    {
        if (PauseManager.paused) return;

        //make the player move
        //modified by rigidbody's Linear Dampening
        float horizontal = imso.xAxis.action.ReadValue<float>();
        float vertical = imso.yAxis.action.ReadValue<float>();

        if(gameOvered)
        {
            horizontal = 0;
            vertical = 0;
            if (fallThroughHoleTimer <= 0) {
                rb.AddForce(Vector3.up * dashForce, ForceMode.Force);
                rb.useGravity = false;
            }
            if (transform.position.y > respawnHeight) {
                gameOvered = false;
                rb.useGravity = true;
                rb.excludeLayers = 0;
                rb.linearDamping = 1;
            }
            
        }
        

        if (rb.linearVelocity.x > 0 && horizontal < 0 || rb.linearVelocity.x < 0 && horizontal > 0)
        {
            horizontal = horizontal * reverseMod;
        }

        if (rb.linearVelocity.y > 0 && vertical < 0 || rb.linearVelocity.y < 0 && vertical > 0)
        {
            vertical = vertical * reverseMod;
        }

        //Rotate player to face their movement direction + some bias from their inputs
        if (!gameOvered && new Vector3(rb.linearVelocity.x + horizontal, 0, rb.linearVelocity.z + vertical) != default)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(rb.linearVelocity.x + horizontal, 0, rb.linearVelocity.z + vertical), Vector3.up);
        }


        //Particle system effects
        ParticleSystem.MainModule particleMain = particles.main;
        particleMain.startSpeed = rb.linearVelocity.magnitude/10;
        ParticleSystem.EmissionModule particleEmmission = particles.emission;
        particleEmmission.rateOverTime = Mathf.Pow(rb.linearVelocity.magnitude/2,2);
        if (isTouchingGround && !particles.isPlaying)
        {
            particles.Play();
        }
        if(!isTouchingGround)
        {
            particles.Stop();
        }


        Vector3 moveDir = new Vector3(horizontal * acceleration, 0, vertical * acceleration);

        // Debug.Log(moveDir);
        rb.AddForce(moveDir);

        
        if(transform.position.y < arenaFloor.transform.position.y)
        {
            rb.linearDamping = 4;
            rb.useGravity = false;
        }
    }

    
    // Testing combat, substitute for "killing enemies with the ice break"
    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            psh.Damage(1);
        }
        else if (other.gameObject.layer == 7) // ice
        {
            isTouchingGround = true;
            if (inAir)
            {
                print("what the hell");
                inAir = false;
                anim.ResetTrigger("jump");
                anim.SetTrigger("land");
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 7) // ice
        {
            isTouchingGround = false;
        }
    }
    
    public void Dash() {
        if (gameOvered) return;
        Vector3 dir = new Vector3(imso.xAxis.action.ReadValue<float>(), 0, imso.yAxis.action.ReadValue<float>());
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir * dashForce, ForceMode.Impulse);
    }

    public void Jump() {
        if (gameOvered) return;
        if (!isTouchingGround) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * dashForce, ForceMode.Impulse);
        anim.SetTrigger("jump");
        anim.ResetTrigger("land");
        inAir = true;
    }
}
