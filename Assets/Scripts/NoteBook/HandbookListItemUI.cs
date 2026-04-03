using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandbookListItemUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Button button;
    [SerializeField] private Image background;

    [Header("Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = new Color(0.9f, 0.8f, 0.5f);

    private int itemIndex;
    private HandbookItemData currentData;
    private Action<int> onSelected;

    public HandbookItemData Data => currentData;
    public int ItemIndex => itemIndex;

    void Awake()
    {
        AutoAssignReferences();
        BindButton();
        SetSelected(false);
    }

    void OnValidate()
    {
        AutoAssignReferences();
    }

    public void Setup(HandbookItemData data, int index, Action<int> onItemSelected = null)
    {
        currentData = data;
        itemIndex = index;
        onSelected = onItemSelected;

        Refresh();
        BindButton();
    }

    public void Refresh()
    {
        if (nameText != null)
            nameText.text = currentData != null ? currentData.itemName : string.Empty;

        if (icon != null)
        {
            Sprite itemIcon = currentData != null ? currentData.icon : null;
            icon.sprite = itemIcon;
            icon.enabled = itemIcon != null;
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (background != null)
            background.color = isSelected ? selectedColor : normalColor;
    }

    public void SetInteractable(bool isInteractable)
    {
        if (button != null)
            button.interactable = isInteractable;
    }

    void BindButton()
    {
        if (button == null)
            return;

        button.onClick.RemoveListener(HandleClick);
        button.onClick.AddListener(HandleClick);
    }

    void HandleClick()
    {
        onSelected?.Invoke(itemIndex);
    }

    void AutoAssignReferences()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (background == null)
            background = GetComponent<Image>();

        if (nameText == null)
            nameText = GetComponentInChildren<TMP_Text>(true);

        if (icon == null)
        {
            Image[] images = GetComponentsInChildren<Image>(true);
            foreach (Image image in images)
            {
                if (image != background)
                {
                    icon = image;
                    break;
                }
            }
        }
    }
}
