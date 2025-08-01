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
    private List<Vector3> m_vertices;
    public const float HOLE_LIFETIME = 5;
    public GameObject holeRefillVisuals;
    private Vector3 spawnedPos;

    private void Start()
    {
        spawnedPos = transform.position;
        m_spawnTime = Time.time;
        m_meshCollider = GetComponentInChildren<MeshCollider>();
        m_vertices = GetVertices();

        // Find all holes inside this hole and make them fall too
        foreach (GameObject holeObj in HoleCutter.Holes)
        {
            if (holeObj == null) return;
            Hole hole = holeObj.GetComponent<Hole>();
            if (hole == null || hole == this) return;

            if (!hole.m_isDead && m_spawnTime > hole.m_spawnTime && !hole.m_isFalling)
            {
                if (ContainsHole(hole))
                {
                    hole.FallDown();
                }
                else if (OverlapsHole(hole))
                {
                    GameObject newHole = Instantiate(hole.gameObject);
                    newHole.GetComponent<Hole>().FallDown();
                    newHole.GetComponent<Hole>().enabled = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > HOLE_LIFETIME + m_spawnTime && !m_isDead)
        {
            m_isDead = true;
            SpawnRespawnVisuals();
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
            other.gameObject.GetComponent<Rigidbody>().linearVelocity.Normalize();
            other.gameObject.GetComponent<Rigidbody>().linearVelocity /= 5;
        }
    }

    private bool ContainsHole(Hole hole)
    {
        foreach (Vector3 vertex in hole.m_vertices)
        {
            if (!GeometryUtils.PointInPolygon3D(vertex, m_vertices))
            {
                return false;
            }
        }
        return true;
    }

    private bool OverlapsHole(Hole hole)
    {
        foreach (Vector3 vertex in hole.m_vertices)
        {
            if (GeometryUtils.PointInPolygon3D(vertex, m_vertices))
            {
                return true;
            }
        }
        return false;
    }

    // TODO: This check does not work well for partial intersection
    private bool ContainsBounds(Bounds otherBounds)
    {
        Bounds myBounds = m_meshCollider.bounds;
        Vector3 min = otherBounds.min;
        min.y = myBounds.min.y;
        Vector3 max = otherBounds.max;
        max.y = myBounds.max.y;

        return GeometryUtils.PointInPolygon(min, m_vertices) && GeometryUtils.PointInPolygon(max, m_vertices);
    }

    private List<Vector3> GetVertices()
    {
        List<Vector3> vertices = new(GetComponent<MeshFilter>().mesh.vertices);
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);
        }
        List<Vector3> convexHull = new();
        GeometryUtils.ConvexHull2D(vertices, convexHull);
        return convexHull;
    }

    private void FallDown()
    {
        m_isFalling = true;
        Rigidbody rb = GetComponent<Rigidbody>();

        // Avoid error for convex geometry collision on dynamic rigidbodies
        m_meshCollider.convex = true;
        m_meshCollider.isTrigger = true;

        // Apply rigidbody flags
        rb.isKinematic = false;
        rb.useGravity = true;

        // Ignore ice and hole
        rb.excludeLayers = 1 << 6 | 1 << 7;

        // Nudge up to prevent z fighting
        rb.MovePosition(new Vector3(rb.transform.position.x, rb.transform.position.y + 0.01f, rb.transform.position.z));

        // Apply new mask material to apply to falling cutout
        GetComponent<MeshRenderer>().material = m_maskMat;
    }
    
    public void SpawnRespawnVisuals()
    {
        GameObject addedVisuals = Instantiate(holeRefillVisuals, spawnedPos, transform.rotation);
        addedVisuals.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
    }
}
