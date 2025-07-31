using Enemies;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    Rigidbody rb;
    public InputManager imso;

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

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //make the player move
        //modified by rigidbody's Linear Dampening
        float horizontal = imso.xAxis.action.ReadValue<float>();
        float vertical = imso.yAxis.action.ReadValue<float>();
        if (rb.linearVelocity.x > 0 && horizontal < 0 || rb.linearVelocity.x < 0 && horizontal > 0)
        {
            horizontal = horizontal * reverseMod;
        }

        if (rb.linearVelocity.y > 0 && vertical < 0 || rb.linearVelocity.y < 0 && vertical > 0)
        {
            vertical = vertical * reverseMod;
        }

        rb.AddForce(horizontal * acceleration, 0, vertical * acceleration);
    }

    
    // Testing combat, substitute for "killing enemies with the ice break"
    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            e.DestroyEnemy();
        }
    }
}
