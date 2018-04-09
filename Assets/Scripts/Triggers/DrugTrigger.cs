public class DrugTrigger : Trigger {

    private string drugName;
    private int minDosage;

    public DrugTrigger(string drugName, int minDosage = 1) {
        type = Trigger.Type.EVENT;
        this.drugName = drugName;
        this.minDosage = minDosage;
    }

    public override bool CheckValid(Slug slug, Drug.DrugState state) {
        return state.drug.GetType().Name == drugName && state.strength >= minDosage;
    }
}
