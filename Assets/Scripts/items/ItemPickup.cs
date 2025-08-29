using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public Items itemDrop;

    private bool playerEntered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        item = AssignItem(itemDrop);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && !playerEntered)
        {
            playerEntered = true; // account for 2 colliders

            PlayerStats player = other.GetComponent<PlayerStats>();
            AddItem(player);

            Destroy(this.gameObject);
        }
    }

    public void AddItem(PlayerStats player)
    {
        foreach (PlayerItemInventory i in player.inventory)
        {
            if (i.name == item.GiveName())
            {
                i.stacks += 1;
                Debug.Log($"Item {i.name} found. New stack count: {i.stacks}");
                return;
            }
        }
        player.inventory.Add(new PlayerItemInventory(item, item.GiveName(), 1));
    }

    public Item AssignItem(Items itemToAssign)
    {
        switch (itemToAssign)
        {
            case Items.HealingItem:
                return new HealingItem();
            case Items.DefenseItem:
                return new DefenseItem();
            default:
                return new HealingItem();
        }
    }
}

public enum Items 
{ 
    HealingItem,
    DefenseItem
}
