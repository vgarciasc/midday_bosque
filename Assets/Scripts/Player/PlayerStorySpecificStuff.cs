using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStorySpecificStuff : MonoBehaviour
{
    [SerializeField]
    RuntimeAnimatorController normalAnimator;
    [SerializeField]
    RuntimeAnimatorController fairyAnimator;
    [HideInInspector]
    public bool isFairy = false;

    void Start() {
        // ToggleFairy(true);
    }

    public void ToggleFairy(bool value) {
        this.isFairy = value;
        this.GetComponent<Animator>().runtimeAnimatorController = isFairy ? fairyAnimator : normalAnimator;

        if (isFairy) {
            InventoryManager.Get().LoseAllItems();
        }
    }
}
