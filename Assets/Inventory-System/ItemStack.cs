using System;

[System.Serializable]
public class ItemStack
{
    public Item Item;
    public int Count;

    public ItemStack()
    {
        Item = null;
        Count = 0;
    }

    public ItemStack(Item i, int n)
    {
        Item = i;
        Count = n;
    }

    public void Add(int n)
    {
        Count += Math.Abs(n);
    }

    public void Remove(int n)
    {
        Count -= Math.Abs(n);
    }

}
