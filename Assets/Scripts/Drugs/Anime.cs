using UnityEngine;

public class Anime : Drug {
    
    public override void Apply(DrugState state) {
        state.slug.audio.pitch *= 1.1f;
        state.slug.blush.gameObject.SetActive(true);
        state.slug.blush.transform.localScale = Vector2.one * (1 + state.strength / 10f);
    }

    public override void Overnight(DrugState state) {
        if (state.strength >= 5) {
            state.slug.wings.gameObject.SetActive(true);
            state.slug.wings.transform.localScale = Vector2.one * (1 + (state.strength - 5) / 5f);
        }
    }
}
