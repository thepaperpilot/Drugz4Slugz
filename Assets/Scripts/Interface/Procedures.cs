using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Procedures : MonoBehaviour {

    public TextMeshProUGUI description;
    public GameObject slugPrefab;
    public GameObject dosePrefab;
    public Transform conditions;
    public VerticalLayoutGroup layoutGroup;

    public void Generate(Procedure procedure) {
        description.text = procedure.description;

        // Removes all children, skipping over description and "instructions" title
        for (int i = 2; i < conditions.childCount; i++) {
            Destroy(conditions.GetChild(i).gameObject);
        }

        GenerateSlug(procedure.firstSlug);
        GenerateSlug(procedure.secondSlug);
        GenerateSlug(procedure.thirdSlug);
        GenerateSlug(procedure.fourthSlug);

        DayManager.Delay(0, Layout);
    }

    void GenerateSlug(Procedure.Dose[] slug) {
        if (slug == null || slug.Length == 0) return;

        GameObject slugObject = Instantiate(slugPrefab, conditions);

        int i = 1;
        foreach (Procedure.Dose dose in slug) {
            GameObject doseObject = Instantiate(dosePrefab, slugObject.transform);
            doseObject.GetComponentInChildren<TextMeshProUGUI>().text = 
                "<b>" + i + ":</b> Apply " + dose.dosage + (dose.dosage == 1 ? " dose" : " doses") + 
                " of " + dose.drug + " to the slug.";
        }
    }

    void Layout() {
        layoutGroup.enabled = false;
        layoutGroup.enabled = true;
    }
}
