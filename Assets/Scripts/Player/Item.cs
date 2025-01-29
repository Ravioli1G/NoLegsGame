using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public abstract string GiveName();

    public virtual void Update(PlayerStats player, int stacks){      }

    public virtual void OnHit(PlayerStats player, Enemy enemy, int stacks){     }
}

public class HealingItem : Item 
{
    public override string GiveName()
    {
        return "Healing";
    }

    public override void Update(PlayerStats player, int stacks)
    {
        player.health += 3 + (2 * stacks);
    }
}

public class DefenseItem : Item 
{
    public override string GiveName()
    {
        return "Defense";
    }

    public override void OnHit(PlayerStats player, Enemy enemy, int stacks)
    {
        // add arguments
    }
}