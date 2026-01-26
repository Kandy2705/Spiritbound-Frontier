using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropData", menuName = "CropData", order = 0)]
public class CropData : ScriptableObject
{
    public int TimeToGrow;
    public int HarvestQuantity;
    public Sprite[] GrowthStages;
    public Sprite ReadyToHarvest;
    public Collectable HarvestedCrop;
}