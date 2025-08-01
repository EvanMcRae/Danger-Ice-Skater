using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;

public class HoleRefillVisuals : MonoBehaviour
{
    private float m_spawnTime;
    private float lifetime = .1f;
    private List<Vector3> m_vertices;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > lifetime + m_spawnTime)
        {
            Destroy(gameObject);
        }
    }
}
