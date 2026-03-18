using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class AddItemInfo : MonoBehaviour
{
    [SerializeField] private List<ItemInfo> listItemInfo1;
    [SerializeField] private List<ItemInfo> listItemInfo2;
    [SerializeField] private GameObject itemInfoPrefab;
    [SerializeField] private Transform listInfo1;
    [SerializeField] private Transform listInfo2;

    private void CreateItem(ItemInfo item, Transform anchor)
    {
        Transform parent = anchor.parent;
        GameObject itemObject = Instantiate(itemInfoPrefab, parent);

        int index = anchor.GetSiblingIndex();
        itemObject.transform.SetSiblingIndex(index + 1);

        TextMeshProUGUI[] textComponents = itemObject.GetComponentsInChildren<TextMeshProUGUI>();
        UnityEngine.UI.Image imageComponent = itemObject.GetComponentInChildren<UnityEngine.UI.Image>();

        if (textComponents.Length >= 1)
            textComponents[0].text = item.description;

        if (imageComponent != null && item.icon != null)
            imageComponent.sprite = item.icon;
    }
    
    void Start()
    {
        foreach (var item in listItemInfo1)
        {
            CreateItem(item, listInfo1);
        }

        foreach (var item in listItemInfo2)
        {
            CreateItem(item, listInfo2);
        }
    }
}
