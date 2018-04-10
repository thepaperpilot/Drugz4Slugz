using System.Linq;
using UnityEngine;

public class Testosterone : Drug {

    public class TestosteroneState : DrugState {
        public Sprite originalEyes;
        public Sprite originalMouth;
    }

    public override DrugState GetDrugState(Slug slug) {
        return new TestosteroneState {
            drug = this,
            slug = slug
        };
    }

    public AudioReverbPreset lowDose;
    public AudioReverbPreset medDose;
    public AudioReverbPreset highDose;

    public Sprite eyes;
    public Sprite smile;
    public Sprite frown;

    public override void Apply(DrugState drugState) {
        TestosteroneState state = drugState as TestosteroneState;

        state.slug.reverb.enabled = true;
        state.slug.reverb.reverbPreset =
            state.strength < 5 ? lowDose :
            state.strength < 10 ? medDose :
            highDose;

        state.slug.moustache.gameObject.SetActive(true);

        if (state.originalEyes == null) {
            state.originalEyes = state.slug.eyes.sprite;
            state.slug.eyes.sprite = eyes;
        }
        if (state.originalMouth == null) {
            state.originalMouth = state.slug.mouth.sprite;
            state.slug.mouth.sprite = state.slug.drugs.Where(d => d.drug.GetType().Name == "Anime").Count() > 0 ? frown : smile;
        }
    }

    public override void Overnight(DrugState drugState) {
        TestosteroneState state = drugState as TestosteroneState;

        state.slug.reverb.enabled = state.strength > 0;
        state.slug.reverb.reverbPreset =
            state.strength < 5 ? lowDose :
            state.strength < 10 ? medDose :
            highDose;

        if (state.strength >= 5) {
            state.slug.shell.gameObject.SetActive(true);
        } else if (state.strength == 0) {
            state.slug.moustache.gameObject.SetActive(true);
            state.slug.eyes.sprite = state.originalEyes;
            state.slug.mouth.sprite = state.originalMouth;
            state.slug.shell.gameObject.SetActive(false);
        }
    }
}
