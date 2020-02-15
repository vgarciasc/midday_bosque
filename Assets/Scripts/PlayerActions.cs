using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerActions : MonoBehaviour
{
    public GameObject fireballPrefab;

    PlayerMovement movement;
    PlayerInventory inventory;
    DialogManager dialog;

    public bool frozen = false;

    void Start() {
        movement = this.GetComponent<PlayerMovement>();
        inventory = this.GetComponent<PlayerInventory>();
        
        dialog = DialogManager.Get();

        dialog.set_active_event += SetFreeze;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && inventory.fireballNum > 0) {
            var obj = Instantiate(
                fireballPrefab,
                this.transform.position,
                Quaternion.identity,
                this.transform.parent
            );
            obj.GetComponent<Fireball>().Init(movement.GetDirection());
            inventory.Change("fireball", -1);
        }        
    }

    public void SetFreeze(bool value) {
        this.frozen = value;
    }
}
