using UnityEngine;

public class Testosterone : Drug {

    public AudioReverbPreset lowDose;
    public AudioReverbPreset medDose;
    public AudioReverbPreset highDose;

    public override void Apply(DrugState state) {
        state.slug.reverb.enabled = true;
        state.slug.reverb.reverbPreset =
            state.strength < 5 ? lowDose :
            state.strength < 10 ? medDose :
            highDose;
    }

    public override void Overnight(DrugState state) {
        state.slug.reverb.enabled = state.strength > 0;
        state.slug.reverb.reverbPreset =
            state.strength < 5 ? lowDose :
            state.strength < 10 ? medDose :
            highDose;
    }
}
