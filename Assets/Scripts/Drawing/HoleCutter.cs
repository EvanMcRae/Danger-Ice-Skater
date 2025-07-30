using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoleCutter : MonoBehaviour
{
    [SerializeField] private GameObject HolePrefab;
    [SerializeField] private List<GameObject> Holes;

    public List<Vector2> Points = new();

    public bool isDrawing = false;
    public bool hasOverlapped = false;

    public const float LOOP_ALLOWANCE = 1f;
    public const int MIN_POINTS = 8;
    public const float RESOLUTION = 0.1f;

    private LineRenderer lineRenderer;
    private Vector2 lastMousePos;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.IsPressed())
        {
            isDrawing = true;
            Vector2 mousePos = Mouse.current.position.ReadValue();

            if (Vector2.Distance(mousePos, lastMousePos) >= RESOLUTION)
            {
                lastMousePos = mousePos;
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                Physics.Raycast(ray, out RaycastHit hit);
                Points.Add(new(hit.point.x, hit.point.z));
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point - transform.position);
            }
        }

        if (isDrawing)
        {
            CheckOverlap();
            if (ShouldEndDraw())
            {
                EndDraw();
            }
        }

        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            foreach (GameObject _ in Holes)
            {
                Destroy(_);
            }
        }
    }

    bool ShouldEndDraw()
    {
        return hasOverlapped || InputSystem.actions["Jump"].WasPressedThisFrame() || Mouse.current.leftButton.wasReleasedThisFrame;
    }

    void EndDraw()
    {
        if (IsClosedLoop())
        {
            // Spawn new hole, converting drawn points into a PolygonCollider2D and then into a mesh
            GameObject newHole = Instantiate(HolePrefab, transform.position, Quaternion.identity);
            PolygonCollider2D poly = newHole.GetComponentInChildren<PolygonCollider2D>();
            poly.points = Points.ToArray();
            Mesh mesh = poly.CreateMesh(false, false);
            if (mesh == null)
            {
                Debug.LogError("Null hole mesh, did you draw out of bounds?");
            }
            //MeshRenderer meshRenderer = newDrawnObject.GetComponent<MeshRenderer>();
            MeshFilter meshFilter = newHole.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            MeshCollider meshCollider = newHole.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            newHole.transform.SetPositionAndRotation(new Vector3(0, -29.9f, 30), Quaternion.Euler(90, 0, 0));
            poly.enabled = false;
            Holes.Add(newHole);
        }

        // Clear points array for next time
        Points = new();
        lineRenderer.positionCount = 0;
        isDrawing = false;
        hasOverlapped = false;
    }

    void CheckOverlap()
    {
        if (hasOverlapped) return;
        for (int i = 0; i < Points.Count - 2; i++) // 2 instead of 1 to prevent detecting a collision with the previously drawn point (which would otherwise always happen)
        {
            if (Vector2.Distance(Points[i], Points[^1]) < 0.02f)
            {
                hasOverlapped = true;
                return;
            }
        }
    }

    bool IsClosedLoop()
    {
        return Points.Count >= MIN_POINTS && Vector2.Distance(Points[0], Points[^1]) <= LOOP_ALLOWANCE;
    }
}
