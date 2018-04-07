using UnityEngine;

public class Pepper : Drug {

    public AudioClip sneezeSound;

    // Apply immediate effects
    public override void Apply(DrugState state) {

    }

    // Act while live
    public override void Play(DrugState state) {
        state.slug.audio.PlayOneShot(sneezeSound);
    }
}
