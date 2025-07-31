using UnityEngine;

public class Hole : MonoBehaviour
{
    private float m_spawnTime, m_killHeight = -10f;
    private bool m_isDead = false, m_isFalling = false;
    [SerializeField] private Material m_maskMat;
    [SerializeField] private MeshCollider m_meshCollider;
    private Bounds m_bounds;

    private void Start()
    {
        m_spawnTime = Time.time;
        m_meshCollider = GetComponentInChildren<MeshCollider>();
        m_bounds = m_meshCollider.bounds; // TODO this might not be populated early enough

        // Find all holes inside this hole and make them fall too
        // TODO: This is not a super accurate check!! It unfortunately cannot use actual concave->concave
        // collision to tell when/where holes intersect, or when holes are partially intersecting.
        // This is actually a really really bad idea now that I think about it.
        foreach (GameObject holeObj in HoleCutter.Holes)
        {
            if (holeObj == null || !holeObj.activeInHierarchy) return;
            Hole hole = holeObj.GetComponent<Hole>();
            if (hole != null && !hole.m_isDead && ContainsBounds(hole.GetComponentInChildren<MeshCollider>().bounds)
                && m_spawnTime > hole.m_spawnTime && !m_isFalling)
            {
                hole.m_isFalling = true;
                Rigidbody rb = hole.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.excludeLayers = 1 << 6 | 1 << 7; // ignore ice and hole
                hole.GetComponent<MeshRenderer>().material = m_maskMat;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < m_killHeight && !m_isDead)
        {
            m_isDead = true;
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (m_isDead) return;

        if (ContainsBounds(other.collider.bounds))
        {
            other.gameObject.GetComponent<Rigidbody>().excludeLayers = 1 << 6 | 1 << 7; // ignore ice and hole
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }

    private bool ContainsBounds(Bounds otherBounds)
    {
        m_bounds = m_meshCollider.bounds;

        Vector3 min = otherBounds.min;
        min.y = m_bounds.min.y;
        Vector3 max = otherBounds.max;
        max.y = m_bounds.max.y;

        return m_bounds.Contains(min) && m_bounds.Contains(max);
    }
}
