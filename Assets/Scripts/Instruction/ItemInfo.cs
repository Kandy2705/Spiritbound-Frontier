using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/ItemInfo")]

public class ItemInfo : ScriptableObject
{
    public Sprite icon;
    public string description;
    public string itemName; // Thêm property cho tên item
}
