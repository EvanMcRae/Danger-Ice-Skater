using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoleCutter : MonoBehaviour
{
    [SerializeField] private GameObject HolePrefab;
    public static List<GameObject> Holes = new();

    [SerializeField] private GameObject CutoutPrefab;
    public static List<GameObject> Cutouts = new();

    public List<Vector2> Points = new();

    public bool isTouchingGround = false;
    public bool isDrawing = false;
    public bool hasOverlapped = false;

    public const float LOOP_ALLOWANCE = 1f;
    public const int MIN_POINTS = 8;
    public const float RESOLUTION = 0.1f;
    public const int MAX_POINTS = 200;
    public const int MIN_POINTS_FOR_HOLE = 25;
    private LineRenderer lineRenderer;
    private Vector3 lastPos;

    [SerializeField] private PlayerController player;

    [SerializeField] private float planeHeight;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isTouchingGround)
        {
            isDrawing = true;
            Vector3 pos = player.transform.position;

            if (Vector3.Distance(pos, lastPos) >= RESOLUTION)
            {
                lastPos = pos;
                Vector2 pointToAdd = new(pos.x, pos.z);

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

                    if (Points.Count - intersectPoint <= MIN_POINTS_FOR_HOLE && intersectPoint > 0)
                    {
                        //This removeRange removes loops that are too small from the path
                        //This means you cannot hit the loop again to cut things off, but it prevents an issue
                        //where in a series of loops, finishing a large enough later loop would cut out all too small earlier ones
                        Points.RemoveRange(intersectPoint, Points.Count - intersectPoint);
                        intersectPoint = -1;
                    }

                    List<Vector2> cutoutPoints = new List<Vector2>();

                    
                    if (intersectPoint >= 0)
                    {
                        //Copy segment that creates hole.
                        for (int i = intersectPoint; i < Points.Count; i++)
                        {
                            cutoutPoints.Add(Points[i]);
                        }
                        MakeCutout(cutoutPoints);

                        // Remove points that were included in the segment, minus the intersection point
                        Points.RemoveRange(intersectPoint + 1, Points.Count - intersectPoint - 1);
                    }
                }

                
                
                //Move buffer over. Poor scaling run time (O(n)), but size is capped small enough it is not an issue. 
                if (Points.Count >= MAX_POINTS)
                    Points.RemoveAt(0);
                Points.Add(pointToAdd);

                lineRenderer.positionCount = Points.Count;
                for (int i = 0; i < lineRenderer.positionCount; i++)
                {
                    lineRenderer.SetPosition(i, new Vector3(Points[i].x, planeHeight, Points[i].y));
                }
                
            }
        }

        if (isDrawing && !player.isTouchingGround)
        {
            // Clear points array for next time
            Points = new();
            lineRenderer.positionCount = 0;
            isDrawing = false;
            hasOverlapped = false;
        }

        // TODO DEBUG - remove
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            foreach (GameObject _ in Holes)
            {
                Destroy(_);
            }
        }
    }

    void MakeCutout(List<Vector2> points)
    {
        // Spawn new hole, converting drawn points into a PolygonCollider2D and then into a mesh
        GameObject newHole = Instantiate(HolePrefab, transform.position, Quaternion.identity);
        GameObject newCutout = Instantiate(CutoutPrefab, transform.position, Quaternion.identity);

        PolygonCollider2D poly = newHole.GetComponentInChildren<PolygonCollider2D>();
        poly.points = points.ToArray();
        Mesh mesh = poly.CreateMesh(false, false);
        if (mesh == null)
        {
            Debug.LogError("Null hole mesh, did you draw out of bounds?");
        }
        poly.enabled = false;

        PopulateMesh(newHole, mesh, false);
        PopulateMesh(newCutout, mesh, true);
        
        Holes.Add(newHole);
        Cutouts.Add(newCutout);
    }

    void PopulateMesh(GameObject newObject, Mesh mesh, bool isCutout)
    {
        MeshFilter meshFilter = newObject.GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshCollider meshCollider = newObject.GetComponentInChildren<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        if (isCutout && mesh != null)
        {
            // Calculate normals
            mesh.RecalculateNormals();
        }

        if (mesh == null)
        {
            Debug.LogError("Null hole mesh, did you draw out of bounds?");
        }

        newObject.transform.SetPositionAndRotation(new Vector3(0, planeHeight + (isCutout ? 0 : 0.0001f), 0), Quaternion.Euler(90, 0, 0));
    }

    //From https://www.reddit.com/r/gamedev/comments/7ww4yx/whats_the_easiest_way_to_check_if_two_line/
    public static bool lineSegmentsIntersect(Vector2 lineOneA, Vector2 lineOneB, Vector2 lineTwoA, Vector2 lineTwoB) 
    { return (((lineTwoB.y - lineOneA.y) * (lineTwoA.x - lineOneA.x) > (lineTwoA.y - lineOneA.y) * (lineTwoB.x - lineOneA.x)) != ((lineTwoB.y - lineOneB.y) * (lineTwoA.x - lineOneB.x) > (lineTwoA.y - lineOneB.y) * (lineTwoB.x - lineOneB.x)) && ((lineTwoA.y - lineOneA.y) * (lineOneB.x - lineOneA.x) > (lineOneB.y - lineOneA.y) * (lineTwoA.x - lineOneA.x)) != ((lineTwoB.y - lineOneA.y) * (lineOneB.x - lineOneA.x) > (lineOneB.y - lineOneA.y) * (lineTwoB.x - lineOneA.x))); }
}
