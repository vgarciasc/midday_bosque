using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButtonObject : MonoBehaviour
{
    public bool remainActive = false;
    public List<RoomButton> roomButtons = new List<RoomButton>();
    
    public virtual void Toggle(bool value) {
        this.gameObject.SetActive(value);
    }

    protected bool IsEveryButtonActive() {
        foreach (var btn in roomButtons) {
            if (!btn.active) {
                return false;
            }
        }
        return true;
    }
}
