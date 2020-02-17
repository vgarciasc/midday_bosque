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
            powder.GetComponent<WhitePowder>().acquisitionEvent.AddListener(Respawn);
            usedPositions.Add(powder.transform.position);
        }

        if (spawnPoints.Count == 0) {
            Debug.LogError("This shouldn't be happening.");
            Debug.Break();
        }
    }

    public void Respawn() {
        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer() {
        yield return new WaitForSeconds(respawnInterval);
        Vector3 position = FindVacantPosition();
        Instantiate(whitePowderPrefab, position, Quaternion.identity, this.transform.parent);
        usedPositions.Add(position);
    }

    Vector3 FindVacantPosition() {
        return spawnPoints.Find((sp) => !usedPositions.Contains(sp.position)).position;
    }
}
