using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhitePowder : MonoBehaviour, PlayerInteractive {
    public delegate void AcquisionDelegate(GameObject obj);
    public event AcquisionDelegate acquisitionEvent;

    public void OnInteraction(GameObject player) {
        InventoryManager.Get().Change(ItemsEnum.POWDER, 1);
        if (acquisitionEvent != null) {
            acquisitionEvent(this.transform.gameObject);
        }
    }
}
