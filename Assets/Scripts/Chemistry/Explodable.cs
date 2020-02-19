using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : ChemMaterial
{
    [SerializeField]
    private GameObject blockedSide;
    [SerializeField]
    private GameObject unblockedSide;

    public override void React(ChemElement element) {
        if (element.kind == ElementKind.EXPLOSION) {
            Die();
        }
    }

    protected override void Die() {
        if (isDead) return;
        
        isDead = true;
        blockedSide.SetActive(false);
        unblockedSide.SetActive(true);
    }
}
