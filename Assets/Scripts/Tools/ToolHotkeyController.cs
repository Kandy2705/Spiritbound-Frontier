using UnityEngine;

public class ToolHotkeyController : MonoBehaviour
{
    private ToolbarController toolbarController;
    private InventoryController inventoryController;
    [SerializeField] private ItemToolbarPanel itemToolbarPanel;

    void Start()
    {
        toolbarController = GetComponent<ToolbarController>();
        inventoryController = GetComponent<InventoryController>();
    }

    void Update()
    {
        if (Time.timeScale == 0 || inventoryController.isOpen)
            return;

        // Z - Cây cuốc (Hoe)
        if (Input.GetKeyDown(KeyCode.X))
        {
            SelectToolByName("Hoe");
        }
        // X - Cây xẻng (Shovel)
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            SelectToolByName("Shovel");
        }
        // C - Bình tưới (WateringCan)
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SelectToolByName("WateringCan");
        }
        // V - Bao thu hoạch (Bag)
        else if (Input.GetKeyDown(KeyCode.V))
        {
            SelectToolByName("Bag");
        }
    }

    private void SelectToolByName(string toolName)
    {
        for (int i = 0; i < GameManager.instance.inventoryContainer.slots.Count; i++)
        {
            var slot = GameManager.instance.inventoryContainer.slots[i];
            if (slot.item != null && slot.item.Name == toolName)
            {
                toolbarController.Set(i);
                
                // Cập nhật highlight trên toolbar
                if (itemToolbarPanel != null)
                {
                    itemToolbarPanel.Highlight(i);
                }
                
                Debug.Log("Đã chọn công cụ: " + toolName + " tại slot " + i);
                return;
            }
        }
        Debug.Log("Không tìm thấy công cụ: " + toolName);
    }
}
