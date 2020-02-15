using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChemFire : ChemElement
{
    Sequence animationSequence;
    Vector3 originalScale;

    public GameObject firePrefab;

    protected new void Start() {
        base.Start();
        originalScale = this.transform.localScale;

        StartCoroutine(Spread());
    }

    protected new void FixedUpdate() {
        base.FixedUpdate();
        if (health >= 1) {
            Spread();
        }
    }

    protected override void HandleLife() {
        base.HandleLife();
        this.health -= Time.deltaTime;
    }

    protected override void React(List<ChemMaterial> materials) {
        bool beingKindled = false;

        foreach (var material in materials) {
            material.React(this);
            if (material.inflammable) {
                this.health += Time.deltaTime;
                beingKindled = true;
            }
        }

        if (beingKindled && animationSequence == null) {
            StartAnimations();
        } else if (!beingKindled && animationSequence != null) {
            StopAnimations();
        }
    }

    protected override void StartAnimations() {
        animationSequence = DOTween.Sequence();
        animationSequence.Append(this.transform.DOScale(this.transform.localScale * 1.1f, 0.2f).SetEase(Ease.Linear));
        animationSequence.Append(this.transform.DOScale(this.transform.localScale / 1.1f, 0.2f).SetEase(Ease.Linear));
        animationSequence.Play().SetLoops(-1);
    }

    protected override void StopAnimations() {
        // animationSequence.Kill();
        // animationSequence = null;
        // this.transform.localScale = originalScale;
    }

    protected override void Die() {
        animationSequence.Kill();
        base.Die();
    }

    IEnumerator Spread() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            var adjs = HushPuppy.GetAdjacentTiles(this.transform.position);
            
            var currentRoom = roomManager.GetCurrentRoom();
            var rooms = new List<Room>() { currentRoom };
            rooms.AddRange(roomManager.GetAdjacentRooms(currentRoom));

            var objs = new List<GameObject>();
            foreach (var adj in adjs) {
                foreach (var room in rooms) {
                    objs.AddRange(room.GetGameObjectsAtTile(adj));
                }
            }

            var materials = new List<ChemMaterial>();
            var containedMaterials = ChemHelp.GetMaterialsFromContainers();
            materials.AddRange(
                containedMaterials.FindAll((f) => 
                    adjs.Contains(f.transform.position)));
            objs.ForEach((f) => {
                var material = f.GetComponent<ChemMaterial>();
                if (material != null) {
                    materials.Add(material);
                }
            });
            
            // var fires = objs.FindAll((obj) => obj.GetComponent<ChemFire>() != null);
            var fires = new List<ChemFire>(GameObject.FindObjectsOfType<ChemFire>());

            foreach (var material in materials) {
                if (material.inflammable
                    && fires.Find((f) => f.transform.position == material.transform.position) == null) {
                        Instantiate(firePrefab, material.transform.position, Quaternion.identity, transform.parent);
                }
            }
        }
    }
}
