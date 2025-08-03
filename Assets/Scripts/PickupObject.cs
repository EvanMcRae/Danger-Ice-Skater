using UnityEngine;
using Game;
using Player;

public class PickupObject : MonoBehaviour
{
    public float spinSpeed = 90f;
    public float bobHeight = 0.5f;
    public float bobSpeed = 2f;

    private Vector3 startPos;

    public GameController gameController;
    public PlayerStatsHandler psh;

    void Start()
    {
        gameController = GameObject.Find("GameManager").GetComponent<GameController>();
        psh = GameObject.Find("Player").GetComponent<PlayerStatsHandler>();
        startPos = transform.position;
        //gameController = gameController.GetComponent<GameController>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);

        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Placeholder logic
            FloatingTextManager.instance.ShowFloatingSprite(transform.position);
            gameController.pickUp();
            psh.Heal(1);
            Debug.Log("Player picked up: " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
