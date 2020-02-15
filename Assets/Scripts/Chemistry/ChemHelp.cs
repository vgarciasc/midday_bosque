using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemHelp : MonoBehaviour
{
    public static List<ChemMaterial> GetMaterialsFromContainers() {
        var output = new List<ChemMaterial>();
        foreach (var cont in GameObject.FindGameObjectsWithTag("MaterialContainer")) {
            foreach (Transform child in cont.transform) {
                var material = child.GetComponent<ChemMaterial>();
                if (material != null) {
                    output.Add(material);
                }
            }
        }
        return output;
    }
}
