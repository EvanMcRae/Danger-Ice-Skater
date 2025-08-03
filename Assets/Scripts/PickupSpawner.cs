using System.Collections;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;
    public Vector3 center = Vector3.zero;
    public Vector2 size = new Vector2(10, 10);
    public float spawnHeight = 0.3f;

    private GameObject pickupA;
    private GameObject pickupB;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 topLeft = center + new Vector3(-size.x / 2, spawnHeight, -size.y / 2);
        Vector3 topRight = center + new Vector3(size.x / 2, spawnHeight, -size.y / 2);
        Vector3 bottomRight = center + new Vector3(size.x / 2, spawnHeight, size.y / 2);
        Vector3 bottomLeft = center + new Vector3(-size.x / 2, spawnHeight, size.y / 2);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }

    private void Start()
    {
        
    }

    public void StartSpawnLoop()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            SpawnTwoPickups();

            // Wait for both to be picked up
            yield return new WaitUntil(() => pickupA == null && pickupB == null);

            float waitTime = Random.Range(10f, 40f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnTwoPickups()
    {
        Vector3 pos1 = RandomPointInRect();
        Vector3 pos2;
        do
        {
            pos2 = RandomPointInRect();
        } while (Vector3.Distance(pos1, pos2) < 1f); // prevent overlapping

        pickupA = Instantiate(pickupPrefab, pos1, Quaternion.identity);
        pickupB = Instantiate(pickupPrefab, pos2, Quaternion.identity);
    }

    private Vector3 RandomPointInRect()
    {
        float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float z = Random.Range(center.z - size.y / 2, center.z + size.y / 2);
        return new Vector3(x, spawnHeight, z);
    }
}
