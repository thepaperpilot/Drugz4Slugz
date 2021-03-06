﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioReverbFilter))]
public class Slug : MonoBehaviour {
    
    public Image body;
    // TODO eyes priority for drugs
    public Image eyes;
    public Image mouth;
    public Image blush;
    public Image wings;
    public Image moustache;
    public Image shell;
    public Image ears;
    public Image tail;
    public Image whiskers;
    public Image salt;
    public GameObject flowers;
    public float minSize = .8f;
    public float maxSize = 1.2f;

    public Sprite deadEyes;
    public Sprite deadMouth;

    [HideInInspector]
    public new AudioSource audio;
    [HideInInspector]
    public AudioReverbFilter reverb;
    [HideInInspector]
    public HashSet<Drug.DrugState> drugs = new HashSet<Drug.DrugState>();

    void Awake() {
        audio = GetComponent<AudioSource>();
        reverb = GetComponent<AudioReverbFilter>();
    }

    public void Generate() {
        transform.localScale = Vector3.one * Random.Range(minSize, maxSize);
        Color color = Random.ColorHSV(0, 1, .5f, 1, 1, 1);
        foreach (Image image in GetComponentsInChildren<Image>(true))
            image.color = color;
        eyes.color = Color.white;
        blush.color = Color.white;
        salt.color = Color.white;
    }

    public void ApplyDrug(Drug drug) {
        Drug.DrugState state = drugs.Where(d => d.drug == drug).FirstOrDefault() ?? drug.GetDrugState(this);
        state.strength++;
        drug.Apply(state);
        if (!drugs.Contains(state)) drugs.Add(state);
    }

    public void Sleep() {
        foreach (Drug.DrugState state in drugs) {
            if (state.strength - state.resistance > state.drug.maxDosage) {
                DestroyImmediate(state.slug.gameObject);
                return;
            } else {
                int resist = Mathf.CeilToInt(state.strength / 4f);
                state.resistance += resist;
                state.strength -= resist;
                state.drug.Overnight(state);
            }
        }
        drugs = new HashSet<Drug.DrugState>(drugs.Where(d => d.strength > 0));
    }
}
