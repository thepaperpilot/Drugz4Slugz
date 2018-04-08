using UnityEngine;

public abstract class Drug : MonoBehaviour {

    public Texture2D cursorImage;
    public int maxDosage = 10;

    public class DrugState {
        public int strength = 0;
        public int resistance = 0;
        public Drug drug;
        public Slug slug;
    }

    public virtual DrugState GetDrugState(Slug slug) {
        return new DrugState {
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
