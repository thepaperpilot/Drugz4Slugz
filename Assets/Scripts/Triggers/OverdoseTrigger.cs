public class OverdoseTrigger : Trigger {

    private string drugName;

    public OverdoseTrigger(string drugName) {
        type = Trigger.Type.OVERDOSE;
        this.drugName = drugName;
    }

    public override bool CheckValid(Slug slug, Drug.DrugState state) {
        return state.drug.GetType().Name == drugName;
    }
}
