using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public enum ItemsEnum {
    BOTTLE, POWDER, FIRE_FRUIT
}

[System.Serializable]
public class ItemUI {
    public ItemsEnum itemKind;
    public GameObject objUI;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Get() {
        return (InventoryManager) HushPuppy.safeFindComponent("GameController", "InventoryManager");
    }

    [SerializeField]
    List<ItemUI> items = new List<ItemUI>();
    [SerializeField]
    AudioClip selectItemClip;

    [HideInInspector]
    public ItemUI currentItem = null;

    Dictionary<ItemsEnum, int> inventory = new Dictionary<ItemsEnum, int>();

    void Start() {
        foreach (ItemsEnum item in System.Enum.GetValues(typeof(ItemsEnum))) {
            inventory.Add(item, 0);
        }

        AcquireItem(ItemsEnum.BOTTLE);
        Change(ItemsEnum.FIRE_FRUIT, 999);
        Change(ItemsEnum.POWDER, 3);

        var player = GameObject.FindGameObjectWithTag("Player");
        var playerItemUse = player.GetComponentInChildren<PlayerItemUse>();
        playerItemUse.itemUseError.AddListener(ItemUseError);
    }

    void AcquireItem(ItemsEnum kind) {
        var obj = GetItemUI(kind).objUI;
        obj.transform.SetAsLastSibling();
        obj.SetActive(true);
        
        if (currentItem.objUI == null) {
            obj.GetComponent<InventoryItemUI>().ToggleSelected(true);
            currentItem = GetItemUI(kind);
        }
    }

    void UnacquireItem(ItemsEnum kind) {
        var obj = GetItemUI(kind).objUI;
        // obj.transform.SetAsLastSibling();
        obj.SetActive(false);
        SelectNextItem();
    }

    ItemUI GetItemUI(ItemsEnum kind) {
        return items.Find((f) => f.itemKind == kind);
    }

    public void SelectNextItem() {
        if (currentItem == null) return;
        
        int currIndex = currentItem.objUI.transform.GetSiblingIndex();
        int nextIndex = currIndex;

        do {
            nextIndex = (nextIndex + 1) % (currentItem.objUI.transform.parent.childCount);
            GameObject nextIndexObj = currentItem.objUI.transform.parent.GetChild(nextIndex).gameObject;

            if (nextIndexObj.activeSelf) {
                this.currentItem.objUI.GetComponent<InventoryItemUI>().ToggleSelected(false);
                nextIndexObj.GetComponent<InventoryItemUI>().ToggleSelected(true);

                this.currentItem = this.items.Find((f) => f.objUI == nextIndexObj);
                EasyAudio.Get().audio.PlayOneShot(this.selectItemClip, 0.05f);
                return;
            }
        } while (nextIndex != currIndex);
    }

    public void ChangeBottleIcon(Sprite sprite) {
        var bottle = GetItemUI(ItemsEnum.BOTTLE).objUI;
        var marker = bottle.transform.GetChild(bottle.transform.childCount - 1);
        
        if (sprite != null) {
            marker.gameObject.SetActive(true);
            marker.GetComponent<Image>().sprite = sprite;
        } else {
            marker.gameObject.SetActive(false);
        }
    }

    public void Change(ItemsEnum itemkind, int amount) {
        var itemUI = GetItemUI(itemkind).objUI.GetComponent<InventoryItemUI>();
        
        if (inventory[itemkind] == 0 && amount > 0) {
            AcquireItem(itemkind);
        }
        if (inventory[itemkind] + amount == 0) {
            UnacquireItem(itemkind);
        }

        inventory[itemkind] += amount;
        itemUI.SetAmount(inventory[itemkind]);
    }

    void ItemUseError() {
        if (currentItem != null) {
            currentItem.objUI.GetComponent<InventoryItemUI>().FadeError();
        }
    }
}
