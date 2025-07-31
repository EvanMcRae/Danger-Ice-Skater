using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class Hole : MonoBehaviour
{
    private float m_spawnTime, m_killHeight = -10f;
    private bool m_isDead = false, m_isFalling = false;
    [SerializeField] private Material m_maskMat;
    [SerializeField] private MeshCollider m_meshCollider;

    private void Start()
    {
        m_spawnTime = Time.time;
        m_meshCollider = GetComponentInChildren<MeshCollider>();

        // Find all holes inside this hole and make them fall too
        // TODO: This is not a super accurate check!! It unfortunately cannot use actual concave->concave
        // collision to tell when/where holes intersect, or when holes are partially intersecting.
        // This is actually a really really bad idea now that I think about it.
        foreach (GameObject holeObj in HoleCutter.Holes)
        {
            if (holeObj == null) return;
            Hole hole = holeObj.GetComponent<Hole>();
            if (hole == null || hole == this) return;

            if (!hole.m_isDead //&& ContainsHole(hole)
                && m_spawnTime > hole.m_spawnTime && !hole.m_isFalling)
            {
            hole.m_isFalling = true;
                Rigidbody rb = hole.GetComponent<Rigidbody>();
                hole.m_meshCollider.convex = true;
                hole.m_meshCollider.isTrigger = true;
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.excludeLayers = 1 << 6 | 1 << 7; // ignore ice and hole
                rb.MovePosition(new Vector3(rb.transform.position.x, rb.transform.position.y + 0.001f, rb.transform.position.z)); // nudge up to prevent z fighting
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
            HoleCutter.Holes.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (m_isDead) return;

        if (ContainsBounds(other.collider.bounds))
        {
            other.gameObject.GetComponent<Rigidbody>().excludeLayers = 1 << 6 | 1 << 7; // ignore ice and hole
            // other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }

    private bool ContainsHole(Hole hole)
    {
        // List<Vector3> myVertices = GetVertices();
        // List<Vector3> holeVertices = hole.GetVertices();

        // foreach (Vector3 vertex in holeVertices)
        // {
        //     if (!GeometryUtils.PointInPolygon(vertex, myVertices))
        //     {
        //         return false;
        //     }
        // }
        return false;
    }

    private bool ContainsBounds(Bounds otherBounds)
    {
        List<Vector3> myVertices = GetVertices();

        Bounds myBounds = m_meshCollider.bounds;
        Vector3 min = otherBounds.min;
        min.y = myBounds.min.y;
        Vector3 max = otherBounds.max;
        max.y = myBounds.max.y;

        return GeometryUtils.PointInPolygon(min, myVertices) && GeometryUtils.PointInPolygon(max, myVertices);
    }

    private List<Vector3> GetVertices()
    {
        List<Vector3> vertices = new(GetComponent<MeshFilter>().mesh.vertices);
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }
        return vertices;
    }
}
