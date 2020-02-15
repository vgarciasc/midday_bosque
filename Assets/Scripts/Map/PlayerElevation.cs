using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElevation : MonoBehaviour
{
    public int currentLevel = 0;
    List<GameObject> stairs = new List<GameObject>();
    List<GameObject> goos = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D collider) {
        var obj = collider.gameObject;
        
        if (obj.CompareTag("Stair") && !stairs.Contains(obj)) {
            stairs.Add(obj);
        }
        else if (obj.CompareTag("Goo") && !goos.Contains(obj)) {
            goos.Add(obj);

            if (goos.Count == 1) {
                TogglePlayerCollision(LayerMask.NameToLayer("Walls"), false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.CompareTag("Stair") && stairs.Contains(obj)) {
            stairs.Remove(obj);

            var stair = obj.GetComponent<Stair>();
            Traverse(stair);
        }
        else if (obj.CompareTag("Goo") && goos.Contains(obj)) {
            goos.Remove(obj);

            if (goos.Count == 0) {
                TogglePlayerCollision(LayerMask.NameToLayer("Walls"), true);
            }
        }
    }

    void Traverse(Stair stair) {
        if (stair.IsObjGoingUp(this.gameObject)) {
            ChangeLevel(stair.GetTopLevel());
        } else if (stair.IsObjGoingDown(this.gameObject)) {
            ChangeLevel(stair.GetBaseLevel());
        } else {
            Debug.LogError("This should not be happening.");
            Debug.Break();
        }
    }

    void ChangeLevel(int newLevel) {
        if (newLevel == -1) return;

        TogglePlayerCollision(GetLevelLayerMask(this.currentLevel), true);
        TogglePlayerCollision(GetLevelLayerMask(newLevel), false);
        
        this.currentLevel = newLevel;
    }

    int GetLevelLayerMask(int level) {
        return LayerMask.NameToLayer("Floor_" + (level + 1).ToString());
    }

    void TogglePlayerCollision(int layer, bool value) {
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            layer,
            !value
        );
    }
}
