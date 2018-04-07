public class DrugTrigger : Trigger {

    private string drugName;

    public DrugTrigger(string drugName) {
        type = Trigger.Type.EVENT;
        adviceRating = 0;
        this.drugName = drugName;
    }

    public override bool CheckValid(Slug slug, Drug.DrugState state) {
        return state.drug.GetType().Name == drugName;
    }
}
