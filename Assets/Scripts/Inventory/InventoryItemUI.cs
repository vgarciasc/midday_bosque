using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    public GameObject selectedMarker;
    public Image icon;

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

    public void FadeError() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(icon.DOFade(0.5f, 0f));
        sequence.Append(icon.DOFade(1f, 0f).SetDelay(0.2f));
        sequence.Play();
    }
}
