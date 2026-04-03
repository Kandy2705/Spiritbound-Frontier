using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class HandbookUI : MonoBehaviour
{
    static HandbookUI instance;

    [Header("Input")]
    [SerializeField] KeyCode toggleKey = KeyCode.N;
    [SerializeField] KeyCode closeKey = KeyCode.Escape;

    [Header("Notebook References")]
    [SerializeField] GameObject noteBookObject;
    [SerializeField] Transform contentRoot;
    [SerializeField] GameObject listItemTemplate;
    [SerializeField] Transform rightPage;

    [Header("Detail References")]
    [SerializeField] TMP_Text detailTitleText;
    [SerializeField] TMP_Text detailPriceText;
    [SerializeField] TMP_Text stateInfoText;
    [SerializeField] Image detailIconImage;
    [SerializeField] Image currentStateImage;
    [SerializeField] Image nextStateImage;
    [SerializeField] Button previousStateButton;
    [SerializeField] Button nextStateButton;

    [SerializeField] TMP_Text detailStateText;
    [SerializeField] TMP_Text currentStateText;

    GameObject toolbarPanel;
    bool toolbarWasActive;

    readonly List<HandbookItemData> handbookItems = new List<HandbookItemData>();
    readonly List<GameObject> spawnedItems = new List<GameObject>();

    int selectedItemIndex = -1;
    int selectedStateIndex;
    bool sceneBound;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        TryBindScene();
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }

    void Update()
    {
        if (!sceneBound)
            TryBindScene();

        if (noteBookObject == null)
            return;

        if (Input.GetKeyDown(toggleKey))
        {
            if (noteBookObject.activeSelf)
                CloseNotebook();
            else
                OpenNotebook();
        }
        else if (noteBookObject.activeSelf && Input.GetKeyDown(closeKey))
        {
            CloseNotebook();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneBound = false;
        toolbarPanel = null;
        toolbarWasActive = false;
        handbookItems.Clear();
        ClearSpawnedItems();
    }

    void TryBindScene()
    {
        if (noteBookObject == null || contentRoot == null || listItemTemplate == null || rightPage == null)
            return;

        EnsureListItemComponent(listItemTemplate);
        BindRightPage();
        BindStateButtons();
        CacheToolbarPanel();
        sceneBound = true;

        if (noteBookObject.activeSelf)
            RefreshNotebook();
    }

    void BindRightPage()
    {
        Transform header = FindDeepChild(rightPage, "Header");
        Transform money = FindDeepChild(rightPage, "Money");
        Transform states = FindDeepChild(rightPage, "States");

        if (detailTitleText == null)
            detailTitleText = CreateOrGetTitleText(header, money);

        if (detailPriceText == null)
            detailPriceText = FindDeepChild(money, "CropName")?.GetComponent<TMP_Text>();

        Transform itemStates = states != null ? FindDeepChild(states, "ItemStates") : null;
        if (stateInfoText == null)
            stateInfoText = FindDeepChild(itemStates, "TimeText")?.GetComponent<TMP_Text>();

        if (detailIconImage == null)
        {
            Transform headerFrame = header != null ? FindDeepChild(header, "Frame") : null;
            Transform iconTransform = headerFrame != null ? FindDeepChild(headerFrame, "Image") : null;
            if (iconTransform == null && header != null)
                iconTransform = FindDeepChild(header, "Image");
            detailIconImage = iconTransform != null ? iconTransform.GetComponent<Image>() : null;
        }

        if (currentStateImage == null)
            currentStateImage = GetInnerImage(itemStates != null ? FindDeepChild(itemStates, "Frame") : null);

        if (nextStateImage == null)
            nextStateImage = GetInnerImage(itemStates != null ? FindDeepChild(itemStates, "Frame (1)") : null);
    }

    TMP_Text CreateOrGetTitleText(Transform header, Transform money)
    {
        TMP_Text existing = FindDeepChild(rightPage, "HandbookDetailTitle")?.GetComponent<TMP_Text>();
        if (existing != null)
            return existing;

        TMP_Text template = money != null ? FindDeepChild(money, "CropName")?.GetComponent<TMP_Text>() : null;
        if (template == null || header == null)
            return template;

        TMP_Text cloned = Instantiate(template, header);
        cloned.name = "HandbookDetailTitle";
        RectTransform rect = cloned.rectTransform;
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = new Vector2(10f, -7f);
        rect.sizeDelta = new Vector2(64f, 16f);
        cloned.alignment = TextAlignmentOptions.Center;
        cloned.enableWordWrapping = false;
        cloned.fontSize = Mathf.Max(cloned.fontSize, 8f);
        return cloned;
    }

    void BindStateButtons()
    {
        Transform navigationBar = FindDeepChild(rightPage, "NavigationBar");
        if (previousStateButton == null)
            previousStateButton = FindDeepChild(navigationBar, "ButtonL")?.GetComponent<Button>();
        if (nextStateButton == null)
            nextStateButton = FindDeepChild(navigationBar, "ButtonR")?.GetComponent<Button>();

        if (previousStateButton != null)
        {
            previousStateButton.onClick.RemoveListener(ShowPreviousState);
            previousStateButton.onClick.AddListener(ShowPreviousState);
        }

        if (nextStateButton != null)
        {
            nextStateButton.onClick.RemoveListener(ShowNextState);
            nextStateButton.onClick.AddListener(ShowNextState);
        }
    }

    void CacheToolbarPanel()
    {
        if (toolbarPanel == null)
            toolbarPanel = FindSceneObjectWithTag("toolbar");
    }

    void OpenNotebook()
    {
        if (!sceneBound)
            return;

        RefreshNotebook();

        toolbarWasActive = toolbarPanel != null && toolbarPanel.activeSelf;
        if (toolbarPanel != null)
            toolbarPanel.SetActive(false);

        noteBookObject.SetActive(true);
    }

    void CloseNotebook()
    {
        if (noteBookObject == null)
            return;

        noteBookObject.SetActive(false);

        if (toolbarPanel != null)
            toolbarPanel.SetActive(toolbarWasActive);
    }

    void RefreshNotebook()
    {
        BuildItemsFromSeeds();
        BuildLeftList();

        if (handbookItems.Count == 0)
        {
            ClearDetail();
            return;
        }

        if (selectedItemIndex < 0 || selectedItemIndex >= handbookItems.Count)
            SelectItem(0);
        else
            UpdateSelectionVisuals();

        UpdateDetailPanel();
    }

    void BuildItemsFromSeeds()
    {
        handbookItems.Clear();

        if (GameManager.instance == null || GameManager.instance.allSeedsContainer == null)
            return;

        HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (SeedSlot seedSlot in GameManager.instance.allSeedsContainer.slots)
        {
            if (seedSlot == null || seedSlot.item == null)
                continue;

            string key = seedSlot.item.ItemName.Trim();
            if (!seen.Add(key))
                continue;

            handbookItems.Add(new HandbookItemData(seedSlot.item));
        }

        handbookItems.Sort((left, right) => string.Compare(left.itemName, right.itemName, StringComparison.OrdinalIgnoreCase));
    }

    void BuildLeftList()
    {
        if (contentRoot == null || listItemTemplate == null)
            return;

        EnsureListItemComponent(listItemTemplate);

        while (spawnedItems.Count < handbookItems.Count - 1)
        {
            GameObject clone = Instantiate(listItemTemplate, contentRoot);
            clone.name = $"CropListItem ({spawnedItems.Count + 2})";
            EnsureListItemComponent(clone);
            spawnedItems.Add(clone);
        }

        for (int i = spawnedItems.Count - 1; i >= Mathf.Max(handbookItems.Count - 1, 0); i--)
        {
            Destroy(spawnedItems[i]);
            spawnedItems.RemoveAt(i);
        }

        if (handbookItems.Count == 0)
        {
            listItemTemplate.SetActive(false);
            return;
        }

        for (int i = 0; i < handbookItems.Count; i++)
        {
            GameObject itemObject = i == 0 ? listItemTemplate : spawnedItems[i - 1];
            itemObject.SetActive(true);
            HandbookListItemUI itemUI = EnsureListItemComponent(itemObject);
            itemUI.Setup(handbookItems[i], i, SelectItem);
            itemUI.SetSelected(i == selectedItemIndex);
        }
    }

    HandbookListItemUI EnsureListItemComponent(GameObject itemObject)
    {
        HandbookListItemUI itemUI = itemObject.GetComponent<HandbookListItemUI>();
        if (itemUI == null)
            itemUI = itemObject.AddComponent<HandbookListItemUI>();

        return itemUI;
    }

    public void SelectItem(int index)
    {
        if (index < 0 || index >= handbookItems.Count)
            return;

        selectedItemIndex = index;
        selectedStateIndex = 0;
        UpdateSelectionVisuals();
        UpdateDetailPanel();
    }

    void UpdateSelectionVisuals()
    {
        if (handbookItems.Count == 0)
            return;

        HandbookListItemUI templateUI = listItemTemplate != null ? listItemTemplate.GetComponent<HandbookListItemUI>() : null;
        if (templateUI != null)
            templateUI.SetSelected(selectedItemIndex == 0);

        for (int i = 0; i < spawnedItems.Count; i++)
        {
            HandbookListItemUI itemUI = spawnedItems[i].GetComponent<HandbookListItemUI>();
            if (itemUI != null)
                itemUI.SetSelected(selectedItemIndex == i + 1);
        }
    }

    void UpdateDetailPanel()
    {
        if (selectedItemIndex < 0 || selectedItemIndex >= handbookItems.Count)
        {
            ClearDetail();
            return;
        }

        HandbookItemData selectedItem = handbookItems[selectedItemIndex];

        if (detailTitleText != null)
            detailTitleText.text = selectedItem.itemName.ToUpperInvariant();

        if (detailPriceText != null)
            detailPriceText.text = $"{selectedItem.buyPrice} coin -> {selectedItem.sellPrice} coin";

        if (detailIconImage != null)
        {
            detailIconImage.sprite = selectedItem.icon;
            detailIconImage.enabled = selectedItem.icon != null;
            detailIconImage.preserveAspect = true;
        }

        UpdateStatePreview(selectedItem);
    }

    void UpdateStatePreview(HandbookItemData selectedItem)
    {
        Sprite[] stateSprites = selectedItem.stateSprites;
        float[] stateDurations = selectedItem.stateDurations;
        int lastStateIndex = Mathf.Max(stateSprites.Length - 1, 0);
        selectedStateIndex = Mathf.Clamp(selectedStateIndex, 0, lastStateIndex);

        Sprite currentSprite = stateSprites.Length > 0 ? stateSprites[selectedStateIndex] : selectedItem.icon;
        Sprite nextSprite = stateSprites.Length > 0 ? stateSprites[Mathf.Min(selectedStateIndex + 1, lastStateIndex)] : selectedItem.icon;

        if (currentStateImage != null)
        {
            currentStateImage.sprite = currentSprite;
            currentStateImage.enabled = currentSprite != null;
            currentStateImage.preserveAspect = true;
        }

        if (nextStateImage != null)
        {
            bool hasNextState = selectedStateIndex < lastStateIndex;
            nextStateImage.sprite = hasNextState ? nextSprite : currentSprite;
            nextStateImage.enabled = nextStateImage.sprite != null;
            nextStateImage.preserveAspect = true;
            if (nextStateImage.transform.parent != null)
                nextStateImage.transform.parent.gameObject.SetActive(nextStateImage.enabled);
        }

        if (stateInfoText != null)
            stateInfoText.text = BuildStateInfoText(selectedItem, stateDurations, lastStateIndex);

        if (previousStateButton != null)
            previousStateButton.interactable = selectedStateIndex > 0;

        if (nextStateButton != null)
            nextStateButton.interactable = selectedStateIndex < lastStateIndex;
    }

    string BuildStateInfoText(HandbookItemData selectedItem, float[] stateDurations, int lastStateIndex)
    {
        string description = string.IsNullOrWhiteSpace(selectedItem.description) ? string.Empty : selectedItem.description.Trim();
        string state1Text;
        string state2Text;
        string timeText = "";

        if (selectedStateIndex >= lastStateIndex)
        {
            state1Text = $"State {selectedStateIndex + 1}/{lastStateIndex + 1}";
            timeText = "Final";
            detailStateText.text = $"{state1Text}";
        }
        else
        {
            float duration = selectedStateIndex < stateDurations.Length ? stateDurations[selectedStateIndex] : 0f;
            state1Text = $"State {selectedStateIndex + 1}";
            state2Text = $"State {selectedStateIndex + 2}";
            timeText = $"{FormatDuration(duration)} s";
            detailStateText.text = $"{state1Text}";
            currentStateText.text = $"{state2Text}";
        }

        return string.IsNullOrWhiteSpace(description) ? timeText : $"{description}\n\n{timeText}";
    }

    void ClearDetail()
    {
        if (detailTitleText != null)
            detailTitleText.text = "HANDBOOK";

        if (detailPriceText != null)
            detailPriceText.text = "No crop data";

        if (stateInfoText != null)
            stateInfoText.text = "Add crops to the seed container to populate the notebook.";

        if (detailIconImage != null)
        {
            detailIconImage.sprite = null;
            detailIconImage.enabled = false;
        }

        if (currentStateImage != null)
        {
            currentStateImage.sprite = null;
            currentStateImage.enabled = false;
        }

        if (nextStateImage != null)
        {
            nextStateImage.sprite = null;
            nextStateImage.enabled = false;
        }

        if (previousStateButton != null)
            previousStateButton.interactable = false;

        if (nextStateButton != null)
            nextStateButton.interactable = false;
    }

    void ShowPreviousState()
    {
        if (selectedItemIndex < 0)
            return;

        selectedStateIndex = Mathf.Max(selectedStateIndex - 1, 0);
        UpdateDetailPanel();
    }

    void ShowNextState()
    {
        if (selectedItemIndex < 0 || selectedItemIndex >= handbookItems.Count)
            return;

        int maxStateIndex = Mathf.Max(handbookItems[selectedItemIndex].stateSprites.Length - 1, 0);
        selectedStateIndex = Mathf.Min(selectedStateIndex + 1, maxStateIndex);
        UpdateDetailPanel();
    }

    void ClearSpawnedItems()
    {
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            if (spawnedItems[i] != null)
                Destroy(spawnedItems[i]);
        }

        spawnedItems.Clear();
    }

    static string FormatDuration(float durationInSeconds)
    {
        durationInSeconds = Mathf.Max(0f, durationInSeconds);
        int totalSeconds = Mathf.RoundToInt(durationInSeconds);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }

    static GameObject FindSceneObjectWithTag(string tagName)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        for (int i = 0; i < allObjects.Length; i++)
        {
            GameObject sceneObject = allObjects[i];
            if (sceneObject == null || !sceneObject.scene.IsValid() || !sceneObject.scene.isLoaded)
                continue;

            if (sceneObject.CompareTag(tagName))
                return sceneObject;
        }

        return null;
    }

    static Transform FindDeepChild(Transform root, string childName)
    {
        if (root == null)
            return null;

        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);
            if (child.name == childName)
                return child;

            Transform nested = FindDeepChild(child, childName);
            if (nested != null)
                return nested;
        }

        return null;
    }

    static Image GetInnerImage(Transform root)
    {
        Transform imageTransform = FindDeepChild(root, "Image");
        if (imageTransform != null)
            return imageTransform.GetComponent<Image>();

        return null;
    }
}
