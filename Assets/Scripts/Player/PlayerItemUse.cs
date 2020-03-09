using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    public UnityEvent itemUseError;
    public UnityEvent bottleUseIn;
    public UnityEvent bottleUseOut;
    public UnityEvent putPowder;
    public UnityEvent throwFireball;

    public GameObject fireballPrefab;
    public GameObject powderPrefab;

    InventoryManager inventory;
    PlayerMovement movement;
    Flooring flooring;

    [SerializeField]
    BottlableData bottled = null;

    bool canUseItems = true;

    void Start() {
        inventory = InventoryManager.Get();

        movement = this.GetComponentInChildren<PlayerMovement>();
        flooring = this.GetComponentInChildren<Flooring>();
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
        Bounds bounds = new Bounds(this.transform.position + movement.lastDirection, Vector2.one * 0.5f);
        var hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max);
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
                    bottleUseIn.Invoke();
                    return;
                }
            }

            itemUseError.Invoke();
        } else {
            List<Vector3> directions = new List<Vector3>() { movement.lastDirection };

            if (movement.lastDirection == Vector3.left) {
                directions.Add(new Vector2(-1, 1));
                directions.Add(new Vector2(-1, -1));
            } else if (movement.lastDirection == Vector3.right) {
                directions.Add(new Vector2(1, 1));
                directions.Add(new Vector2(1, -1));
            }

            foreach (var dir in directions) {
                var nextTile = TilemapHelper.TilePosInFrontOfObj(this.gameObject, dir);
                Bounds tileBounds = new Bounds(nextTile, Vector2.one * 1.5f);
                var tileHits = Physics2D.OverlapAreaAll(tileBounds.min, tileBounds.max,
                    1 << LayerMask.NameToLayer("Walls")
                    | (1 << LayerMask.NameToLayer("Stairs"))
                    | (TilemapHelper.GetLayerMaskCreatureCollision(flooring.currentLevel)));
                
                int k = 0;
                foreach (var hit in tileHits) {
                    if (!hit.CompareTag("Player")) {
                        k++;
                    }
                }

                if (k == 0) {
                    var obj = Instantiate(
                        Resources.Load(
                                "Prefabs/Creatures/" + bottled.prefabName,
                                typeof(GameObject)) as GameObject,
                        this.transform.position,
                        Quaternion.identity,
                        this.transform.parent);
                    obj.transform.localPosition = nextTile;
                    bottled = null;
                    inventory.ChangeBottleIcon(null);
                    bottleUseOut.Invoke();
                    return;
                }
            }

            itemUseError.Invoke();
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
        throwFireball.Invoke();
    }

    void UsePowder() {
        var targetPosInt = (this.transform.position - this.transform.localPosition)
                           + TilemapHelper.TilePosInFrontOfObj(this.gameObject, movement.lastDirection);
        Bounds bounds = new Bounds(targetPosInt, Vector2.one * 0.5f);
        var hits = Physics2D.OverlapAreaAll(bounds.min, bounds.max);
        foreach (var hit in hits) {
            if (hit.GetComponentInChildren<ChemPowder>() != null) {
                return;
            }
        }

        var obj = Instantiate(
            powderPrefab,
            Vector2.zero,
            Quaternion.identity,
            this.transform.parent);
        obj.transform.position = targetPosInt;
        inventory.Change(ItemsEnum.POWDER, -1);
        putPowder.Invoke();
    }

    public void SelectNextItem() {
        inventory.SelectNextItem();
    }

    public void SetActive(bool value) {
        this.canUseItems = value;
    }
}
