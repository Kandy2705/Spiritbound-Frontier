using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropSpriteController : MonoBehaviour
{
    [Header("Crop Sprites")]
    [SerializeField] private Sprite[] stateSprites; // Mảng chứa sprite cho từng state
    [SerializeField] private Vector2[] stateScales; // Mảng chứa scale cho từng state
    [SerializeField] private Vector2[] stateOffsets; // Mảng chứa offset cho từng state
    
    [Header("Crop Settings")]
    [SerializeField] private Vector2 cropSize = Vector2.one;
    [SerializeField] private Vector2 spriteOffset = Vector2.zero;
    
    private SpriteRenderer spriteRenderer;
    private int currentState = 0;
    private Vector3 basePosition; // Vị trí gốc của tile (không có offset)
    
    private void Awake()
    {
        Debug.Log("CropSpriteController Awake() called");
        basePosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer not found in CropSpriteController!");
        else
            Debug.Log("SpriteRenderer found successfully");
        
        // Khởi tạo mảng nếu chưa có
        if (stateSprites == null || stateSprites.Length == 0)
            stateSprites = new Sprite[6];
            
        if (stateScales == null || stateScales.Length == 0)
            stateScales = new Vector2[6];
            
        if (stateOffsets == null || stateOffsets.Length == 0)
            stateOffsets = new Vector2[6];
            
        // Set default scale và offset
        for (int i = 0; i < 6; i++)
        {
            if (stateScales[i] == Vector2.zero)
                stateScales[i] = Vector2.one;
                
            if (stateOffsets[i] == Vector2.zero)
                stateOffsets[i] = spriteOffset;
        }
        
        Debug.Log("CropSpriteController initialized successfully");
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
        
        Debug.Log($"Current state: {currentState}, StateSprites length: {stateSprites?.Length}");
        
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
        
        // Cập nhật sprite
        spriteRenderer.sprite = stateSprites[currentState];
        Debug.Log($"Set sprite to: {stateSprites[currentState].name}");
        
        // Cập nhật scale
        if (currentState < stateScales.Length)
        {
            transform.localScale = new Vector3(stateScales[currentState].x, stateScales[currentState].y, 1f);
        }
        
        // Cập nhật offset: luôn tính từ basePosition để tránh tích lũy offset
        if (currentState < stateOffsets.Length)
        {
            transform.position = new Vector3(
                basePosition.x + stateOffsets[currentState].x,
                basePosition.y + stateOffsets[currentState].y,
                basePosition.z
            );
        }
    }
    
    // Getters để các script khác có thể truy cập
    public int GetCurrentState() => currentState;
    public Sprite GetCurrentSprite() => currentState < stateSprites.Length ? stateSprites[currentState] : null;
    public Vector2 GetCropSize() => cropSize;
    public Vector2 GetSpriteOffset() => spriteOffset;
    
    // Setters để cấu hình từ ngoài
    public void SetCropData(Sprite[] sprites, Vector2[] scales, Vector2[] offsets, Vector2 size, Vector2 offset)
    {
        stateSprites = sprites;
        stateScales = scales;
        stateOffsets = offsets;
        cropSize = size;
        spriteOffset = offset;
        
        // Không gọi UpdateSprite() ở đây vì basePosition chưa được set đúng
        // Initialize() sẽ được gọi sau khi CropObject.Initialize() set transform.position
    }
    
    // Gọi sau khi transform.position đã được set đúng
    public void SetBasePosition(Vector3 position)
    {
        basePosition = position;
    }
}
