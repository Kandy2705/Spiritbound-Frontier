using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitReaction : PlayerHit
{
    GameObject toolbar;
    ToolbarController toolbarController;
    
    public override void Hit()
    {
        toolbar = GameObject.FindWithTag("toolbar");
        toolbarController = GetComponent<ToolbarController>();
        
        GameManager.instance.inventoryContainer.RemoveItem(GameManager.instance.toolbarControllerGlobal.GetItem, 1);
        toolbar.SetActive(!toolbar.activeInHierarchy);
        toolbar.SetActive(true);
    }
}