using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    public GameObject fireballPrefab;
    public GameObject powderPrefab;

    InventoryManager inventory;
    PlayerMovement movement;
    PlayerElevation elevation;

    [SerializeField]
    BottlableData bottled = null;

    bool canUseItems = true;

    void Start() {
        inventory = InventoryManager.Get();

        movement = this.GetComponentInChildren<PlayerMovement>();
        elevation = this.GetComponent<PlayerElevation>();
    }

    public void UseItem() {
        if (!canUseItems) return;

        switch (inventory.currentItem.itemKind) {
            case ItemsEnum.BOTTLE:
                UseBottle();
                break;
            case ItemsEnum.FIRE_FRUIT:
                UseFireball();
                break;
            case ItemsEnum.POWDER:
                UsePowder();
                break;
        }
    }

    void UseBottle() {
        var hits = Physics2D.RaycastAll(
            this.transform.position,
            movement.lastDirection,
            2f);
        if (this.bottled == null || this.bottled.sprite == null) {
            foreach (var hit in hits) {
                var bottlable = hit.transform.GetComponent<Bottlable>();
                if (bottlable != null) {
                    this.bottled = new BottlableData(
                        bottlable.data.prefabName,
                        bottlable.data.sprite
                    );
                    inventory.ChangeBottleIcon(bottled.sprite);
                    Destroy(hit.transform.gameObject);
                    return;
                }
            }
        } else {
            hits = Physics2D.RaycastAll(
                this.transform.position,
                movement.lastDirection,
                2f,
                1 << LayerMask.NameToLayer("Walls")
                    | (1 << LayerMask.NameToLayer("Stairs"))
                    | TilemapHelper.GetLayerMaskCreatureCollision(elevation.currentLevel));
            if (hits.Length == 0) {
                var obj = Instantiate(
                    Resources.Load(
                            "Prefabs/Creatures/" + bottled.prefabName,
                            typeof(GameObject)) as GameObject,
                    Vector2.zero,
                    Quaternion.identity,
                    this.transform.parent);
                obj.transform.localPosition = TilemapHelper.TilePosInFrontOfObj(
                    this.gameObject,
                    movement.lastDirection);
                bottled = null;
                inventory.ChangeBottleIcon(null);
            }
        }
    }

    void UseFireball() {
        var obj = Instantiate(
            fireballPrefab,
            this.transform.position,
            Quaternion.identity,
            this.transform.parent);
        var fireball = obj.GetComponent<Fireball>();
        fireball.Init(movement.lastDirection);
        inventory.Change(ItemsEnum.FIRE_FRUIT, -1);
    }

    void UsePowder() {
        var obj = Instantiate(
            powderPrefab,
            Vector2.zero,
            Quaternion.identity,
            this.transform.parent);
        obj.transform.localPosition = TilemapHelper.TilePosInFrontOfObj(this.gameObject, movement.lastDirection);
        inventory.Change(ItemsEnum.POWDER, -1);
    }

    public void SelectNextItem() {
        inventory.SelectNextItem();
    }

    public void SetActive(bool value) {
        this.canUseItems = value;
    }
}
