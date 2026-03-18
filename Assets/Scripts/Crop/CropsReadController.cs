using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropsReadController : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] List<CropData> cropDatas;
    [SerializeField] CropsManager cropsManager; // Thêm reference đến CropsManager
    Dictionary<TileBase, CropData> cropsFromTiles;

    private void Start()
    {
        cropsFromTiles = new Dictionary<TileBase, CropData>();

        foreach (CropData cropData in cropDatas)
        {
            foreach (TileBase tile in cropData.tiles)
            {
                cropsFromTiles.Add(tile, cropData);
            }
        }
    }

    public Vector3Int GetGridPosition(Vector2 position, bool mousePosition)
    {
        Vector3 worldPosition;

        if (mousePosition)
        {
            worldPosition = Camera.main.ScreenToWorldPoint(position);
        }
        else
        {
            worldPosition = position;
        }

        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);

        return gridPosition;
    }

    public TileBase GetTileBase(Vector3Int gridPosition)
    {
        TileBase tile = tilemap.GetTile(gridPosition);

        return tile;
    }

    public CropData GetCropData(TileBase tilebase)
    {
        return cropsFromTiles[tilebase];
    }
    
    // Method mới để kiểm tra có object cây tại vị trí không
    public bool HasCropObject(Vector3Int gridPosition)
    {
        if (cropsManager != null && cropsManager.cropObjects != null)
        {
            return cropsManager.cropObjects.ContainsKey(gridPosition);
        }
        return false;
    }
    
    // Method mới để lấy object cây tại vị trí
    public GameObject GetCropObject(Vector3Int gridPosition)
    {
        if (cropsManager != null && cropsManager.cropObjects != null)
        {
            if (cropsManager.cropObjects.ContainsKey(gridPosition))
            {
                return cropsManager.cropObjects[gridPosition];
            }
        }
        return null;
    }
    
    // Method mới để kiểm tra xem có thể đặt cây tại vị trí này không
    public bool CanPlaceCrop(Vector3Int gridPosition, Crop cropData)
    {
        // Kiểm tra xem đã có cây nào tại vị trí này chưa
        if (HasCropObject(gridPosition))
        {
            return false;
        }
        
        // Kiểm tra xem đất có phù hợp không
        TileBase groundTile = GetTileBase(gridPosition);
        if (groundTile == null || (!groundTile.name.Contains("Plowed") && !groundTile.name.Contains("Water")))
        {
            return false;
        }
        
        // Nếu cây lớn hơn 1x1, kiểm tra các ô lân cận
        if (cropData.cropSize.x > 1 || cropData.cropSize.y > 1)
        {
            for (int x = 0; x < cropData.cropSize.x; x++)
            {
                for (int y = 0; y < cropData.cropSize.y; y++)
                {
                    if (x == 0 && y == 0) continue; // Bỏ qua vị trí chính đã kiểm tra
                    
                    Vector3Int checkPos = new Vector3Int(gridPosition.x + x, gridPosition.y + y, gridPosition.z);
                    
                    // Kiểm tra có object cây nào tại vị trí lân cận không
                    if (HasCropObject(checkPos))
                    {
                        return false;
                    }
                    
                    // Kiểm tra đất tại vị trí lân cận
                    TileBase checkTile = GetTileBase(checkPos);
                    if (checkTile == null || (!checkTile.name.Contains("Plowed") && !checkTile.name.Contains("Water")))
                    {
                        return false;
                    }
                }
            }
        }
        
        return true;
    }
}


