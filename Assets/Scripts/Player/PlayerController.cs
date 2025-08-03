using Enemies;
using Enemies.SpecificTypes;
using Input;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public InputManager imso;
    public PlayerSoundsPlayer psp;

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

    [SerializeField]
    public Animator anim;

    private bool inAir = false;

    Vector3 priorVel;

    private float pauseBankingTimerLen = .2f;
    private float pauseBankingTimerStart = 0;
    [SerializeField]
    Slider staminaBar;

    private Vector2 lastPos;

    public bool startCutsceneActive = true;
    public bool startedThisFrame = false;

    public GameObject splashPrefab;

    public Image staminabarImage;
    public Sprite happyImage;
    public Sprite mehImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WaterSplashParticles.splashGameobj = splashPrefab;
        rb = GetComponent<Rigidbody>();

        pm = FindAnyObjectByType<PauseManager>();
        fallThroughHole = false;

        if (staminaBar != null)
        {
            staminaBar.maxValue = dashCooldown;
        }
        lastPos = transform.position;
        lastPos.y = 0;
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

        if (PauseManager.ShouldNotRun()) return;
        if (startCutsceneActive) return;

        if (startedThisFrame)
        {
            startedThisFrame = false;
        }
        else if (imso.jump.action.WasPressedThisFrame()) Jump();

        dashTimer = Mathf.Max(dashTimer - Time.deltaTime, 0);

        if (imso.dash.action.WasPressedThisFrame() &&
            (imso.xAxis.action.IsPressed() || imso.yAxis.action.IsPressed()) &&
            dashTimer <= 0)
        {
            AkUnitySoundEngine.PostEvent("PlayerDash", gameObject);
            Dash();
            dashTimer = dashCooldown;
        }

        if (staminaBar != null)
        {
            staminaBar.value = dashTimer;

            if (dashTimer <= 0)
                staminabarImage.sprite = happyImage;
            else
                staminabarImage.sprite = mehImage;
        }

        if (fallThroughHole)
        {
            fallThroughHoleTimer -= Time.deltaTime;
        }

        if(psh.killed)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
    }

    void FixedUpdate()
    {
        if (PauseManager.ShouldNotRun()) return;
        if (startCutsceneActive) return;

        //make the player move
        //modified by rigidbody's Linear Dampening
        float horizontal = imso.xAxis.action.ReadValue<float>();
        float vertical = imso.yAxis.action.ReadValue<float>();


        anim.SetBool("useGlideAnim", (Vector2.Dot(new Vector2(horizontal, vertical), new Vector2(rb.linearVelocity.x, rb.linearVelocity.z)) > 0 && rb.linearVelocity.magnitude > 10)
                                     || (horizontal == 0 && vertical == 0));

        if (!anim.GetBool("useGlideAnim")) psp.isGliding = false;

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
                    psp.RunSplashSound();
                    fallThroughHoleDamaged = true;
                    WaterSplashParticles.CreateSplashParticles(transform.position.x, transform.position.z);
                    psh.Damage(2);
                }
                rb.useGravity = false;
                if (rb.linearVelocity.y < 0) rb.linearVelocity = Vector3.zero;
                rb.AddForce(3 * dashForce * Vector3.up, ForceMode.Force); // rise up
            }
            if (transform.position.y > respawnHeight)
            { // once at respawn height
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


        if ((anim.GetCurrentAnimatorStateInfo(0).IsName("Glide") || anim.GetCurrentAnimatorStateInfo(0).IsName("Skate")) && pauseBankingTimerStart + pauseBankingTimerLen < Time.time)
        {
            //Rotate player to lean into turns
            //float bankingAmount = Mathf.Sin(Vector2.SignedAngle(new Vector2(rb.linearVelocity.x, rb.linearVelocity.z), new Vector2(horizontal, vertical)) * Mathf.Deg2Rad);
            float bankingAmount = Mathf.Sin(Vector2.SignedAngle(new Vector2(rb.linearVelocity.x, rb.linearVelocity.z), new Vector2(priorVel.x, priorVel.z)) * Mathf.Deg2Rad);

            //anim.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(bankingAmount * -200 * (rb.linearVelocity.magnitude/11), -45, 45));
            anim.transform.localRotation = Quaternion.Euler(0, 0, -Mathf.Clamp(bankingAmount * Mathf.Pow(rb.linearVelocity.magnitude, 2) * 1.5f, -45, 45));
        }
        else
        {
            anim.transform.localRotation = Quaternion.Euler(0, 0, 0);
            priorVel = new Vector3();
        }



        //Particle system effects
        ParticleSystem.MainModule particleMain = particles.main;
        particleMain.startSpeed = rb.linearVelocity.magnitude / 10;
        ParticleSystem.EmissionModule particleEmmission = particles.emission;
        particleEmmission.rateOverTime = Mathf.Pow(rb.linearVelocity.magnitude / 2, 2);
        if (isTouchingGround && !particles.isPlaying)
        {
            particles.Play();
        }
        if (!isTouchingGround)
        {
            particles.Stop();
        }


        Vector3 moveDir = new Vector3(horizontal * acceleration, 0, vertical * acceleration);

        // Debug.Log(moveDir);
        rb.AddForce(moveDir);


        if (transform.position.y < arenaFloor.transform.position.y)
        {
            rb.linearDamping = 4;
            rb.useGravity = false;
        }

        priorVel = (4 * priorVel + rb.linearVelocity) / 5;

        Vector2 currPos = new(transform.position.x, transform.position.z);
        Vector2 horizVel = (currPos - lastPos) / Time.fixedDeltaTime;
        if (horizVel.magnitude >= 0.8f)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
            psp.isGliding = false;
        }
        AkUnitySoundEngine.SetRTPCValue("playerVelocity", 100f * Mathf.Clamp01(horizVel.magnitude / 10f));
        lastPos = currPos;

        if (inAir && rb.linearVelocity.y < 0)
        {
            Ray ray = new(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1f, 1 << 7, QueryTriggerInteraction.Ignore))
            {
                anim.ResetTrigger("jump");
                anim.SetTrigger("land");
                inAir = false;
            }
        }
    }

    
    // Testing combat, substitute for "killing enemies with the ice break"
    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            psh.Damage(1);

            if(!other.gameObject.TryGetComponent(out LandMineDestroy l) && !other.gameObject.TryGetComponent(out Puck p) && other.transform.position.y + .5f < transform.position.y)
            {
                rb.MovePosition(transform.position + new Vector3(transform.position.x - other.transform.position.x, 0, transform.position.z - other.transform.position.z).normalized + Vector3.down * .5f);
            }
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
        priorVel = rb.linearVelocity;
        pauseBankingTimerStart = Time.time;
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
        priorVel = rb.linearVelocity;
    }

    public void Jump() {
        if (fallThroughHole) return;
        if (!isTouchingGround) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * dashForce, ForceMode.Impulse);
        anim.SetTrigger("jump");
        anim.ResetTrigger("land");
        AkUnitySoundEngine.PostEvent("PlayerJump", gameObject);
        inAir = true;
        pauseBankingTimerStart = Time.time;
    }
}
