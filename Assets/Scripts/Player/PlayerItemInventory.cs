using UnityEngine;

[System.Serializable]
public class PlayerItemInventory
{
    public Item item;
    public string name;
    public int stacks;

    public PlayerItemInventory(Item newItem, string newName, int newStacks)
    {
        item = newItem;
        name = newName;
        stacks = newStacks;
    }

    // contiune https://www.youtube.com/watch?v=iU6mKyQjOYI
}
