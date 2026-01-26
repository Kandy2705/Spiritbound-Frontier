using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarUI : MonoBehaviour
{
    [SerializeField] private List<SlotUI> toolbarSlots = new List<SlotUI>();

    private Dictionary<KeyCode, System.Action> selectableSlots = new Dictionary<KeyCode, System.Action>();

    private SlotUI selectedSlot;

    private void Start()
    {
        SelectSlot(0);

        int currentSlot = 0;
        for (int keyCodeValue = 49; keyCodeValue <= 57; keyCodeValue++)
        {
            int preventFunctionCall = currentSlot;

            selectableSlots.Add((KeyCode)keyCodeValue, () => SelectSlot(preventFunctionCall));
            currentSlot++;
        }

        selectableSlots.Add(KeyCode.Alpha0, () => SelectSlot(currentSlot));
    }

    private void Update()
    {
        foreach (KeyValuePair<KeyCode, System.Action> numericKey in selectableSlots)
        {
            if (!Input.GetKeyDown(numericKey.Key) || !selectableSlots.ContainsKey(numericKey.Key))
            {
                continue;
            }

            selectableSlots[numericKey.Key].Invoke();
        }
    }

    public void SelectSlot(SlotUI slot)
    {
        SelectSlot(slot.slotId);
    }

    public void SelectSlot(int slot)
    {
        if (toolbarSlots.Count != 10)
        {
            return;
        }

        selectedSlot?.SetHighlight(false);

        selectedSlot = toolbarSlots[slot];
        selectedSlot.SetHighlight(true);

        GameManager.instance.player.inventory.toolbar.SelectSlot(slot);
    }
}
