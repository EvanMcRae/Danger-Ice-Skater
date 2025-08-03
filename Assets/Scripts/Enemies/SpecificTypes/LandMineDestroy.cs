using UnityEngine;
using Utility;

public class LandMineDestroy : MonoBehaviour
{
    public float destroyDelay;
    public bool destroyOnPlayerCollision;
    private float time;
    public GameObject explosion;

    public void Start()
    {
        time = destroyDelay;
    }

    public void Update()
    {
        if (PauseManager.ShouldNotRun()) return;

        destroyDelay -= Time.deltaTime;
        if (destroyDelay <= 0)
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
    public void OnCollisionEnter(Collision other)
    {
        if (!destroyOnPlayerCollision) return;
        if (other.gameObject.CompareTag("Player") && destroyDelay > 0)
        {
            Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
