using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhitePowder : MonoBehaviour, PlayerInteractive {
    [HideInInspector]
    public UnityEvent acquisitionEvent;

    public void OnInteraction(GameObject player) {
        InventoryManager.Get().Change(ItemsEnum.POWDER, 1);
        acquisitionEvent.Invoke();
        Destroy(this.gameObject);
    }
}
