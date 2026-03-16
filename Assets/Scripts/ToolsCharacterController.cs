using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System;

public class ToolsCharacterController : MonoBehaviour
{
    PlayerControl character;
    Rigidbody2D rgbd2d;
    [SerializeField] MarkerManager markerManager;
    [SerializeField] TileMapReadController tileMapReadController;
    [SerializeField] CropsReadController cropsReadController;
    [SerializeField] float maxDistance = 2f;
    [SerializeField] CropsManager cropsManager;
    [SerializeField] TileData plowableTiles;
    [SerializeField] TileData toMowTiles;
    [SerializeField] TileData toSeedTiles;
    [SerializeField] TileData waterableTiles;
    InventoryController inventoryController;
    ToolbarController toolbarController;
    [SerializeField] CropInfoDisplay cropInfoDisplay;
    [SerializeField] GameObject toolbarPanel;

    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;

    private static int cornPickUpCount = 3;
    private static int parsleyPickUpCount = 1;
    private static int potatoPickUpCount = 1;
    private static int strawberryPickUpCount = 1;
    private static int tomatoPickUpCount = 1;

    private static int cornSeedsCount = 4;
    private static int parsleySeedsCount = 3;
    private static int potatoSeedsCount = 1;
    private static int strawberrySeedsCount = 6;
    private static int tomatoSeedsCount = 3;

    Vector3Int selectedTilePosition;
    Vector3Int selectedCropPosition;
    bool selectable;

    public static Dictionary<Vector2Int, TileData> fields;
    public static Dictionary<Vector2Int, CropData> crops;

    UI_ShopController shopPanel;


    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<PlayerControl>();
        rgbd2d = GetComponent<Rigidbody2D>();
        fields = new Dictionary<Vector2Int, TileData>();
        crops = new Dictionary<Vector2Int, CropData>();
        toolbarController = GetComponent<ToolbarController>();
        inventoryController = GetComponent<InventoryController>();

