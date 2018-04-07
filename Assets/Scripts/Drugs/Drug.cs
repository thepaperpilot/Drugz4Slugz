using UnityEngine;

public abstract class Drug : MonoBehaviour {

    public Texture2D cursorImage;

    public class DrugState {
        public int strength;
    }

    public virtual DrugState GetDrugState() {
        return new DrugState {
            strength = 1
        };
    }

    // Apply immediate effects
    public virtual void Apply(Slug slug, DrugState state) {

    }

    // Act while live
    public virtual void Play(Slug slug, DrugState state) {

    }
}
