using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farming : MonoBehaviour
{
    private TileManager tileManager;

    private Player player;

    [SerializeField] private List<GameObject> cropPrefabs;
    private List<Crop> crops;

    void Start()
    {
        tileManager = GameManager.instance.GetComponent<TileManager>();
        player = GameManager.instance.player;
        crops = new List<Crop>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (tileManager == null)
            {
                tileManager = GameManager.instance.GetComponent<TileManager>();
            }

            if (player == null)
            {
                player = GameManager.instance.player;
            }

            Vector3Int playerPosition = new Vector3Int((int)player.transform.position.x, (int)player.transform.position.y, 0);
            Inventory.Slot selectedSlot = player.inventory.toolbar.selectedSlot;

            if (CanPlant(playerPosition))
            {
                if (selectedSlot.itemName == "Wheat Seeds")
                {
                    Plant(playerPosition, selectedSlot.itemName);
                    player.inventory.toolbar.selectedSlot.RemoveItem();
                    GameManager.instance.uiManager.RefreshInventoryUI("Toolbar");
                }
            }
            else if (CanHarvest(playerPosition))
            {
                if (CanHarvest(playerPosition))
                {
                    Harvest(playerPosition);
                }
            }
        }
    }

    public bool CanPlant(Vector3Int position)
    {
        string tile = tileManager.GetTileName(position);
        Debug.Log(tile);

        if (string.IsNullOrEmpty(tile) || !tile.Contains("Tilled"))
        {
            return false;
        }

        return true;
    }

    public void Plant(Vector3Int position, string seedName)
    {
        tileManager.SetCropTile(position);
        Crop plantedCrop = Instantiate(cropPrefabs[0], position, Quaternion.identity).GetComponent<Crop>();
        plantedCrop.OnPlant();
        crops.Add(plantedCrop);
    }

    public bool CanHarvest(Vector3Int position)
    {
        string tileName = tileManager.GetTileName(position);

        if (string.IsNullOrEmpty(tileName) || !tileName.Contains("Crop"))
        {
            return false;
        }
        else if (!HasCrop(position))
        {
            return false;
        }

        Crop crop = GetByPosition(position);
        return crop.CanHarvest();
    }

    public void Harvest(Vector3Int position)
    {
        tileManager.SetTilledTile(position);
        Crop crop = GetByPosition(position);

        crop.OnHarvest();
        crops.Remove(crop);
    }

    public bool HasCrop(Vector3Int position)
    {
        foreach (var crop in crops)
        {
            if (crop.transform.position == position)
            {
                return true;
            }
        }

        return false;
    }

    private Crop GetByPosition(Vector3Int position)
    {
        foreach (var crop in crops)
        {
            if (crop.transform.position == position)
            {
                return crop;
            }
        }

        return null;
    }
}
