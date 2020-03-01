using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePowderSpawner : MonoBehaviour
{
    private List<Vector3> usedPositions = new List<Vector3>();

    [SerializeField]
    GameObject whitePowderPrefab;
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private float respawnInterval;

    void Start() {
        foreach (var powder in GameObject.FindObjectsOfType<WhitePowder>()) {
            if (TilemapHelper.InsideSameRoom(this.transform.position, powder.transform.position, RoomManager.Get().dimensions)) {
                RegisterPowder(powder);
            }
        }

        if (spawnPoints.Count == 0) {
            Debug.LogError("This shouldn't be happening.");
            Debug.Break();
        }
    }

    public void Respawn(GameObject destroyedPowder) {
        usedPositions.Remove(destroyedPowder.transform.position);
        Destroy(destroyedPowder);
        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer() {
        yield return new WaitForSeconds(respawnInterval);
        yield return new WaitWhile(() => TilemapHelper.InPlayerRoom(this.gameObject));
        yield return new WaitForSeconds(0.5f);
        Vector3 position = FindVacantPosition();
        var obj = Instantiate(whitePowderPrefab, position, Quaternion.identity, this.transform.parent);
        RegisterPowder(obj.GetComponent<WhitePowder>());
    }

    Vector3 FindVacantPosition() {
        return spawnPoints.Find((sp) => !usedPositions.Contains(sp.position)).position;
    }

    void RegisterPowder(WhitePowder powder) {
        powder.acquisitionEvent += Respawn;
        usedPositions.Add(powder.transform.position);
    }
}
