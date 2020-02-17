using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Get() {
        return (RoomManager) HushPuppy.safeFindComponent("GameController", "RoomManager");
    }

    public int currentRoomIdx = -1;
    public List<Room> rooms = new List<Room>();
    public Room firstRoom;

    public Vector3 dimensions = new Vector2(10, 10);

    void Start()
    {
        rooms = new List<Room>(GameObject.FindObjectsOfType<Room>());
        foreach (var room in rooms) {
            room.index = rooms.IndexOf(room);
        }

        currentRoomIdx = firstRoom.index;
        // DeactivateRooms();
        
        // Camera.main.transform.position += firstRoom.transform.position;
        // GameObject.FindObjectOfType<Player>().transform.position = firstRoom.transform.position;
    }

    public void RegisterChange(Vector3 direction) {
        foreach (var room in rooms) {
            if (room.transform.position - rooms[currentRoomIdx].transform.position == direction) {
                currentRoomIdx = room.index;
                // DeactivateRooms();
                return;
            }
        }

        Debug.LogError("This should not be happening");
    }

    void DeactivateRooms() {
        rooms.ForEach((room) => {
            room.gameObject.SetActive(room.index == currentRoomIdx);
        });
    }

    public bool IsInCurrentRoomLimits(Vector3 position) {
        if (currentRoomIdx == -1) return false;
        var currentRoomCenter = rooms[currentRoomIdx].transform.position;
        var diff = currentRoomCenter - position;

        return diff.x > (- dimensions.x/2)
            && diff.y > (- dimensions.y/2)
            && diff.x < (+ dimensions.x/2)
            && diff.y < (+ dimensions.y/2);
    }

    public Room GetCurrentRoom() {
        if (currentRoomIdx == -1) return firstRoom;
        return rooms[currentRoomIdx];
    }

    public List<Room> GetAdjacentRooms(Room room) {
        var adjs = new List<Room>();
        foreach (var f in rooms) {
            var diff = f.transform.position - room.transform.position;
            if (diff == Vector3.left * dimensions.x || diff == Vector3.right * dimensions.x
                || diff == Vector3.up  * dimensions.y || diff == Vector3.down  * dimensions.y) {
                adjs.Add(f);
            }
        }
        return adjs;
    }

    public static int RoomDistance(Vector2 r1, Vector2 r2, Vector2 dim) {
        return Mathf.CeilToInt(Mathf.Abs(r1.x - r2.x) / dim.x)
            + Mathf.CeilToInt(Mathf.Abs(r1.y - r2.y) / dim.y);
    }
}
