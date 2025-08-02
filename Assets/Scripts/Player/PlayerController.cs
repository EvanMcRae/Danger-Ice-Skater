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
    public bool fallThroughHoleDamaged = false;

    public MaskObject arenaFloor;

    public static bool fallThroughHole = false;

    public ParticleSystem particles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        pm = FindAnyObjectByType<PauseManager>();
        fallThroughHole = false;
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

        if (fallThroughHole) {
            fallThroughHoleTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (PauseManager.paused) return;

        //make the player move
        //modified by rigidbody's Linear Dampening
        float horizontal = imso.xAxis.action.ReadValue<float>();
        float vertical = imso.yAxis.action.ReadValue<float>();

        if (fallThroughHole)
        {
            horizontal = 0;
            vertical = 0;

            if (fallThroughHoleTimer > 0) // float above for a bit for comic effect
            {
                rb.useGravity = false;
                rb.constraints |= RigidbodyConstraints.FreezePosition;
            }
            if (fallThroughHoleTimer <= 0 && fallThroughHoleTimer > -fallThroughHoleTime) // sploosh down
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                rb.useGravity = true;
                rb.AddForce(10 * dashForce * Vector3.down, ForceMode.Force);
            }
            if (fallThroughHoleTimer <= -fallThroughHoleTime)
            {
                if (!fallThroughHoleDamaged)
                {
                    fallThroughHoleDamaged = true;
                    psh.Damage(2);
                }
                rb.useGravity = false;
                if (rb.linearVelocity.y < 0) rb.linearVelocity = Vector3.zero;
                rb.AddForce(3 * dashForce * Vector3.up, ForceMode.Force); // rise up
            }
            if (transform.position.y > respawnHeight) { // once at respawn height
                if (rb.linearVelocity.y > 0) rb.linearVelocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                fallThroughHole = false;
                fallThroughHoleDamaged = false;
                rb.useGravity = true;
                rb.excludeLayers = 0;
                rb.linearDamping = 1;
            }
            
        }
        

        if (rb.linearVelocity.x > 0 && horizontal < 0 || rb.linearVelocity.x < 0 && horizontal > 0)
        {
            horizontal *= reverseMod;
        }

        if (rb.linearVelocity.y > 0 && vertical < 0 || rb.linearVelocity.y < 0 && vertical > 0)
        {
            vertical *= reverseMod;
        }

        //Rotate player to face their movement direction + some bias from their inputs
        if (!fallThroughHole && new Vector3(rb.linearVelocity.x + horizontal, 0, rb.linearVelocity.z + vertical) != default)
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
        if (fallThroughHole) return;
        Vector3 dir = new Vector3(imso.xAxis.action.ReadValue<float>(), 0, imso.yAxis.action.ReadValue<float>());
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir * dashForce, ForceMode.Impulse);
    }

    public void Jump() {
        if (fallThroughHole) return;
        if (!isTouchingGround) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * dashForce, ForceMode.Impulse);
    }
}
