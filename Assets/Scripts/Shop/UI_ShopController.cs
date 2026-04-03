using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UI_ShopController : MonoBehaviour
{
    private Transform container;
    private Transform shopItemTemplate;
    private readonly Dictionary<string, Crop> cropsByKey = new Dictionary<string, Crop>();
    [SerializeField] private MoneyController money;
    public Button btn;
    [SerializeField] GameObject toolbarPanel;
    [SerializeField] GameObject inventoryPanel;
    public bool isOpen;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
    }

    private void Start()
    {

        Dictionary<string, Sprite> plantsDictionary = CreateSeedsFromSprite();
        CacheCropData();

        CreateItemButton(plantsDictionary["Seeds_Corn"], "Seeds_Corn", "corn", 0, "Corn Seeds");
        CreateItemButton(plantsDictionary["Seeds_Parsley"], "Seeds_Parsley", "parsley", 1, "Parsley Seeds");
        CreateItemButton(plantsDictionary["Seeds_Tomato"], "Seeds_Tomato", "tomato", 2, "Tomato Seeds");
        CreateItemButton(plantsDictionary["Seeds_Strawberry"], "Seeds_Strawberry", "strawberry", 3, "Strawberry seeds");
        CreateItemButton(plantsDictionary["Seeds_Potato"], "Seeds_Potato", "potato", 4, "Potato tuber");

        gameObject.SetActive(false);
        Hide();
    }

    private Dictionary<string, Sprite> CreateSeedsFromSprite()
    {
        Dictionary<string, Sprite> plantsDictionary = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Plants");

        foreach (Sprite sprite in sprites)
        {
            plantsDictionary.Add(sprite.name, sprite);
        }

        return plantsDictionary;

    }

    private void CacheCropData()
    {
        cropsByKey.Clear();

        if (GameManager.instance == null || GameManager.instance.allSeedsContainer == null)
            return;

        foreach (SeedSlot seedSlot in GameManager.instance.allSeedsContainer.slots)
        {
            if (seedSlot.item == null)
                continue;

            string key = seedSlot.item.ItemName.Trim().ToLowerInvariant();
            if (!cropsByKey.ContainsKey(key))
                cropsByKey.Add(key, seedSlot.item);
        }
    }

    private int GetCropBuyPrice(string cropKey)
    {
        if (cropsByKey.TryGetValue(cropKey.Trim().ToLowerInvariant(), out Crop crop))
            return crop.BuyPrice;

        switch (cropKey.Trim().ToLowerInvariant())
        {
            case "corn":
                return 100;
            case "parsley":
                return 30;
            case "tomato":
                return 60;
            case "strawberry":
                return 150;
            case "potato":
                return 110;
            default:
                return 0;
        }
    }

    private void CreateItemButton(Sprite itemSprite, string itemName, string cropKey, int positionIndex, string displayedName)
    {
        int itemCost = GetCropBuyPrice(cropKey);
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        float shopItemHeight = 60f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, 150 + (-shopItemHeight * positionIndex));
        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(displayedName);
        shopItemTransform.Find("priceText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("itemIcon").GetComponent<Image>().sprite = itemSprite;

        Item newItem = ScriptableObject.CreateInstance<Item>();

        foreach (ItemSlot itemSlot in GameManager.instance.allItemsContainer.slots)
        {
            if (itemSlot.item.Name == itemName)
            {
                newItem = itemSlot.item;
            }
        }

        btn = shopItemTransform.GetComponent<Button>();
        btn.onClick.AddListener(delegate { TaskWithParameters(itemCost, newItem); });
    }

    void TaskWithParameters(long itemCost, Item item)
    {
        if (money.canBuyItems(itemCost))
        {
            money.substractMoney(itemCost);
            FindObjectOfType<SoundManager>().Play("Money");

            if (item.Name.Contains("Seeds_Corn"))
            {
                GameManager.instance.inventoryContainer.Add(item, 4);
            }
            else if (item.Name.Contains("Seeds_Tomato"))
            {
                GameManager.instance.inventoryContainer.Add(item, 3);
            }
            else if (item.Name.Contains("Seeds_Strawberry"))
            {
                GameManager.instance.inventoryContainer.Add(item, 6);
            }
            else if (item.Name.Contains("Seeds_Parsley"))
            {
                GameManager.instance.inventoryContainer.Add(item, 3);
            }
            else if (item.Name.Contains("Seeds_Potato"))
            {
                GameManager.instance.inventoryContainer.Add(item, 1);
            }
            else
            {
                GameManager.instance.inventoryContainer.Add(item);
            }
        }
        

        toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);
        toolbarPanel.SetActive(true);
    }

    public void Show()
    {
        isOpen = true;
        gameObject.SetActive(true);
        
    }

    public void Hide()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }

    //dodać obsługę kliknięcia i zakup przedmiotu

    private void Update()
    {
        inventoryPanel.SetActive(false);
        toolbarPanel.SetActive(true);
    }
}
