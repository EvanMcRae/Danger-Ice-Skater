using Enemies;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public InputManager imso;

    [SerializeField]
    PauseManager pm;

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


    public MaskObject arenaFloor;

    public static bool gameOvered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        pm = FindAnyObjectByType<PauseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (imso.pause.action.WasPressedThisFrame() && pm != null)
        {
            if (pm.paused)
            {
                pm.Unpause();
            }
            else
            {
                pm.Pause();
            }
        }
        
        if (imso.jump.action.WasPressedThisFrame()) Jump();
            
        dashTimer = Mathf.Max(dashTimer - Time.deltaTime, 0);
            
        if (imso.dash.action.WasPressedThisFrame() && 
            (imso.xAxis.action.IsPressed() || imso.yAxis.action.IsPressed()) &&
            dashTimer <= 0) {
                
            Dash();
            dashTimer = dashCooldown;
        }
    }

    void FixedUpdate()
    {
        //make the player move
        //modified by rigidbody's Linear Dampening
        float horizontal = imso.xAxis.action.ReadValue<float>();
        float vertical = imso.yAxis.action.ReadValue<float>();

        if(gameOvered)
        {
            horizontal = 0;
            vertical = 0;
        }

        if (rb.linearVelocity.x > 0 && horizontal < 0 || rb.linearVelocity.x < 0 && horizontal > 0)
        {
            horizontal = horizontal * reverseMod;
        }

        if (rb.linearVelocity.y > 0 && vertical < 0 || rb.linearVelocity.y < 0 && vertical > 0)
        {
            vertical = vertical * reverseMod;
        }

        Vector3 moveDir = new Vector3(horizontal * acceleration, 0, vertical * acceleration);
        
        Debug.Log(moveDir);
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
            //e.DestroyEnemy();
            Debug.Log("-1 Health -- TODO add this");
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
        Vector3 dir = new Vector3(imso.xAxis.action.ReadValue<float>(), 0, imso.yAxis.action.ReadValue<float>());
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(dir * dashForce, ForceMode.Impulse);
    }

    public void Jump() {
        if (!isTouchingGround) return;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * dashForce, ForceMode.Impulse);
    }
}
