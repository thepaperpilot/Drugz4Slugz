using UnityEngine;

public class Salt : Drug {

    public float colorMod = .7f;
    public float colorDuration = 1;
    
    public override void Play(DrugState state) {
        Color color = state.slug.body.color;
        state.slug.body.CrossFadeColor(new Color(color.r * colorMod, color.g * colorMod, color.b * colorMod), colorDuration, false, false);
        state.slug.salt.gameObject.SetActive(true);
    }
}
