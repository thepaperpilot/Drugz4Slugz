using UnityEngine;

public class Pepper : Drug {

    public AudioClip sneezeSound;
    public Sprite sneezeMouth;
    public Sprite sneezeEyes;

    // Act while live
    public override void Play(DrugState state) {
        Sprite originalMouth = state.slug.mouth.sprite;
        Sprite originalEyes = state.slug.eyes.sprite;

        state.slug.mouth.sprite = sneezeMouth;
        state.slug.eyes.sprite = sneezeEyes;
        state.slug.audio.PlayOneShot(sneezeSound);

        DayManager.Delay(sneezeSound.length / state.slug.audio.pitch, delegate {
            state.slug.mouth.sprite = originalMouth;
            state.slug.eyes.sprite = originalEyes;
        });
    }
}
