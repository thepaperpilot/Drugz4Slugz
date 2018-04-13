using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Waste : Drug {

    public Drug[] drugs;
    private List<Drug> currentDrugs = new List<Drug>();

    public void Pickup() {
        if (DrugManager.selected == null) {
            IEnumerable<Drug> randomDrugs = drugs.OrderBy(d => Random.value);
            currentDrugs.Clear();
            // 50% chance of one drug
            // 25% chance of two drugs
            // 12.5% change of three
            // ...
            do {
                currentDrugs.Add(randomDrugs.ElementAt(currentDrugs.Count));
            } while (Random.value > .5f);
            DrugManager.instance.Pickup(this);
        } else {
            DrugManager.instance.Pickup(null);
        }
    }

    public override void Apply(DrugState drugState) {
        foreach (Drug drug in currentDrugs) {
            // Each drug gets applied random number of times
            // 50% chance of one dose
            // 25% chance of two doses
            // ...
            do {
                Debug.Log("Applying " + drug.GetType().Name);
                drugState.slug.ApplyDrug(drug);
            } while (Random.value > .25f);
        }
    }
}
