using UnityEngine;

public class Pepper : Drug {

    public AudioClip sneezeSound;

    // Apply immediate effects
    public override void Apply(Slug slug, DrugState state) {
        slug.audio.PlayOneShot(sneezeSound);
    }
}
