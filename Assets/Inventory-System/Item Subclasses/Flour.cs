using UnityEngine;

[CreateAssetMenu(fileName = "Flour", menuName = "Scriptable Objects/Flour")]
public class Flour : Item
{
    [SerializeField] GameObject flourPrefab;
    public Transform launchPoint;

    public override void Use()
    {
        Vector2 location = new();
        if (launchPoint != null)
        {
            location = launchPoint.position;
        }
        Launch(location);
    }

    protected virtual void Launch(Vector2 location)
    {
        GameObject proj = Instantiate(flourPrefab, location, Quaternion.identity);

    }
}
