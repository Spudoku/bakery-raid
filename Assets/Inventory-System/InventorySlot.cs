using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;


// visual container for inventory
public class InventorySlot : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private Image spriteImage;
    [SerializeField] private TMP_Text countText;

    [Header("Other")]
    [SerializeField] private Sprite placeHolder;

    ItemStack itemStack;

    void Awake()
    {
        ClearItem();
    }

    public void SetItem(ItemStack stack)
    {
        itemStack = stack;
        if (itemStack.Item == null)
        {
            spriteImage.sprite = placeHolder;
        }
        else
        {
            spriteImage.sprite = itemStack.Item.sprite;
        }

        UpdateCount();
    }

    public void ClearItem()
    {
        spriteImage.sprite = placeHolder;
        UpdateCount();
    }

    public void UpdateCount()
    {

        if (itemStack == null || itemStack.Count == 0)
        {
            countText.text = "";
        }
        else if (itemStack.Count > 100000)
        {
            countText.text = $"∞";
        }
        else
        {
            countText.text = $"{itemStack.Count}";
        }
    }
}
