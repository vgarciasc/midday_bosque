using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerItemUse))]
public class PlayerActions : MonoBehaviour
{
    PlayerMovement movement;
    PlayerItemUse itemUse;

    public InputAction fireAction;
    public InputAction rollAction;
    public InputAction changeItemAction;

    void Start() {
        movement = this.GetComponent<PlayerMovement>();
        itemUse = this.GetComponent<PlayerItemUse>();
        
        DialogManager.Get().setActiveEvent += SetFreeze;

        fireAction.performed += _ => Fire();
        fireAction.Enable();
        rollAction.performed += _ => Roll();
        rollAction.Enable();
        changeItemAction.performed += _ => ChangeItem();
        changeItemAction.Enable();
    }

    public void SetFreeze(bool value) {
        movement.SetActive(!value);
        itemUse.SetActive(!value);
    }

    #region Fire
    private void Fire() {
        if (CanInteract()) {
            Interact();
        } else {
            itemUse.UseItem();
        }
    }

    private List<PlayerInteractive> GetInteractions() {
        var playerColls = new List<BoxCollider2D>(this.GetComponentsInChildren<BoxCollider2D>());
        BoxCollider2D playerColl = playerColls.Find((f) => !f.isTrigger);
        Vector3 playerCenter = playerColl.bounds.center;

        var hits = Physics2D.RaycastAll(
            playerCenter,
            movement.lastDirection,
            1f);

        var interactions = new List<PlayerInteractive>();
        foreach (var hit in hits) {
            var interactive = hit.transform.GetComponent<PlayerInteractive>();
            if (interactive != null) {
                interactions.Add(interactive);
            }
        }

        return interactions;
    }

    private bool CanInteract() {
        return GetInteractions().Count != 0;
    }

    private void Interact() {
        GetInteractions()[0].OnInteraction(this.gameObject);
    }
    #endregion Fire

    private void Roll() {
        movement.Roll();
    }

    private void ChangeItem() {
        itemUse.SelectNextItem();
    }
}
