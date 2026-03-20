using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHit : ToolHit
{
    //[SerializeField] GameObject pickUpAxe;
    [SerializeField] GameObject pickUpWateringCan;
    [SerializeField] GameObject pickUpHoe;
    [SerializeField] GameObject pickUpShovel;
    [SerializeField] GameObject pickUpBag;
    [SerializeField] GameObject pickUpPotato;

    [SerializeField] int dropCount = 5;
    [SerializeField] float spread = 0.9f;

    List<GameObject> items;

    private void Start()
    {
        items = new List<GameObject>();
        //items.Add(pickUpAxe);
        items.Add(pickUpWateringCan);
        items.Add(pickUpHoe);
        items.Add(pickUpShovel);
        items.Add(pickUpBag);
        items.Add(pickUpPotato);
        Debug.Log($"Chest initialized with {items.Count} items.");
    }

    public override void Hit()
    {
        // safety: if no prefabs assigned, do nothing
        if (items == null || items.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        // filter out null prefabs
        List<GameObject> available = new List<GameObject>();
        foreach (var it in items)
        {
            if (it != null) available.Add(it);
        }

        if (available.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        int spawned = 0;

        // If we have enough distinct prefabs, spawn without replacement
        if (available.Count >= dropCount)
        {
            while (spawned < dropCount)
            {
                int idx = UnityEngine.Random.Range(0, available.Count);
                GameObject prefab = available[idx];

                // drop position
                Vector3 position = transform.position;
                position.x -= spread * UnityEngine.Random.value - spread / 2;
                position.y -= spread * UnityEngine.Random.value - spread / 2;

                Instantiate(prefab, position, Quaternion.identity);

                // remove to avoid repetition
                available.RemoveAt(idx);
                spawned++;
            }
        }
        else
        {
            // spawn all available distinct ones first
            while (available.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, available.Count);
                GameObject prefab = available[idx];

                Vector3 position = transform.position;
                position.x -= spread * UnityEngine.Random.value - spread / 2;
                position.y -= spread * UnityEngine.Random.value - spread / 2;

                Instantiate(prefab, position, Quaternion.identity);

                available.RemoveAt(idx);
                spawned++;
            }

            // if still need more, spawn random (with replacement) from the original items list
            while (spawned < dropCount)
            {
                int idx = UnityEngine.Random.Range(0, items.Count);
                GameObject prefab = items[idx];
                if (prefab == null) continue;

                Vector3 position = transform.position;
                position.x -= spread * UnityEngine.Random.value - spread / 2;
                position.y -= spread * UnityEngine.Random.value - spread / 2;

                Instantiate(prefab, position, Quaternion.identity);
                spawned++;
            }
        }

        Destroy(gameObject);
    }
}
