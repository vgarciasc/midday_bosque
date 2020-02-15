using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TeleportDoor : Stair
{
    [SerializeField]
    Room destinationRoom;
    [SerializeField]
    Transform destinationPos;

    bool isTeleporting = false;

    void OnTriggerEnter2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.CompareTag("Player") && !isTeleporting) {
            isTeleporting = true;
            StartCoroutine(TeleportPlayer(obj));
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        var obj = collider.gameObject;
        if (obj.CompareTag("Player")) {
            isTeleporting = false;
        }
    }

    IEnumerator TeleportPlayer(GameObject player) {
        player.GetComponent<PlayerActions>().SetFreeze(true);
        
        var fadeManager = FadeManager.Get();
        yield return fadeManager.Fade(1f);
        if (destinationRoom != null) {
            player.transform.position = destinationRoom.startingPos.transform.position;
        } else if (destinationPos) {
            player.transform.position = destinationPos.transform.position;
        }
        yield return fadeManager.Fade(0f);

        player.GetComponent<PlayerActions>().SetFreeze(false);
    }

    public override int GetTopLevel() {
        Vector3Int targetTileInt = Vector3Int.zero;
        int maxLevel = -1;

        if (destinationRoom != null) {
            targetTileInt = HushPuppy.GetVecAsTileVec(destinationRoom.startingPos.transform.position);
        } else if (destinationPos) {
            targetTileInt = HushPuppy.GetVecAsTileVec(destinationPos.transform.position);
        }

        var tilemaps = new List<Tilemap>(FindObjectsOfType<Tilemap>());
        foreach (var tilemap in tilemaps) {
            string tilemapLayerName = LayerMask.LayerToName(tilemap.gameObject.layer);
            if (!tilemapLayerName.Contains("Floor")) {
                continue;
            }

            int tilemapLevel = int.Parse(tilemapLayerName.Split('_')[1]);
            tilemapLevel -= 1; //in the layers, we start counting on 1
            
            var targetTile = tilemap.GetTile(targetTileInt);

            if (targetTile != null) {
                maxLevel = tilemapLevel > maxLevel ? tilemapLevel : maxLevel;
            }
        }

        return maxLevel;
    }

    public override bool IsObjGoingUp(GameObject obj) {
        return true;
    }
}
