using UnityEngine;
// Interface for all items


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Statistics")]
    public string itemName;
    public float useSpeed = 1f;      // the interval this item can be used in sequence

    [Header("Cosmetics")]
    public Sprite sprite;

    public bool canBeUsed = false;
    public bool isConsumable;   // if true, use immediately after being picked up

    public virtual void Use()
    {


    }

    public virtual void Use(Vector2 location, Quaternion rotation)
    {

    }

}
