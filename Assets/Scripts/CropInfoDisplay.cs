using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CropInfoDisplay : MonoBehaviour
{
    [SerializeField] private CropsManager cropsManager;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameManager gameManager;

    private Image cropImage;

    void Awake()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void Show(Vector3Int position)
    {
        if (cropsManager == null || panel == null)
            return;

        if (!cropsManager.crops.ContainsKey(position))
        {
            panel.SetActive(false);
            return;
        }

        Crop crop = cropsManager.crops[position];
        panel.SetActive(true);

        if (nameText != null)
            nameText.text = string.IsNullOrEmpty(crop.Name) ? "Plant" : "Name: " + crop.Name;

        if (timeText != null)
        {
            if (!crop.timerIsRunning)
                timeText.text = "Needs watering";
            else
                timeText.text = $"Next stage: {FormatTime(crop.timeRemaining)}";
        }

        if (cropImage != null && gameManager != null && gameManager.toolbarControllerGlobal != null)
        {
            var toolbarItem = gameManager.toolbarControllerGlobal.GetItem;
            if (toolbarItem != null && toolbarItem.icon != null)
            {
                cropImage.sprite = toolbarItem.icon;
                cropImage.gameObject.SetActive(true);
            }
            else
            {
                cropImage.gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    string FormatTime(float time)
    {
        if (time < 0)
            time = 0;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
