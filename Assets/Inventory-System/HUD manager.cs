
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class HUDmanager : MonoBehaviour
{
    [Header("Inventory HUD")]
    [SerializeField] GameObject InventoryHUD;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Inventory targetInventory; // the specific inventory to display;
    [SerializeField] private Color unselectedColor;
    [SerializeField] private Color selectedColor;

    [SerializeField] private List<GameObject> inventorySlots = new List<GameObject>();

    void Awake()
    {




    }

    #region Inventory
    public void InitInventoryHUD()
    {

        if (targetInventory != null)
        {
            targetInventory.hUDmanager = this;

        }
        else
        {
            return;
        }
        // add slots
        for (int i = 0; i < targetInventory.Size(); i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, InventoryHUD.transform);
            inventorySlots.Add(newSlot);
        }


    }

    public void UpdateInventoryHUD()
    {
        for (int i = 0; i < targetInventory.Size(); i++)
        {
            UpdateInventoryHUDAtIndex(i);
        }
    }

    public void UpdateInventoryHUDAtIndex(int index)
    {
        // Debug.Log($"[UpdateInventoryHUDAtIndex] targetInventoryExists? {targetInventory != null}");
        // Debug.Log($"[UpdateInventoryHUDAtIndex] stack is supposed to be {targetInventory.ItemAtIndex(index).Item.itemName}");
        ItemStack stack = targetInventory.ItemAtIndex(index);
        InventorySlot slot = inventorySlots[index].GetComponent<InventorySlot>();
        if (stack == null)
        {

            slot.ClearItem();
        }
        else
        {
            slot.SetItem(stack);
        }
    }


    public void SelectItemAtIndex(int index)
    {
        foreach (GameObject slot in inventorySlots)
        {
            Image background = slot.GetComponent<Image>();
            background.color = unselectedColor;
        }
        Image selectedBackground = inventorySlots[index].GetComponent<Image>();
        selectedBackground.color = selectedColor;

    }

    #endregion

}
