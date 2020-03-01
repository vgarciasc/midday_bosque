using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutsceneTriggerKind {
    NONE, FAIRY_WATER, SICK_KID_END, ENTER_FAIRY_FOUNTAIN
}

public class CutsceneTrigger : MonoBehaviour
{
    bool active;
    [SerializeField]
    CutsceneTriggerKind kind;

    void OnTriggerEnter2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.CompareTag("Player")) {
            var player = obj.GetComponent<PlayerStorySpecificStuff>();
            if (player != null) {
                HandleTrigger(player);
            }
        }
    }

    void HandleTrigger(PlayerStorySpecificStuff player) {
        if (active) return;
        
        switch (kind) {
            case CutsceneTriggerKind.SICK_KID_END:
                if (player.isFairy) {
                    StartCoroutine(CutsceneManager.Get().EndingGame());
                    active = true;
                }
                break;
            case CutsceneTriggerKind.FAIRY_WATER:
                if (!player.isFairy) {
                    StartCoroutine(CutsceneManager.Get().TurnFairy(player));
                    active = true;
                }
                break;
            case CutsceneTriggerKind.ENTER_FAIRY_FOUNTAIN:
                CutsceneManager.Get().EnterFairyFountain();
                active = true;
                break;
        }
    }
}
