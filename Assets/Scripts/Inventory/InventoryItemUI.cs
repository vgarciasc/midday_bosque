using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    public GameObject selectedMarker;

    bool selected = false;
    TextMeshProUGUI itemNameText;

    [SerializeField]
    bool consumable;
    [SerializeField]
    string itemName;

    public void ToggleSelected(bool value) {
        this.selected = value;
        this.selectedMarker.SetActive(this.selected);
    }

    public void SetAmount(int amount) {
        itemNameText = this.GetComponentInChildren<TextMeshProUGUI>();
        itemNameText.text = itemName + " x" + amount.ToString();
    }
}
