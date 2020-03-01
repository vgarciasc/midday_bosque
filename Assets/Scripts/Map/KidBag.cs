using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidBag : MonoBehaviour, PlayerInteractive
{
    bool active = false;
    
    [SerializeField]
    Sprite bagOpen;

    public void OnInteraction(GameObject player)
    {
        if (active) return;
        active = true;

        this.GetComponent<SpriteRenderer>().sprite = bagOpen;
        
        InventoryManager.Get().AcquireItem(ItemsEnum.BOTTLE);
    }
}
