using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemPowder : ChemMaterial
{
    [SerializeField]
    private GameObject explosionPrefab;

    public override void React(ChemElement element) {
        if (element.kind == ElementKind.FIRE) {
            element.Die();
            Die();
        }
    }

    protected override void Die() {
        if (isDead) return;
        
        isDead = true;
        Instantiate(explosionPrefab,
            this.transform.position,
            Quaternion.identity,
            this.transform.parent);
        Destroy(this.gameObject);
    }
}
