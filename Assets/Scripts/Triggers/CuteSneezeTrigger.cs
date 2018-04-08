using System.Linq;

public class CuteSneezeTrigger : DrugTrigger {

    public CuteSneezeTrigger() : base("Pepper") {}

    public override bool CheckValid(Slug slug, Drug.DrugState state) {
        if (!base.CheckValid(slug, state)) return false;

        Drug.DrugState anime = slug.drugs.Where(d => d.drug.GetType().Name == "Anime").FirstOrDefault();
        if (anime == null) return false;

        return anime.strength > 2;
    }
}
