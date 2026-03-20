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
        
        spriteController = GetComponent<CropSpriteController>();
        if (spriteController == null)
        {
            return;
        }
        
        Vector3 worldPos = tilemap.CellToWorld(position) + tilemap.cellSize * 0.5f;
        worldPos.z = 0f;
        transform.position = worldPos;
        
        Debug.Log($"CropObject positioned at {worldPos}");
        
        spriteController.SetBasePosition(worldPos);
        
        spriteController.SetCropData(
            crop.stateSprites,
            crop.stateScales, 
            crop.stateOffsets,
            crop.cropSize,
            crop.spriteOffset
        );
        
        spriteController.Initialize(0);
        
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingOrder = -position.y + 20;
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
        int lastState = cropData.stateSprites.Length - 1;
        return currentState >= lastState;
    }
    
    public bool CanPlaceAt(Vector3Int position, Tilemap tilemap)
    {
        for (int x = 0; x < cropData.cropSize.x; x++)
        {
            for (int y = 0; y < cropData.cropSize.y; y++)
            {
                Vector3Int checkPos = new Vector3Int(position.x + x, position.y + y, position.z);
                TileBase tile = tilemap.GetTile(checkPos);
                
                if (tile != null && !tile.name.Contains("Plowed") && !tile.name.Contains("Water"))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
