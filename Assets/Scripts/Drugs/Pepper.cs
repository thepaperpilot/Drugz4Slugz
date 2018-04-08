using UnityEngine;

public class Pepper : Drug {

    public AudioClip sneezeSound;

    // Act while live
    public override void Play(DrugState state) {
        state.slug.audio.PlayOneShot(sneezeSound);
    }
}