        var shopPanelAll = Resources.FindObjectsOfTypeAll<UI_ShopController>();
        shopPanel = shopPanelAll[0];
    }

    // Update is called once per frame
    void Update()
    {
        SelectTile();
        CanSelectCheck();
        Marker();
        if (Input.GetMouseButtonDown(0))
        {
            if (!inventoryController.isOpen)
            {
                // Tắt info panel khi nhấn chuột trái
                if (cropInfoDisplay != null)
                    cropInfoDisplay.HideInfo();
                    
                if (UseToolWorld() == true)
                {
                    return;
                }
                UseTool();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (!inventoryController.isOpen)
            {
                if (cropInfoDisplay != null)
                    cropInfoDisplay.Show(selectedTilePosition);
            }
        }
    }

    private bool CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit)
        {
            //Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.name.Contains("Tree"))
            {
                return true;
            }
            if (hit.collider.gameObject.name.Contains("CampFire"))
            {
                return true;
            }
            if (hit.collider.gameObject.name.Contains("Chest"))
            {
                return true;
            }
        }
        return false;
    }
    private bool CastRayPlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit)
        {
            if (hit.collider.gameObject.name.Contains("Player"))
            {
                return true;
            }
        }
        return false;
    }
            private void SelectTile()
    {
        selectedTilePosition = tileMapReadController.GetGridPosition(Input.mousePosition, true);
        TileBase tileBase = tileMapReadController.GetTileBase(selectedTilePosition);
        try
        {
            TileData tileData = tileMapReadController.GetTileData(tileBase);
            if (!(tileData is null))
            {
                if (!fields.ContainsKey((Vector2Int)selectedTilePosition))
                {
                    fields.Add((Vector2Int)selectedTilePosition, tileData);
                }
                else
                {
                    fields[(Vector2Int)selectedTilePosition] = tileData;
                }
            }
        }
        catch
        {
            return;
        }

        selectedCropPosition = cropsReadController.GetGridPosition(Input.mousePosition, true);
        TileBase cropBase = cropsReadController.GetTileBase(selectedTilePosition);
        try
        {
            CropData cropData = cropsReadController.GetCropData(cropBase);
            if (!(cropData is null))
            {
                if (!crops.ContainsKey((Vector2Int)selectedTilePosition))
                {
                    crops.Add((Vector2Int)selectedTilePosition, cropData);
                }
                else
                {
                    crops[(Vector2Int)selectedTilePosition] = cropData;
                }
            }
        }
        catch
        {
            return;
        }

    }

    void CanSelectCheck()
    {
        if (Time.timeScale == 0) //if game paused
            return;

        Vector2 characterPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
        markerManager.Show(selectable);
    }

    private void CheckingWheter(string toolName, int seedsCount)
    {
        if (GameManager.instance.inventoryContainer.slots[toolbarController.selectedTool].count 
            >= seedsCount)
        {
            cropsManager.SeedCrop(selectedTilePosition, toolName);
            GameManager.instance.inventoryContainer.RemoveItem(toolbarController.GetItem, seedsCount);
        }
    }

    private void CheckingIfHasEnoughSeeds(string toolName, string foodToolName, int pickUpCount)
    {                
        cropsManager.Collect(selectedTilePosition, toolName);
        // foreach (ItemSlot itemSlot in GameManager.instance.allItemsContainer.slots)
        // {
        //     if (itemSlot.item.Name == foodToolName)
        //     {
        //         GameManager.instance.inventoryContainer.Add(itemSlot.item, pickUpCount);
        //         RefreshToolbar();
        //         break;
        //     }
        // }
    }

    private void Marker()
    {
        markerManager.markedCellPosition = selectedTilePosition;
    }

    // interacting with physical objects in the world
    private bool UseToolWorld()
    {
        if (Time.timeScale == 0)
            return false;

        // CUTTING TREE
        Vector2 position = rgbd2d.position + character.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);


        foreach (Collider2D collidor in colliders)
        {
            ToolHit hitTree = collidor.GetComponent<ToolHit>();
            CampFireHit hitFire = collidor.GetComponent<CampFireHit>();
            ChestHit hitChest = collidor.GetComponent<ChestHit>();
            PlayerHit hitPlayer = collidor.GetComponent<PlayerHit>();

            if (hitTree != null && toolbarController.GetItem != null &&
                toolbarController.GetItem.Name == "Axe" && CastRay() == true)
            {
                hitTree.Hit();
                // Debug.Log("we can hit");
                return true;
            }
            if (hitFire != null && toolbarController.GetItem != null &&
                toolbarController.GetItem.Name == "Wood" && CastRay() == true)
            {
                hitFire.Hit();
                return true;
            }
            if (hitChest != null && CastRay() == true)
            {
                hitChest.Hit();
                return true;
            }
            if (hitPlayer != null && toolbarController.GetItem != null && CastRayPlayer() == true && (toolbarController.GetItem.Name == "Food_Corn" || toolbarController.GetItem.Name == "Food_Parsley"
                    || toolbarController.GetItem.Name == "Food_Potato" || toolbarController.GetItem.Name == "Food_Strawberry" || toolbarController.GetItem.Name == "Food_Tomato"))
            {
                hitPlayer.Hit();
                return true;
            }
        }

        return false;
    }

    private void RefreshToolbar()
    {
        toolbarPanel.SetActive(!toolbarPanel.activeInHierarchy);
        toolbarPanel.SetActive(true);
    }

    private void UseTool()
    {
        if (Time.timeScale == 0) //if game paused - return
            {
                return;
            }

        // when sth is present on the grid but you can't plant there
        if (selectable == true && toolbarController.GetItem != null)
        {
            TileBase tileBase = tileMapReadController.GetTileBase(selectedTilePosition);
            TileData tileData = tileMapReadController.GetTileData(tileBase);
            Debug.Log("Selected tile position: " + tileBase + tileData);
            //TileData cropData = cropsReadController.GetTileData(tileBase);

            /*if (toolbarController.GetItem.Name == "WateringCan" && fields[(Vector2Int)selectedTilePosition].watered) //if you are using watering can - play sound of water
                FindObjectOfType<SoundManager>().Play("Water");*/

            if (tileData != plowableTiles && tileData != toMowTiles && tileData != toSeedTiles && tileData != waterableTiles) //if tile doesn't have any ability
            {
                return;
            }

            // Debug.Log("Wybrane narzędzie: " + toolbarController.GetItem.Name);
            //Debug.Log(crops[(Vector2Int)selectedTilePosition]);
            //if there is no plant on tile
            // Kiểm tra từ CropsManager thay vì dictionary cũ
            bool hasCrop = cropsManager.crops.ContainsKey(selectedTilePosition);
            bool isPlanted = hasCrop && cropsManager.crops[selectedTilePosition].planted;
            bool isWaterable = fields[(Vector2Int)selectedTilePosition].waterable;
            bool isWateringCan = toolbarController.GetItem.Name == "WateringCan";

            //if there is no plant on tile
            if (!hasCrop) 
            {
                //usage of tools if tile has suitable ability
                if (fields[(Vector2Int)selectedTilePosition].ableToMow && toolbarController.GetItem.Name == "Shovel" 
                    && shopPanel.isOpen == false)
                {
                    cropsManager.Mow(selectedTilePosition);
                }
                else if (fields[(Vector2Int)selectedTilePosition].plowable && toolbarController.GetItem.Name == "Hoe")
                {
                    cropsManager.Plow(selectedTilePosition);
                }
                else if (fields[(Vector2Int)selectedTilePosition].ableToSeed && toolbarController.GetItem.isSeed == true)
                {
                    switch (toolbarController.GetItem.Name) //depending on what seed you have chosen
                    {
                        case "Seeds_Corn":
                            CheckingWheter("corn", cornSeedsCount);
                        break;
                        case "Seeds_Parsley":
                            CheckingWheter("parsley", parsleySeedsCount);
                        break;
                        case "Seeds_Potato":
                            CheckingWheter("potato", potatoSeedsCount);
                        break;
                        case "Seeds_Strawberry":
                            CheckingWheter("strawberry", strawberrySeedsCount);
                        break;
                        case "Seeds_Tomato":
                            CheckingWheter("tomato", tomatoSeedsCount);
                        break;
                    }

                    // Refreshing the count of seeds
                    RefreshToolbar();
                }               
            }

            //usage of tools if there is a planted tile
            else if (isPlanted && isWaterable && isWateringCan)
            {
                Debug.Log("ket thuc tuoi nuoc");
                cropsManager.Water(selectedTilePosition);
                FindObjectOfType<SoundManager>().Play("Water");
            }

            else if (toolbarController.GetItem.Name == "Bag" && hasCrop)
            {
                // Kiểm tra thu hoạch dựa vào CropObject state thay vì CropData tile cũ
                Crop cropAtPos = cropsManager.crops[selectedTilePosition];
                CropObject cropObj = null;
                if (cropsManager.cropObjects.ContainsKey(selectedTilePosition))
                    cropObj = cropsManager.cropObjects[selectedTilePosition].GetComponent<CropObject>();

                bool isFullyGrown = cropObj != null && cropObj.IsFullyGrown();

                if (isFullyGrown)
                {
                    string cropName = cropAtPos.name; // e.g. "Corn(Clone)"
                    if (cropName.Contains("Corn") || cropsManager.corns.ContainsKey(selectedTilePosition))
                        CheckingIfHasEnoughSeeds("corn", "Food_Corn", cornPickUpCount);
                    else if (cropName.Contains("Parsley") || cropsManager.parsleys.ContainsKey(selectedTilePosition))
                        CheckingIfHasEnoughSeeds("parsley", "Food_Parsley", parsleyPickUpCount);
                    else if (cropName.Contains("Potato") || cropsManager.potatoes.ContainsKey(selectedTilePosition))
                        CheckingIfHasEnoughSeeds("potato", "Food_Potato", potatoPickUpCount);
                    else if (cropName.Contains("Strawberry") || cropsManager.strawberries.ContainsKey(selectedTilePosition))
                        CheckingIfHasEnoughSeeds("strawberry", "Food_Strawberry", strawberryPickUpCount);
                    else if (cropName.Contains("Tomato") || cropsManager.tomatoes.ContainsKey(selectedTilePosition))
                        CheckingIfHasEnoughSeeds("tomato", "Food_Tomato", tomatoPickUpCount);
                }
                else
                {
                    Debug.Log("Cây chưa đủ trưởng thành để thu hoạch!");
                }
            }
        }
    }
}
