using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FruitTreeFruitionState {
    public Sprite sprite;
    public int fruitionLevel;
}

public class FruitTree : MonoBehaviour, PlayerInteractive {
    public List<FruitTreeFruitionState> states = new List<FruitTreeFruitionState>();

    FruitTreeFruitionState currentState;
    SpriteRenderer sr;

    [SerializeField]
    int startFruitionLevel = 2;
    [SerializeField]
    float fruitionStep = 0.5f;

    void Start() {
        sr = this.GetComponent<SpriteRenderer>();

        ChangeState(startFruitionLevel);
    }

    public void OnInteraction(GameObject player) {
        if (currentState.fruitionLevel != 2) return;

        InventoryManager.Get().Change(ItemsEnum.FIRE_FRUIT, 1);
        ChangeState(0);
    }

    void ChangeState(int newFruitionLevel) {
        currentState = states.Find((state) => state.fruitionLevel == newFruitionLevel);
        if (currentState == null) {
            Debug.LogError("This shouldn't have happened.");
            Debug.Break();
        }

        sr.sprite = currentState.sprite;
        StartCoroutine(GrowFruit());
    }

    IEnumerator GrowFruit() {
        int nextFruitionLevel = currentState.fruitionLevel + 1;
        var nextState = states.Find((state) => state.fruitionLevel == nextFruitionLevel);
        if (nextState == null) {
            yield break;
        }

        yield return new WaitForSeconds(fruitionStep);
        ChangeState(nextFruitionLevel);
    }
}
