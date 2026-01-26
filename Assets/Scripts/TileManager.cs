using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile tilledTile;
    [SerializeField] private Tile seededTile;

    // void Start()
    // {
    //     foreach (var position in interactableMap.cellBounds.allPositionsWithin)
    //     {
    //         TileBase tile = interactableMap.GetTile(position);

    //         if (tile != null && tile.name == "Interactable_Visible")
    //         {
    //             interactableMap.SetTile(position, hiddenInteractableTile);
    //         }
    //     }
    // }

    // public void Plow(Vector3Int position)
    // {
    //     interactableMap.SetTile(position, plowedTile);
    // }

    public void SetCropTile(Vector3Int position)
    {
        interactableMap.SetTile(position, seededTile);
    }

    public void SetTilledTile(Vector3Int position)
    {
        interactableMap.SetTile(position, tilledTile);
    }

    public string GetTileName(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if (tile == null)
        {
            return string.Empty;
        }

        return tile.name;
    }
}
