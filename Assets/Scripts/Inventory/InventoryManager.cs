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

    [HideInInspector]
    public ItemUI currentItem = null;

    void Start() {
        AcquireItem(ItemsEnum.BOTTLE);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            AcquireItem(ItemsEnum.BOTTLE);
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            AcquireItem(ItemsEnum.POWDER);
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            SelectNextItem();
        }
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

    ItemUI GetItemUI(ItemsEnum kind) {
        return items.Find((f) => f.itemKind == kind);
    }

    void SelectNextItem() {
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
}
