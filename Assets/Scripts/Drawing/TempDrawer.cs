using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempDrawer : MonoBehaviour
{
    [SerializeField] private GameObject DrawnObjectPrefab;
    [SerializeField] private List<GameObject> DrawnObjects;

    public List<Vector2> Points = new();

    public bool isDrawing = false;
    public bool hasOverlapped = false;

    public const float LOOP_ALLOWANCE = 0.2f;
    public const int MIN_POINTS = 8;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.IsPressed())
        {
            isDrawing = true;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Physics.Raycast(ray, out RaycastHit hit);
            Points.Add(new(hit.point.x, hit.point.z));
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
            foreach (GameObject _ in DrawnObjects)
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
            // Spawn new drawn object, converting drawn points into a PolygonCollider2D and then into a mesh
            GameObject newDrawnObject = Instantiate(DrawnObjectPrefab, transform.position, Quaternion.identity);
            PolygonCollider2D poly = newDrawnObject.GetComponentInChildren<PolygonCollider2D>();
            poly.points = Points.ToArray();
            Mesh mesh = poly.CreateMesh(false, false);
            if (mesh == null)
            {
                Debug.LogError("Null mesh!! why? idk, unity sucks!");
            }
            //MeshRenderer meshRenderer = newDrawnObject.GetComponent<MeshRenderer>();
            MeshFilter meshFilter = newDrawnObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            MeshCollider meshCollider = newDrawnObject.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            newDrawnObject.transform.SetPositionAndRotation(new Vector3(0, -29.9f, 30), Quaternion.Euler(90, 0, 0));
            DrawnObjects.Add(newDrawnObject);
        }

        // Clear points array for next time
        Points = new();
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
        return true;

        //return Points.Count >= MIN_POINTS && Vector2.Distance(Points[0], Points[^1]) <= LOOP_ALLOWANCE;
    }
}
