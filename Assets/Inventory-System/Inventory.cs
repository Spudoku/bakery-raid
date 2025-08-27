
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool isTarget = false;
    public int inventorySize = 10;
    [SerializeField] private ItemStack[] inventoryList;

    [SerializeField] private ItemStack[] startingItems;

    [Header("Active Item")]
    [SerializeField] private Item activeItem = null;
    private int selectedIndex = 0;
    [SerializeField] private float cooldown;

    public bool initialized = false;

    [Header("Sounds")]
    public AudioSource addItemSource;

    [Header("Debug")]
    [SerializeField] string activeItemName;
    [SerializeField] int selectedIndexDebug;

    public HUDmanager hUDmanager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Awake()
    {

        inventoryList = new ItemStack[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            inventoryList[i] = new ItemStack();
        }
        if (hUDmanager != null)
        {
            hUDmanager.InitInventoryHUD();
        }


        foreach (ItemStack item in startingItems)
        {
            AddItem(item);
        }

        if (hUDmanager != null)
        {
            hUDmanager.UpdateInventoryHUD();
            hUDmanager.SelectItemAtIndex(0);
        }


    }

    void Update()
    {
        cooldown -= Time.deltaTime;
        if (isTarget)
        {
            ChangeSelectedIndex(InputManager.MouseScrollInput);
            if (InputManager.useWasPressed)
            {
                Debug.Log($"[Inventory.Update] trying to use an item!");
                UseActiveItem();
            }
        }
        if (activeItem == null)
        {
            activeItemName = "NULL";
        }
        else
        {
            activeItemName = activeItem.name;
        }

        selectedIndexDebug = selectedIndex;
    }


    public void SelectItemAtIndex(int index)
    {
        activeItem = inventoryList[index].Item;

    }

    // activate the item's Action and remove one instance of the item
    public void UseActiveItem()
    {
        if (activeItem == null || cooldown > 0)
        {
            return;
        }
        if (!activeItem.canBeUsed)
        {
            return;
        }

        activeItem.Use();

        cooldown = activeItem.useSpeed;
        RemoveItem(activeItem, 1);

        if (IndexOf(activeItem) < 0)
        {
            activeItem = null;
        }
    }

    // probably won't be used but oh well
    public void UseItemAtIndex(int index)
    {
        if (index < 0 || index >= inventoryList.Count())
        {
            return;
        }
        ItemStack stack = inventoryList[index];
        stack.Item.Use();
    }

    public void AddItem(ItemStack stack)
    {
        AddItem(stack.Item, stack.Count);
    }

    // Should probably just use ItemStacks but it works :)
    private void AddItem(Item item, int count)
    {
        int index = IndexOf(item);
        // Debug.Log($"[Inventory.AddItem] index of {item.itemName}: {index}");
        // inventory currently does not contain item
        if (index < 0)
        {
            // check capacity
            index = IndexOf(null);
            if (index < 0)
            {
                Debug.Log($"[Inventory.AddItem] inventory is full!");
                return;
            }
            inventoryList[index] = new ItemStack(item, count);
            Debug.Log($"[Inventory.AddItem] added {item.itemName} x {count} to inventory");

            if (activeItem == null)
            {
                SelectItemAtIndex(index);
            }

        }
        else
        {
            inventoryList[index].Add(count);
            Debug.Log($"[Inventory.AddItem] added {item.itemName} x {count} to inventory");
        }
        if (hUDmanager != null)
        {
            hUDmanager.UpdateInventoryHUDAtIndex(index);
        }


    }

    public void RemoveItem(Item item, int count)
    {
        int index = IndexOf(item);

        if (index < 0)
        {
            Debug.Log($"[Inventory.RemoveItem] cannot remove {item.itemName}; none found in inventory");
            return;
        }

        ItemStack stack = inventoryList[index];
        if (stack.Count < count)
        {
            Debug.Log($"[Inventory.RemoveItem] cannot remove {count} of {item.itemName} from inventory!");

            return;
        }



        if (stack.Count == count)
        {
            inventoryList[index] = new ItemStack();
            Debug.Log($"[Inventory.RemoveItem] cleared ");
            hUDmanager.UpdateInventoryHUDAtIndex(index);
            return;
        }
        else
        {
            stack.Remove(count);
        }

        // Debug.Log($"[Inventory.RemoveItem] removed item {item.itemName}x{count} from inventory");
        hUDmanager.UpdateInventoryHUDAtIndex(index);
    }

    public int Size()
    {
        return inventorySize;
    }

    public int IndexOf(Item item)
    {
        // Debug.Log($"[Inventory.IndexOf] looking for {item.itemName}");
        for (int i = 0; i < inventorySize; i++)
        {

            if ((inventoryList[i].Item == null && item == null) || inventoryList[i].Item == item)
            {
                return i;
            }
        }
        return -1;
    }

    public ItemStack ItemAtIndex(int index)
    {
        return inventoryList[index];
    }


    private void ChangeSelectedIndex(float amount)
    {
        if (amount == 0)
        {
            return;
        }
        selectedIndex += (int)amount;
        if (selectedIndex >= inventorySize)
        {
            selectedIndex = 0;
        }
        else if (selectedIndex < 0)
        {
            selectedIndex = inventorySize - 1;
        }
        // Debug.Log($"[Inventory.ChangeSelectedIndex] trying to access index {selectedIndex}");
        activeItem = inventoryList[selectedIndex].Item;

        hUDmanager.SelectItemAtIndex(selectedIndex);
    }

    public bool HasCapacityForItem(Item item)
    {
        return IndexOf(item) >= 0 || IndexOf(null) >= 0;
    }
}
