using UnityEngine;

[System.Serializable]
public class HandbookItemData
{
    [SerializeField] private Crop sourceCrop;

    public HandbookItemData()
    {
    }

    public HandbookItemData(Crop crop)
    {
        sourceCrop = crop;
    }

    public Crop SourceCrop => sourceCrop;
    public string itemName => sourceCrop != null ? sourceCrop.ItemName : string.Empty;
    public Sprite icon => sourceCrop != null ? sourceCrop.HandbookIcon : null;
    public string description => sourceCrop != null ? sourceCrop.description : string.Empty;
    public int buyPrice => sourceCrop != null ? sourceCrop.BuyPrice : 0;
    public int sellPrice => sourceCrop != null ? sourceCrop.SellPrice : 0;
    public Sprite[] stateSprites => sourceCrop != null ? sourceCrop.stateSprites : System.Array.Empty<Sprite>();
    public float[] stateDurations => sourceCrop != null ? sourceCrop.GetAllStageDurations() : System.Array.Empty<float>();

    public void SetSourceCrop(Crop crop)
    {
        sourceCrop = crop;
    }
}
