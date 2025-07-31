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
    public const int MAX_POINTS = 200;
    private LineRenderer lineRenderer;
    private Vector2 lastMousePos;

    [SerializeField] private float planeHeight;

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
                Vector2 pointToAdd = new(hit.point.x, hit.point.z);

                if (Points.Count > 0)
                {
                    int intersectPoint = -1;
                    //Check if new segment intersects with any prior segment
                    for (int i = 1; i < Points.Count; i++)
                    {
                        if (lineSegmentsIntersect(Points[i - 1], Points[i], pointToAdd, Points[^1]))
                        {
                            intersectPoint = i - 1;
                            break;
                        }
                    }

                    if (Points.Count - intersectPoint <= 5)
                        intersectPoint = -1;

                    List<Vector2> cutoutPoints = new List<Vector2>();

                    
                    if (intersectPoint >= 0)
                    {
                        //Copy segment that creates hole.
                        for (int i = intersectPoint; i < Points.Count; i++)
                        {
                            cutoutPoints.Add(Points[i]);
                        }

                        MakeCutout(cutoutPoints);
                        Debug.Log("CuttingSegment");
                    }
                }

                if (Points.Count < MAX_POINTS)
                {
                    Points.Add(new(hit.point.x, hit.point.z));
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point - transform.position);
                }
                else
                {
                    //Move buffer over. Poor scaling run time (O(n)), but size is capped small enough it is not an issue. 
                    Points.RemoveAt(0);
                    Points.Add(new(hit.point.x, hit.point.z));

                    for(int i = 0; i < lineRenderer.positionCount; i++)
                    {
                        lineRenderer.SetPosition(i, new Vector3(Points[i].x, 0, Points[i].y));
                    }
                    
                }
                
            }
        }

        if (isDrawing)
        {
            if (ShouldEndDraw())
            {
                // Clear points array for next time
                Points = new();
                lineRenderer.positionCount = 0;
                isDrawing = false;
                hasOverlapped = false;
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
        return InputSystem.actions["Jump"].WasPressedThisFrame() || Mouse.current.leftButton.wasReleasedThisFrame;
    }


    void MakeCutout(List<Vector2> points)
    {
        // Spawn new hole, converting drawn points into a PolygonCollider2D and then into a mesh
        GameObject newHole = Instantiate(HolePrefab, transform.position, Quaternion.identity);
        PolygonCollider2D poly = newHole.GetComponentInChildren<PolygonCollider2D>();
        poly.points = points.ToArray();
        Mesh mesh = poly.CreateMesh(false, false);
        if (mesh == null)
        {
            Debug.LogError("Null hole mesh, did you draw out of bounds?");
        }
        MeshFilter meshFilter = newHole.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshCollider meshCollider = newHole.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        newHole.transform.SetPositionAndRotation(new Vector3(0, planeHeight + 0.0001f, 30), Quaternion.Euler(90, 0, 0));
        poly.enabled = false;
        Holes.Add(newHole);
    }

    //From https://www.reddit.com/r/gamedev/comments/7ww4yx/whats_the_easiest_way_to_check_if_two_line/
    public static bool lineSegmentsIntersect(Vector2 lineOneA, Vector2 lineOneB, Vector2 lineTwoA, Vector2 lineTwoB) 
    { return (((lineTwoB.y - lineOneA.y) * (lineTwoA.x - lineOneA.x) > (lineTwoA.y - lineOneA.y) * (lineTwoB.x - lineOneA.x)) != ((lineTwoB.y - lineOneB.y) * (lineTwoA.x - lineOneB.x) > (lineTwoA.y - lineOneB.y) * (lineTwoB.x - lineOneB.x)) && ((lineTwoA.y - lineOneA.y) * (lineOneB.x - lineOneA.x) > (lineOneB.y - lineOneA.y) * (lineTwoA.x - lineOneA.x)) != ((lineTwoB.y - lineOneA.y) * (lineOneB.x - lineOneA.x) > (lineOneB.y - lineOneA.y) * (lineTwoB.x - lineOneA.x))); }
}
