using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Data/Crop")]

public class Crop : ItemInfo
{
    [Header("Crop Sprites")]
    public Sprite[] stateSprites; 
    public Vector2[] stateScales;
    public Vector2[] stateOffsets;

    [Header("Handbook / Economy")]
    [SerializeField] private int buyPrice;
    [SerializeField] private int sellPrice;
    [SerializeField] private float[] stateDurations;

    [Header("Crop Settings")]
    public Vector2 cropSize = Vector2.one;
    public Vector2 spriteOffset = Vector2.zero;

    // public SpriteRenderer currentRenderer;
    public Sprite state; 
    public Vector3Int position;
    public GameObject cropObject;

    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public bool planted = false;
    public Text timeText;
    public string Name;

    public string ItemName => string.IsNullOrWhiteSpace(Name) ? GetFallbackName() : Name;
    public Sprite HandbookIcon => state != null ? state : (icon != null ? icon : GetFallbackIcon());
    public int BuyPrice => buyPrice > 0 ? buyPrice : GetDefaultBuyPrice();
    public int SellPrice => sellPrice > 0 ? sellPrice : GetDefaultSellPrice();
    public int TransitionCount => Mathf.Max((stateSprites?.Length ?? 0) - 1, 0);
    
    public Sprite GetStateSprite(int stateIndex)
    {
        return stateSprites != null && stateIndex >= 0 && stateIndex < stateSprites.Length ? stateSprites[stateIndex] : null;
    }
    
    public Vector2 GetStateScale(int stateIndex)
    {
        return stateScales != null && stateIndex >= 0 && stateIndex < stateScales.Length ? stateScales[stateIndex] : Vector2.one;
    }
    
    public Vector2 GetStateOffset(int stateIndex)
    {
        return stateOffsets != null && stateIndex >= 0 && stateIndex < stateOffsets.Length ? stateOffsets[stateIndex] : spriteOffset;
    }

    public float GetStageDuration(int stateIndex)
    {
        if (stateIndex < 0 || stateIndex >= TransitionCount)
            return 0f;

        if (stateDurations != null && stateIndex < stateDurations.Length && stateDurations[stateIndex] > 0f)
            return stateDurations[stateIndex];

        return GetDefaultStageDuration();
    }

    public float[] GetAllStageDurations()
    {
        float[] durations = new float[TransitionCount];
        for (int i = 0; i < durations.Length; i++)
        {
            durations[i] = GetStageDuration(i);
        }

        return durations;
    }

    public bool HasNextState(int currentState)
    {
        return currentState >= 0 && currentState < TransitionCount;
    }

    string GetNormalizedKey()
    {
        string rawName = string.IsNullOrWhiteSpace(Name) ? GetFallbackName() : Name;
        return rawName.Replace("(Clone)", string.Empty).Trim().ToLowerInvariant();
    }

    string GetFallbackName()
    {
        return name.Replace("(Clone)", string.Empty).Trim();
    }

    Sprite GetFallbackIcon()
    {
        if (stateSprites == null || stateSprites.Length == 0)
            return null;

        return stateSprites[stateSprites.Length - 1] ?? stateSprites[0];
    }

    float GetDefaultStageDuration()
    {
        switch (GetNormalizedKey())
        {
            case "corn":
                return 120f;
            case "parsley":
            case "tomato":
                return 60f;
            case "potato":
            case "strawberry":
                return 90f;
            default:
                return 60f;
        }
    }

    int GetDefaultBuyPrice()
    {
        switch (GetNormalizedKey())
        {
            case "corn":
                return 100;
            case "parsley":
                return 30;
            case "potato":
                return 110;
            case "strawberry":
                return 150;
            case "tomato":
                return 60;
            default:
                return 0;
        }
    }

    int GetDefaultSellPrice()
    {
        switch (GetNormalizedKey())
        {
            case "corn":
                return 130;
            case "parsley":
                return 39;
            case "potato":
                return 143;
            case "strawberry":
                return 195;
            case "tomato":
                return 78;
            default:
                return 20;
        }
    }
}
