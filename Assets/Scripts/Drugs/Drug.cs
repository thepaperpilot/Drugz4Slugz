using UnityEngine;

public abstract class Drug : MonoBehaviour {

    public Texture2D cursorImage;

    public class DrugState {
        public int strength;
        public Drug drug;
        public Slug slug;
    }

    public virtual DrugState GetDrugState(Slug slug) {
        return new DrugState {
            strength = 0,
            drug = this,
            slug = slug
        };
    }

    // Apply immediate effects
    public virtual void Apply(DrugState state) {

    }

    // Act while live
    public virtual void Play(DrugState state) {

    }

    // Act overnight
    public virtual void Overnight(DrugState state) {

    }
}
