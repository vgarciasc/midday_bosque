using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Room))]
[CanEditMultipleObjects]
public class RoomEditor : Editor
{
	public override void OnInspectorGUI() {
		DrawDefaultInspector();
		Room room = (Room) target;

		if (GUILayout.Button("Set as Initial")) {
			RoomManager.Get().firstRoom = room;
		}
	}
}
