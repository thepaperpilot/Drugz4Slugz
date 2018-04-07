using UnityEngine;

public abstract class Drug : MonoBehaviour {

    public class DrugState {
        public int strength;
    }

    public abstract DrugState GetDrugState();

    // Apply immediate effects
    public virtual void Apply(Slug slug, DrugState state) {

    }

    // Act while live
    public virtual void Play(Slug slug, DrugState state) {

    }
}
