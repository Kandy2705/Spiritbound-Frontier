using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float counter = 0;

    public UnityAction<Timer> CheckCropProgress;

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (counter >= 5)
        {
            CheckCropProgress?.Invoke(this);
            counter = 0;
        }
    }
}
