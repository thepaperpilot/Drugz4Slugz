using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Slug : MonoBehaviour {
    
    public Image body;
    public float minSize = .8f;
    public float maxSize = 1.2f;

    [HideInInspector]
    public new AudioSource audio;

    private List<Drug.DrugState> drugs = new List<Drug.DrugState>();

    void Awake() {
        audio = GetComponent<AudioSource>();
    }

    public void Generate() {
        transform.localScale = Vector3.one * Random.Range(minSize, maxSize);
        body.color = Random.ColorHSV(0, 1, .5f, 1, 1, 1);
    }

    public void ApplyDrug(Drug drug) {
        Drug.DrugState state = drug.GetDrugState();
        drug.Apply(this, state);
        drugs.Add(state);
    }
}
