using UnityEngine;

public class OvernightChangeTrigger : Trigger {

    private string drugName;

    public OvernightChangeTrigger(string drugName) {
        type = Trigger.Type.OVERNIGHT;
        this.drugName = drugName;
    }

    public override bool CheckValid(Slug slug, Drug.DrugState state) {
        return state.drug.GetType().Name == drugName && state.overnightChange;
    }
}
