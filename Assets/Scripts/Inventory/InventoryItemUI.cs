using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    bool selected = false;

    public GameObject selectedMarker;

    public void ToggleSelected(bool value) {
        this.selected = value;
        this.selectedMarker.SetActive(this.selected);
    }
}
