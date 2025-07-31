using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoleCutter : MonoBehaviour
{
    public struct Point
    {
        public Vector2? coord;
        public int age;
    }

    [SerializeField] private GameObject HolePrefab;
    [SerializeField] private List<GameObject> Holes;

    public bool isDrawing = false;
    public bool hasOverlapped = false;
    public int overlapStart = -1, overlapEnd = -1;

    public const float LOOP_ALLOWANCE = 0.2f;
    public const int MIN_POINTS = 30;
    public const float RESOLUTION = 2f;

    private LineRenderer lineRenderer;
    private Vector2 lastMousePos;

    public const int MAX_POINTS = 400;
    public List<Point> Points = new();

    public int numPoints = 0;

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
                Physics.Raycast(ray, out RaycastHit hit, 1000, ~(1 << 2));

                numPoints++;

                if (lineRenderer.positionCount < MAX_POINTS)
                {
                    lineRenderer.positionCount++;
                }

                // Keep cycling list of points
                Point newPoint;
                newPoint.coord = new(hit.point.x, hit.point.z);
                newPoint.age = numPoints;
                Points.Add(newPoint);
                if (numPoints > MAX_POINTS)
                {
                    Points.RemoveAt(0);
                }

                lineRenderer.SetPosition((numPoints - 1) % MAX_POINTS, hit.point - transform.position);
                
                //if ((numPoints - 1) % MAX_POINTS == 0) Debug.Break();
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

        // TODO DEBUG -- reset key
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
        if (hasOverlapped)
        {
            // Spawn new hole, converting drawn points into a PolygonCollider2D and then into a mesh
            GameObject newHole = Instantiate(HolePrefab, transform.position, Quaternion.identity);
            PolygonCollider2D poly = newHole.GetComponentInChildren<PolygonCollider2D>();

            // Get only the two points that touched
            List<Vector2> pts = new();
            for (int i = overlapStart; i <= overlapEnd; i++)
            {
                if (Points[i].coord != null)
                    pts.Add(Points[i].coord.Value);
            }

            poly.points = pts.ToArray();

            // Null out the points in the hole
            for (int i = overlapStart; i <= overlapEnd; i++)
            {
                Point pt = Points[i];
                pt.coord = null;
                Points[i] = pt;
            }

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

        isDrawing = false;
        hasOverlapped = false;
    }

    void CheckOverlap()
    {
        if (hasOverlapped) return;
        for (int i = 0; i < Mathf.Min(numPoints, MAX_POINTS) - 1; i++)
        {
            for (int j = i + 1; j < Mathf.Min(numPoints, MAX_POINTS); j++)
            {
                if (Points[i].coord != null && Points[j].coord != null)
                {
                    if (Vector2.Distance(Points[i].coord.Value, Points[j].coord.Value) <= LOOP_ALLOWANCE
                        && Points[j].age - Points[i].age >= MIN_POINTS)
                    {
                        hasOverlapped = true;
                        overlapStart = i;
                        overlapEnd = j;
                        return;
                    }
                }
            }
        }
    }
}
