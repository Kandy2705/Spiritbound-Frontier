using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;
    private TileManager tileManager;
    private Animator animator;

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
    }

    public void DropItem(Item item)
    {
        Vector2 spawnLocation = transform.position;

        Vector2 spawnOffset = Random.insideUnitCircle + new Vector2(1f, 1.5f);

        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        droppedItem.rb2d.AddForce(spawnOffset * 2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DropItem(item);
        }
    }
}
