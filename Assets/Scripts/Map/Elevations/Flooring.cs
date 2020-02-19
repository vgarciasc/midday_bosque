using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flooring : MonoBehaviour {
    public int currentLevel = 0;
    [SerializeField]
    private List<GameObject> stairs = new List<GameObject>();

    void OnEnable() {
        this.currentLevel = TilemapHelper.GetFloor(this.transform.position);
        ChangeLevel(this.currentLevel);
    }

    private void ChangeLevel(int level) {
        if (level == -1) return;

        this.currentLevel = level;
        string levelLayer = "Floor_" + level.ToString();
        this.gameObject.layer = LayerMask.NameToLayer(levelLayer);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.CompareTag("Stair") && !stairs.Contains(obj)) {
            stairs.Add(obj);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.CompareTag("Stair") && stairs.Contains(obj)) {
            stairs.Remove(obj);

            var stair = obj.GetComponent<Stair>();
            Traverse(stair);
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
}
