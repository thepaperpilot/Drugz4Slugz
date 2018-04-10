using UnityEngine;

public class Katfeine : Drug {

    public class KatState : DrugState {
        public Sprite originalEyes;
        public Sprite originalMouth;
    }

    public override DrugState GetDrugState(Slug slug) {
        return new KatState {
            drug = this,
            slug = slug
        };
    }

    public Sprite eyesSprite;
    public Sprite mouthSprite;

    public override void Apply(DrugState drugState) {
        KatState state = drugState as KatState;
        
        state.slug.ears.gameObject.SetActive(true);
        if (state.originalEyes == null) {
            state.originalEyes = state.slug.eyes.sprite;
            state.slug.eyes.sprite = eyesSprite;
        }
        if (state.originalMouth == null) {
            state.originalMouth = state.slug.mouth.sprite;
            state.slug.eyes.sprite = eyesSprite;
            state.slug.mouth.sprite = mouthSprite;
        }
    }

    public override void Overnight(DrugState drugState) {
        KatState state = drugState as KatState;

        state.overnightChange = false;
        if (state.strength >= 5) {
            if (!state.slug.tail.gameObject.activeSelf)
                state.overnightChange = true;
            state.slug.tail.gameObject.SetActive(true);
            state.slug.whiskers.gameObject.SetActive(true);
        } else if (state.strength == 0) {
            state.slug.tail.gameObject.SetActive(false);
            state.slug.whiskers.gameObject.SetActive(false);
            state.slug.ears.gameObject.SetActive(false);
            state.slug.eyes.sprite = state.originalEyes;
            state.slug.mouth.sprite = state.originalMouth;
        }
    }
}
