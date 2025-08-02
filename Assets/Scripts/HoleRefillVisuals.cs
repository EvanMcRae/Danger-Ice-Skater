using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.XR.CoreUtils;
using DG.Tweening;

public class HoleRefillVisuals : MonoBehaviour
{
    [SerializeField] private float m_lifetime = 2f;
    private float m_spawnTime;
    private float m_dissolveProgress = 1;
    private MeshRenderer m_meshRenderer;
    private bool m_isDead = false;
    private MaterialPropertyBlock m_matBlock;
    private Hole m_hole;
    private Tween m_tween;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_spawnTime = Time.time;
        m_tween = DOTween.To(() => m_dissolveProgress, x => m_dissolveProgress = x, 0, m_lifetime);
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_matBlock = new();
        m_matBlock.SetFloat("_Dissolve_Progress", 1);
        m_meshRenderer.SetPropertyBlock(m_matBlock);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.paused) return;
        
        if (!m_isDead)
        {
            m_matBlock.SetFloat("_Dissolve_Progress", m_dissolveProgress);
            m_meshRenderer.SetPropertyBlock(m_matBlock);
        }

        if (Time.time > m_lifetime + m_spawnTime && !m_isDead)
        {
            Destroy(gameObject);
            m_hole.RemoveHole();
            m_isDead = true;
        }
    }

    public void SetHole(Hole hole)
    {
        m_hole = hole;
    }

    public void FallDown()
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<MeshRenderer>().material.renderQueue = 3000;
    }

    void OnDestroy()
    {
        if (m_tween.active)
            DOTween.Kill(m_tween);
    }
}
