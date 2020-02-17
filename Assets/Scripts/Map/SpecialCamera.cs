using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpecialCamera : MonoBehaviour
{
    public GameObject letterboxTilemap;

    [SerializeField]
    [Range(0f, 1f)]
    float transitionDuration = 0.2f;

    Vector3 offset;
    Vector3 roomCenter;
    Vector3 roomSize;

    CameraFocus cameraFocus;
    RoomManager roomManager;

    void Start() {
        cameraFocus = GameObject.FindObjectOfType<CameraFocus>();
        roomManager = RoomManager.Get();

        Init();
    }

    void Init() {
        var firstRoom = RoomManager.Get().firstRoom;
        this.offset = this.transform.position;

        cameraFocus.transform.position = 
            firstRoom.startingPos == null ? 
                firstRoom.transform.position : 
                firstRoom.startingPos.transform.position;
    }

    void Update() {
        // Vector3 changePos = Vector3.zero;

        // if (cameraFocus.transform.position.x - roomCenter.x > (roomManager.dimensions.x/2)) {
        //     changePos.x = +roomManager.dimensions.x;
        // } else if (cameraFocus.transform.position.x - roomCenter.x < -(roomManager.dimensions.x/2)) {
        //     changePos.x = -roomManager.dimensions.x;
        // }

        // if (cameraFocus.transform.position.y - roomCenter.y > (roomManager.dimensions.y/2)) {
        //     changePos.y = +roomManager.dimensions.y;
        // } else if (cameraFocus.transform.position.y - roomCenter.y < -(roomManager.dimensions.y/2)) {
        //     changePos.y = -roomManager.dimensions.y;
        // }

        // roomCenter += changePos;
        // letterboxTilemap.transform.position += changePos;
        
        // this.transform.position = roomCenter + offset;
        // roomManager.RegisterChange(changePos);
        HandleRoomChange();
    }

    Vector3 lastRoomCenter;

    void HandleRoomChange() {
        var roomCenter = TilemapHelper.GetRoomCenter(
            this.cameraFocus.transform.position,
            roomManager.dimensions);

        if (lastRoomCenter != roomCenter) {
            var roomDistance = RoomManager.RoomDistance(lastRoomCenter, roomCenter, roomManager.dimensions) ;
            
            float dur = roomDistance == 1 ? transitionDuration : 0f;
            
            this.transform.DOMove(roomCenter + offset, dur);
            letterboxTilemap.transform.DOMove(roomCenter, dur);
            lastRoomCenter = roomCenter;
        }
    }
}
