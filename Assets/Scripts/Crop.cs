using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] private CropData data;
    private float growthProgress;
    private SpriteRenderer spriteRenderer;

    public bool CanHarvest()
    {
        return growthProgress >= 100;
    }

    public void OnPlant()
    {
        GameManager.instance.timer.CheckCropProgress += CheckProgress;
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    private void CheckProgress(Timer t)
    {
        growthProgress += t.counter / data.TimeToGrow * 100;

        if (growthProgress >= 100)
        {
            spriteRenderer.sprite = data.GrowthStages[3];
            GameManager.instance.timer.CheckCropProgress -= CheckProgress;
        }
        else if (growthProgress >= 50)
        {
            spriteRenderer.sprite = data.GrowthStages[2];
        }
        else if (growthProgress >= 25)
        {
            spriteRenderer.sprite = data.GrowthStages[1];
        }
    }

    public void OnHarvest()
    {
        for (int i = 0; i < data.HarvestQuantity; i++)
        {
            Vector2 spawnPoint = transform.position;
            Vector2 spawnOffset = Random.insideUnitCircle + new Vector2(1f, 1.5f);
            Instantiate(data.HarvestedCrop, spawnPoint + spawnOffset, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
