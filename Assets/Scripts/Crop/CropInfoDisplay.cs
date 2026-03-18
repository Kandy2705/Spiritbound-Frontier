using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CropInfoDisplay : MonoBehaviour
{
    [SerializeField] private CropsManager cropsManager;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject panel;
    [SerializeField] private Image cropImage;
    [SerializeField] private GameObject imageInfo;
    
    private Vector3Int currentCropPosition;
    private bool isShowing = false;

    void Awake()
    {
        if (panel != null)
            panel.SetActive(false);
    }
    
    void Update()
    {
        // Cập nhật real-time khi đang hiển thị
        if (isShowing && cropsManager != null && cropsManager.crops.ContainsKey(currentCropPosition))
        {
            UpdateTimeDisplay();
        }
    }
    
    private void UpdateTimeDisplay()
    {
        if (timeText != null)
        {
            Crop crop = cropsManager.crops[currentCropPosition];
            if (!crop.timerIsRunning)
                timeText.text = "Needs watering";
            else
                timeText.text = $"Next stage: {FormatTime(crop.timeRemaining)}";
        }
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

        currentCropPosition = position;
        isShowing = true;
        panel.SetActive(true);

        Crop crop = cropsManager.crops[position];

        // Hiển thị tên cây (bỏ "(Clone)" suffix)
        if (nameText != null)
        {
            string displayName = string.IsNullOrEmpty(crop.Name) ? crop.name.Replace("(Clone)", "").Trim() : crop.Name;
            nameText.text = "Name: " + displayName;
        }

        // Cập nhật thời gian (sẽ được cập nhật real-time trong Update)
        UpdateTimeDisplay();

        // Lấy sprite từ CropObject hiện tại (sprite của state hiện tại)
        if (cropImage != null)
        {
            Sprite currentSprite = null;

            // Lấy CropObject tại vị trí này
            if (cropsManager.cropObjects.ContainsKey(position))
            {
                GameObject cropObj = cropsManager.cropObjects[position];
                if (cropObj != null)
                {
                    CropSpriteController spriteCtrl = cropObj.GetComponent<CropSpriteController>();
                    if (spriteCtrl != null)
                        currentSprite = spriteCtrl.GetCurrentSprite();
                }
            }

            // Fallback: lấy sprite từ stateSprites[0] nếu không tìm được
            if (currentSprite == null && crop.stateSprites != null && crop.stateSprites.Length > 0)
                currentSprite = crop.stateSprites[0];

            if (currentSprite != null)
            {
                cropImage.sprite = currentSprite;
                cropImage.gameObject.SetActive(true);
            }
            else
            {
                cropImage.gameObject.SetActive(false);
            }
            
            // Cập nhật thêm imageInfo nếu có
            if (imageInfo != null)
            {
                Image infoImage = imageInfo.GetComponent<Image>();
                if (infoImage != null && currentSprite != null)
                    infoImage.sprite = currentSprite;
            }
        }
    }

    public void Hide()
    {
        isShowing = false;
        if (panel != null)
            panel.SetActive(false);
    }
    
    // Tắt info panel khi nhấn ra ngoài
    public void HideInfo()
    {
        Hide();
    }

    string FormatTime(float time)
    {
        if (time < 0) time = 0;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
