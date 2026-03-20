using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DayTimeController : MonoBehaviour
{
    const float SecondsInDay = 86400f;
    public float time;
    [SerializeField] Text TimeDisplay;
    [SerializeField] float TimeScale;
    [SerializeField] float LightTransition = 0.0001f;
    public int day;

    private void Start()
    {
        day = 0;
        time = 25200f;
    }
    private float getHours
    {
        get { return time / 3600f; }
    }

    public float GetTime
    {
        get { return time; }
    }


    public int AddMoney(float time)
    {
        return 200 + ((int)(time / 86400f) - 1)*10;
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        time += Time.deltaTime * TimeScale;
        int hours = (int)getHours;
        TimeDisplay.text = hours.ToString("00") + ":00";
        UnityEngine.Rendering.Universal.Light2D light = transform.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        if (time > 25200f && time < 72000f)
        {
            light.intensity = 1f;
        }

        if ((time > 72000f && time < 86400f) || ((time > 0f && time < 18000f)))
        {
            if (light.intensity > 0.3f)
            {
                light.intensity -= LightTransition;
            }
        }
        
        if (time > 18000f && time < 25200f)
        {
            if (light.intensity < 1f)
                light.intensity += LightTransition;
        }

        if (time > SecondsInDay)
        {
            time = 0;
            day += 1;
            MoneyController.money += AddMoney(time);
        }
    }

}
