using DG.Tweening;
using Game;
using UnityEngine;

public class Cutout : MonoBehaviour
{
    private Color m_tint = Color.white;
    private float m_dissolveProgress = 0f;
    public bool m_isDead = false;
    public Color finalTint;
    private MaterialPropertyBlock m_matBlock;
    private MeshRenderer m_meshRenderer;
    private Tween m_tintTween, m_dissolveTween;
    private bool m_playedSound = false;

    public void Start()
    {
        m_matBlock = new();
        m_meshRenderer = GetComponent<MeshRenderer>();
        Invoke(nameof(StartTintTween), 0.2f);
        Invoke(nameof(StartDissolveTween), 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.ShouldNotRun()) return;

        m_matBlock.SetColor("_Tint_Colour", m_tint);
        m_matBlock.SetFloat("_Dissolve_Progress", m_dissolveProgress);
        m_meshRenderer.SetPropertyBlock(m_matBlock);

        if (transform.position.y < GameController.KILL_HEIGHT && !m_playedSound)
        {
            m_playedSound = true;
            AkUnitySoundEngine.PostEvent("IceSplash", gameObject);
        }

        if (transform.position.y < GameController.KILL_HEIGHT * 2 && !m_isDead)
        {
            m_isDead = true;
            Destroy(gameObject);
        }
    }

    void StartTintTween()
    {
        m_tintTween = DOTween.To(() => m_tint, x => m_tint = x, finalTint, 1.2f);
    }

    void StartDissolveTween()
    {
        m_dissolveTween = DOTween.To(() => m_dissolveProgress, x => m_dissolveProgress = x, 1, 1f);
    }

    void OnDestroy()
    {
        DOTween.Kill(m_tintTween);
        DOTween.Kill(m_dissolveTween);
    }
}
