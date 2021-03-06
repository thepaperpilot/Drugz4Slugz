﻿using System.Linq;
using UnityEngine;

public class Anime : Drug {

    public class AnimeState : DrugState {
        public Sprite originalEyes;
        public Sprite originalMouth;
    }

    public override DrugState GetDrugState(Slug slug) {
        return new AnimeState {
            drug = this,
            slug = slug
        };
    }

    public Sprite eyesSprite;
    public Sprite mouthSprite;

    public override void Apply(DrugState drugState) {
        AnimeState state = drugState as AnimeState;

        state.slug.audio.pitch *= 1.1f;
        state.slug.blush.gameObject.SetActive(true);
        state.slug.blush.transform.localScale = new Vector2(1 + state.strength / 100f, 1 + state.strength / 10f);
        if (state.originalEyes == null) {
            state.originalEyes = state.slug.eyes.sprite;
            state.slug.eyes.sprite = eyesSprite;
        }
        if (state.originalMouth == null) {
            state.originalMouth = state.slug.mouth.sprite;
            state.slug.mouth.sprite = mouthSprite;
            DrugState testosterone = state.slug.drugs.Where(d => d.drug.GetType().Name == "Testosterone").FirstOrDefault();
            state.slug.mouth.sprite = testosterone == null ? mouthSprite : (testosterone.drug as Testosterone).frown;
        }
    }

    public override void Play(DrugState state) {
        if (state.strength - state.resistance > maxDosage) {
            state.slug.flowers.SetActive(true);
            state.slug.flowers.transform.position = Camera.main.ScreenToWorldPoint(state.slug.transform.position + Vector3.forward * 20);
        }
    }

    public override void Overnight(DrugState drugState) {
        AnimeState state = drugState as AnimeState;

        state.overnightChange = false;
        if (state.strength >= 5) {
            if (!state.slug.wings.gameObject.activeSelf)
                state.overnightChange = true;
            state.slug.wings.gameObject.SetActive(true);
        } else if (state.strength == 0) {
            state.slug.wings.gameObject.SetActive(false);
            state.slug.blush.gameObject.SetActive(false);
            state.slug.eyes.sprite = state.originalEyes;
            state.slug.mouth.sprite = state.originalMouth;
        }
        state.slug.audio.pitch = Mathf.Pow(1.1f, state.strength);
        state.slug.blush.transform.localScale = new Vector2(1 + state.strength / 100f, 1 + state.strength / 10f);
    }
}
