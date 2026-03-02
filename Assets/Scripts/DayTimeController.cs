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


    void Update()
    {
        if (Time.timeScale == 0)
            return;

        //Kontrola czasu i wyświetlanie
        time += Time.deltaTime * TimeScale;
        int hours = (int)getHours;
        TimeDisplay.text = hours.ToString("00") + ":00";
        UnityEngine.Rendering.Universal.Light2D light = transform.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        //Światło dzienne od 4 do 20
        if (time > 25200f && time < 72000f)
        {
            light.intensity = 1f;
        }

        //Rozjaśnia się w godzinach 20 - 4
        if ((time > 72000f && time < 86400f) || ((time > 0f && time < 18000f)))
        {
            if (light.intensity > 0.3f)
            {
                light.intensity -= LightTransition;
            }
        }
        
        //Lights up 4 - 7
        if (time > 18000f && time < 25200f)
        {
            if (light.intensity < 1f)
                light.intensity += LightTransition;
        }

        //Zmiana dnia na nowy
        if (time > SecondsInDay)
        {
            time = 0;
            day += 1;
            //codzienna dostawa punktow
            MoneyController.money += 200;
        }

        if(day == 9 && time > 25200f)
        {
            Application.LoadLevel(4);
        }
    }

}
