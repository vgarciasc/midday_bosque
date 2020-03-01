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

    Vector3 shakeOriginalPos;

    CameraFocus cameraFocus;
    RoomManager roomManager;

    void Start() {
        cameraFocus = GameObject.FindObjectOfType<CameraFocus>();
        roomManager = RoomManager.Get();
        shakeOriginalPos = this.transform.parent.localPosition;

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

            roomManager.RegisterChange(roomCenter);
        }
    }

    public void ScreenShake(float power, int frames) { StartCoroutine(ScreenShake_Coroutine(power, frames)); }
    IEnumerator ScreenShake_Coroutine(float power, int frames) {
        if (power < 0.05f) {
            power = 0.1f;
        }

        for (int i = 0; i < frames; i++) {
            yield return new WaitForEndOfFrame();
            float x = GetScreenShakeDistance(power);
            float y = GetScreenShakeDistance(power);

            this.transform.parent.localPosition = new Vector3(shakeOriginalPos.x + x,
                                                       shakeOriginalPos.y + y,
                                                       shakeOriginalPos.z);
        }

        this.transform.parent.localPosition = shakeOriginalPos;
    }

    float GetScreenShakeDistance(float power) {
        float power_aux = power;
        int count = 0;
        while (true) {
            count++;
            float aux = Mathf.Pow(-1, Random.Range(0, 2)) * Random.Range(power_aux / 4, power_aux / 2);
            if (Mathf.Abs(aux) > 0.1f) {
                return aux;
            }
            if (count > 5) {
                count = 0;
                power_aux += 0.25f;
            }
        }
    }
}
