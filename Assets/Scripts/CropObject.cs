using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropObject : MonoBehaviour
{
    private CropSpriteController spriteController;
    private Crop cropData;
    
    private Vector3Int gridPosition;
    private Tilemap groundTilemap;
    private int currentState = 0;
    
    public void Initialize(Crop crop, Vector3Int position, Tilemap tilemap)
    {
        cropData = crop;
        gridPosition = position;
        groundTilemap = tilemap;
        
        Debug.Log($"Initializing CropObject at position {position}");
        Debug.Log($"Crop stateSprites length: {crop.stateSprites?.Length}");
        
        // Lấy CropSpriteController component
        spriteController = GetComponent<CropSpriteController>();
        if (spriteController == null)
        {
            Debug.LogError("CropObject không có CropSpriteController component!");
            return;
        }
        
        // Căn chỉnh vị trí TRƯỚC: lấy tâm của cell (CellToWorld trả về góc dưới-trái, cộng thêm nửa cell)
        Vector3 worldPos = tilemap.CellToWorld(position) + tilemap.cellSize * 0.5f;
        worldPos.z = 0f;
        transform.position = worldPos; // Set vị trí gốc (base position)
        
        Debug.Log($"CropObject positioned at {worldPos}");
        
        // Set base position cho CropSpriteController TRƯỚC khi khởi tạo sprite
        spriteController.SetBasePosition(worldPos);
        
        // Khởi tạo với data từ crop (sau khi đã có base position)
        spriteController.SetCropData(
            crop.stateSprites,
            crop.stateScales, 
            crop.stateOffsets,
            crop.cropSize,
            crop.spriteOffset
        );
        
        Debug.Log("CropSpriteController data set successfully");
        
        spriteController.Initialize(0); // Bắt đầu từ state 0 (basePosition đã được set đúng)
        
        // Sắp xếp sorting order để hiển thị đúng chiều sâu
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = -position.y + 20;
            Debug.Log($"SpriteRenderer sorting order set to {position.y}");
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found!");
        }
    }
    
    public void UpdateSprite(int stateIndex)
    {
        if (spriteController != null)
        {
            spriteController.SetState(stateIndex);
            currentState = stateIndex;
        }
    }
    
    public void GrowToNextState()
    {
        if (spriteController != null)
        {
            spriteController.GrowToNextState();
            currentState = spriteController.GetCurrentState();
        }
    }
    
    public Vector3Int GetGridPosition()
    {
        return gridPosition;
    }
    
    public int GetCurrentState()
    {
        return currentState;
    }
    
    public Crop GetCropData()
    {
        return cropData;
    }
    
    public bool IsFullyGrown()
    {
        if (cropData == null || cropData.stateSprites == null || cropData.stateSprites.Length == 0)
            return false;
        // Cây đã trưởng thành khi đạt state cuối cùng (index = Length - 1)
        int lastState = cropData.stateSprites.Length - 1;
        return currentState >= lastState;
    }
    
    // Kiểm tra xem có thể đặt cây tại vị trí này không (dùng cho cây lớn)
    public bool CanPlaceAt(Vector3Int position, Tilemap tilemap)
    {
        // Kiểm tra tất cả các ô mà cây sẽ chiếm
        for (int x = 0; x < cropData.cropSize.x; x++)
        {
            for (int y = 0; y < cropData.cropSize.y; y++)
            {
                Vector3Int checkPos = new Vector3Int(position.x + x, position.y + y, position.z);
                TileBase tile = tilemap.GetTile(checkPos);
                
                // Chỉ cho phép đặt trên đất đã cày hoặc đất đã tưới
                if (tile != null && !tile.name.Contains("Plowed") && !tile.name.Contains("Water"))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
