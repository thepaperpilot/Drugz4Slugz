using UnityEngine;

public class Anime : Drug {
    
    public override void Apply(DrugState state) {
        state.slug.audio.pitch *= 1.1f;
        state.slug.blush.gameObject.SetActive(true);
        state.slug.blush.transform.localScale = Vector2.one * (1 + state.strength / 10f);
    }

    public override void Overnight(DrugState state) {
        state.overnightChange = false;
        if (state.strength >= 5) {
            if (!state.slug.wings.gameObject.activeSelf)
                state.overnightChange = true;
            state.slug.wings.transform.localScale = Vector2.one * (1 + (state.strength - 5) / 5f);
            state.slug.wings.gameObject.SetActive(true);
        } else if (state.strength == 0) {
            state.slug.wings.gameObject.SetActive(false);
            state.slug.blush.gameObject.SetActive(false);
        }
        state.slug.audio.pitch = Mathf.Pow(1.1f, state.strength);
        state.slug.blush.transform.localScale = Vector2.one * (1 + state.strength / 10f);
    }
}
