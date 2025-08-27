using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineOfSight : MonoBehaviour
{
    public float fovCone;
    public float sightRange;

    public LayerMask layerMask;
    public bool enableLaser = true;

    private LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layerMask = LayerMask.GetMask(new string[] { "Ground", "Default" });
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableLaser)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, sightRange, layerMask);
            Vector3 sightPoint;
            if (hit.collider != null)
            {
                sightPoint = hit.point;
            }
            else
            {
                sightPoint = transform.position + transform.right * sightRange;
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, sightPoint);
        }

    }

    public bool CanSeeGO(GameObject go, float range)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, go.transform.position - transform.position, range, layerMask);
        // Debug.DrawLine(transform.position, hit.point, Color.blue, 5f);
        // Debug.Log($"[LineOfSight.CanSeeGO] trying to look at {go} at range {range}; hit.position is {hit.point}");
        if (hit.collider != null && hit.collider.gameObject.Equals(go))
        {
            return true;
        }
        return false;
    }

    public static bool CanSeeGO(Transform location, GameObject go, float range, LayerMask lm)
    {
        Vector2 dir = (go.transform.position - location.position).normalized;
        // ignore own 
        LayerMask finalMask = lm | (1 << location.gameObject.layer);
        RaycastHit2D hit = Physics2D.Raycast(location.position, dir, range, finalMask);
        Debug.DrawRay(location.position, dir * range, Color.red, 5f);
        if (hit.collider != null)
        {
            Debug.Log($"[LineOfSight.CanSeeGO] hit {hit.collider.gameObject}!");
            Debug.DrawLine(location.position, hit.point, Color.blue, 5f);
        }
        // Debug.DrawLine(location.position, go.transform.position, Color.red, 5f);
        if (hit.collider != null && hit.collider.gameObject.Equals(go))
        {
            return true;
        }
        return false;
    }

    public float DistanceUntilObject(Vector2 start, Vector2 dir, float range = Mathf.Infinity)
    {
        RaycastHit2D hit = Physics2D.Raycast(start, dir, range);
        if (hit.collider != null)
        {
            return Vector2.Distance(start, hit.point);
        }
        return Mathf.Infinity;
    }



}
