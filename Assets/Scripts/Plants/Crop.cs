using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Globalization;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/Crop")]

public class Crop : ScriptableObject
{
    [Header("Crop Sprites")]
    public Sprite[] stateSprites; 
    public Vector2[] stateScales;
    public Vector2[] stateOffsets;

    [Header("Crop Settings")]
    public Vector2 cropSize = Vector2.one;
    public Vector2 spriteOffset = Vector2.zero;

    // public SpriteRenderer currentRenderer;
    public SpriteRenderer state; 
    public Vector3Int position;
    public GameObject cropObject;

    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public bool planted = false;
    public Text timeText;
    public string Name;
    
    public Sprite GetStateSprite(int stateIndex)
    {
        return stateIndex >= 0 && stateIndex < stateSprites.Length ? stateSprites[stateIndex] : null;
    }
    
    public Vector2 GetStateScale(int stateIndex)
    {
        return stateIndex >= 0 && stateIndex < stateScales.Length ? stateScales[stateIndex] : Vector2.one;
    }
    
    public Vector2 GetStateOffset(int stateIndex)
    {
        return stateIndex >= 0 && stateIndex < stateOffsets.Length ? stateOffsets[stateIndex] : spriteOffset;
    }
}