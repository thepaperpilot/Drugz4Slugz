public class Anime : Drug {

    // Apply immediate effects
    public override void Apply(DrugState state) {
        state.slug.audio.pitch *= 1.1f;
    }
}
