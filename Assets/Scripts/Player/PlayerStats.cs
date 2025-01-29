using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public List<PlayerItemInventory> inventory = new List<PlayerItemInventory>();
    public float tick = 1;

    [Header("Integer stats")]
    public int health;
    public int jumpAmount;

    [Header("Float stats")]
    public float maxSpeed;

    [Header("Boolean stats")]
    public bool hasLegs;

    private void Start()
    {
        StartCoroutine(CallItemUpdate());
    }

    private void Update()
    {

    }

    IEnumerator CallItemUpdate() 
    { 
        foreach (PlayerItemInventory i in inventory)
        {
            i.item.Update(this, i.stacks);
        }
        yield return new WaitForSeconds(tick);
        StartCoroutine(CallItemUpdate());
    }

    public void CallItemOnHit(Enemy enemy) 
    { 
        foreach (PlayerItemInventory i in inventory) 
        { 
            i.item.OnHit(this, enemy, i.stacks);
        }
    }
}
