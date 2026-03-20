using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSpriteController : MonoBehaviour
{
    [Header("Crop Sprites")]
    [SerializeField] private Sprite[] stateSprites;
    [SerializeField] private Vector2[] stateScales;
    [SerializeField] private Vector2[] stateOffsets;
    
    [Header("Crop Settings")]
    [SerializeField] private Vector2 cropSize = Vector2.one;
    [SerializeField] private Vector2 spriteOffset = Vector2.zero;
    
    private SpriteRenderer spriteRenderer;
    private int currentState = 0;
    private Vector3 basePosition;
    
    private void Awake()
    {
        basePosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer not found in CropSpriteController!");
        
        if (stateSprites == null || stateSprites.Length == 0)
            stateSprites = new Sprite[6];
            
        if (stateScales == null || stateScales.Length == 0)
            stateScales = new Vector2[6];
            
        if (stateOffsets == null || stateOffsets.Length == 0)
            stateOffsets = new Vector2[6];
            
        for (int i = 0; i < 6; i++)
        {
            if (stateScales[i] == Vector2.zero)
                stateScales[i] = Vector2.one;
                
            if (stateOffsets[i] == Vector2.zero)
                stateOffsets[i] = spriteOffset;
        }
    }
    
    public void Initialize(int startState = 0)
    {
        currentState = startState;
        UpdateSprite();
    }
    
    public void SetState(int state)
    {
        if (state >= 0 && state < stateSprites.Length && state != currentState)
        {
            currentState = state;
            UpdateSprite();
        }
    }
    
    public void GrowToNextState()
    {
        if (currentState < stateSprites.Length - 1)
        {
            currentState++;
            UpdateSprite();
        }
    }
    
    private void UpdateSprite()
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is null!");
            return;
        }
        
        if (stateSprites == null || stateSprites.Length == 0)
        {
            Debug.LogError("StateSprites array is null or empty!");
            return;
        }
        
        if (currentState >= stateSprites.Length)
        {
            Debug.LogError($"Current state {currentState} is out of range! Array length: {stateSprites.Length}");
            return;
        }
        
        if (stateSprites[currentState] == null)
        {
            Debug.LogError($"StateSprites[{currentState}] is null!");
            return;
        }
        
        spriteRenderer.sprite = stateSprites[currentState];
        
        if (currentState < stateScales.Length)
        {
            transform.localScale = new Vector3(stateScales[currentState].x, stateScales[currentState].y, 1f);
        }
        
        if (currentState < stateOffsets.Length)
        {
            transform.position = new Vector3(
                basePosition.x + stateOffsets[currentState].x,
                basePosition.y + stateOffsets[currentState].y,
                basePosition.z
            );
        }
    }
    
    public int GetCurrentState() => currentState;
    public Sprite GetCurrentSprite() => currentState < stateSprites.Length ? stateSprites[currentState] : null;
    public Vector2 GetCropSize() => cropSize;
    public Vector2 GetSpriteOffset() => spriteOffset;
    
    public void SetCropData(Sprite[] sprites, Vector2[] scales, Vector2[] offsets, Vector2 size, Vector2 offset)
    {
        stateSprites = sprites;
        stateScales = scales;
        stateOffsets = offsets;
        cropSize = size;
        spriteOffset = offset;
    }
    
    public void SetBasePosition(Vector3 position)
    {
        basePosition = position;
    }
}
