using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    public GameObject xprefab;
    InventoryManager inventory;
    PlayerMovement movement;
    PlayerActions playerActions;
    PlayerElevation playerElevation;

    [SerializeField]
    BottlableData bottled = null;

    void Start() {
        inventory = InventoryManager.Get();
        movement = this.GetComponentInChildren<PlayerMovement>();
        playerActions = this.GetComponent<PlayerActions>();
        playerElevation = this.GetComponent<PlayerElevation>();
    }

    void Update() {
        if (playerActions.frozen) return;
        
        HandleUse();
    }

    void HandleUse() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            switch (inventory.currentItem.itemKind) {
                case ItemsEnum.BOTTLE:
                    UseBottle();
                    break;
            }
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
                    | TilemapHelper.GetLayerMaskCreatureCollision(playerElevation.currentLevel));
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
}
