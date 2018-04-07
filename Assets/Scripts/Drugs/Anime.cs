public class Anime : Drug {

    // Apply immediate effects
    public override void Apply(Slug slug, DrugState state) {
        slug.audio.pitch *= 1.1f;
    }
}
