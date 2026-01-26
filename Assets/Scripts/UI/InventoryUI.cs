using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public string inventoryName;
    public List<SlotUI> slots = new List<SlotUI>();

    [SerializeField] private Canvas canvas;

    private Inventory inventory;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventory.GetInventoryByName(inventoryName);

        SetupSlots();
        Refresh();
    }

    public void Refresh()
    {
        if (slots.Count == inventory.slots.Count)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (inventory.slots[i].itemName != string.Empty)
                {
                    slots[i].SetItem(inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmtpy();
                }
            }
        }
    }

    public void Remove()
    {
        Item item = GameManager.instance.itemManager.GetItemByName(inventory.slots[UIManager.draggedSlot.slotId].itemName);

        if (item != null)
        {
            if (UIManager.dragSingle)
            {
                GameManager.instance.player.DropItem(item);
                inventory.Remove(UIManager.draggedSlot.slotId);
            }
            else
            {
                int amount = inventory.slots[UIManager.draggedSlot.slotId].count;

                GameManager.instance.player.DropItem(item, amount);
                inventory.Remove(UIManager.draggedSlot.slotId, amount);
            }

            Refresh();
        }

        UIManager.draggedSlot = null;
    }

    public void SlotBeginDrag(SlotUI slot)
    {
        UIManager.draggedSlot = slot;
        UIManager.draggedIcon = Instantiate(UIManager.draggedSlot.itemIcon);
        UIManager.draggedIcon.transform.SetParent(canvas.transform);
        UIManager.draggedIcon.raycastTarget = false;
        UIManager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(UIManager.draggedIcon.gameObject);
    }

    public void SlotDrag()
    {
        MoveToMousePosition(UIManager.draggedIcon.gameObject);
    }

    public void SlotEndDrag()
    {
        Destroy(UIManager.draggedIcon.gameObject);
        UIManager.draggedIcon = null;
    }

    public void SlotDrop(SlotUI slot)
    {
        if (UIManager.dragSingle)
        {
            UIManager.draggedSlot.inventory.MoveSlot(UIManager.draggedSlot.slotId, slot.slotId, slot.inventory);

        }
        else
        {
            var amount = UIManager.draggedSlot.inventory.slots[UIManager.draggedSlot.slotId].count;
            UIManager.draggedSlot.inventory.MoveSlot(
                UIManager.draggedSlot.slotId,
                slot.slotId,
                slot.inventory,
                amount
            );
        }
        GameManager.instance.uiManager.RefreshAll();
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                null,
                out Vector2 position
            );

            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    private void SetupSlots()
    {
        int counter = 0;

        foreach (var slot in slots)
        {
            slot.slotId = counter;
            counter++;
            slot.inventory = inventory;
        }
    }
}
