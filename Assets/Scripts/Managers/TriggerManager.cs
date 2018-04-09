using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerManager : MonoBehaviour {

    public static TriggerManager instance;

    public static Dictionary<string, Trigger> triggersDict;

    public float drugActivationDelay = 2f;
    public float randomChatMin = 1f;
    public float randomChatMax = 8f;

    private Trigger[] triggers;

    void Awake() {
        instance = this;
    }

    void Start () {
        triggersDict = CreateTriggersDict();
        triggers = CommentChainManager.commentChains.Select(chain => chain.trigger).Distinct().Select(t => triggersDict[t]).ToArray();
    }

    public void BeginRecording() {
        DayManager.BeginRecording();
        // Play random comment chain with a "FIRST" type
        ReadRandomChain(triggers
            .Where(t => t.type == Trigger.Type.FIRST)
            .OrderBy(t => Random.value)
            .FirstOrDefault());

        // Play overnight changes comment changes
        foreach (Drug.DrugState drug in DayManager.slugs.SelectMany(s => s.drugs).OrderBy(r => Random.value)) {
            ReadRandomChain(triggers
                .Where(t => t.type == Trigger.Type.OVERNIGHT && t.CheckValid(drug.slug, drug))
                .OrderBy(t => Random.value)
                .FirstOrDefault());
        }

        // Start rest of broadcast
        StartCoroutine(ActivateDrugs());
        StartCoroutine(RandomChat());
    }

    IEnumerator ActivateDrugs() {
        yield return new WaitForSeconds(drugActivationDelay);
        IEnumerable<Drug.DrugState> drugs = DayManager.slugs.SelectMany(s => s.drugs);
        foreach (Drug.DrugState drug in drugs.OrderBy(r => Random.value)) {
            // Activate drug
            drug.drug.Play(drug);

            // Chat's response
            IEnumerable<Trigger> filteredTriggers = triggers
                .Where(t => t.type == Trigger.Type.EVENT && t.CheckValid(drug.slug, drug));
            if (drug.strength - drug.resistance > drug.drug.maxDosage)
                filteredTriggers = filteredTriggers.Union(triggers
                    .Where(t => t.type == Trigger.Type.OVERDOSE && t.CheckValid(drug.slug, drug)));
            ReadRandomChain(filteredTriggers.OrderBy(t => Random.value).FirstOrDefault());

            // Wait before doing next drug activation
            yield return new WaitForSeconds(drugActivationDelay);
        }
        if (drugs.Count() < 2)
            yield return new WaitForSeconds(drugActivationDelay * (2 - drugs.Count()));
        
        StopAllCoroutines();
        DayManager.instance.endDayButton.gameObject.SetActive(true);
    }

    IEnumerator RandomChat() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(randomChatMin, randomChatMax));
            ReadRandomChain(triggers
                .Where(t => t.type == Trigger.Type.RANDOM)
                .OrderBy(t => Random.value)
                .FirstOrDefault());
        }
    }

    public static void ReadRandomChain(Trigger trigger) {
        if (trigger == null) return;

        CommentChain chain = CommentChainManager.commentChains
            .Where(c => triggersDict[c.trigger] == trigger)
            .OrderBy(c => Random.value)
            .FirstOrDefault();
        if (chain != null) chain.Read();
    }

    public static Dictionary<string, Trigger> CreateTriggersDict() {
        // Create our triggers menu
        Dictionary<string, Trigger> triggersDict = new Dictionary<string, Trigger>();
        triggersDict.Add("Filler/First", new Trigger { type = Trigger.Type.FIRST });
        triggersDict.Add("Filler/Random", new Trigger { type = Trigger.Type.RANDOM });
        triggersDict.Add("Events/Special/Cute Sneeze", new CuteSneezeTrigger());
        triggersDict.Add("Events/Overdose/Any", new Trigger { type = Trigger.Type.OVERDOSE });
        foreach (string drug in DrugManager.GetDrugs()) {
            triggersDict.Add("Events/Low Dose/" + drug, new DrugTrigger(drug));
            triggersDict.Add("Events/Medium Dose/" + drug, new DrugTrigger(drug, 5));
            triggersDict.Add("Events/High Dose/" + drug, new DrugTrigger(drug, 10));
            triggersDict.Add("Events/Overnight Change/" + drug, new OvernightChangeTrigger(drug));
            triggersDict.Add("Events/Overdose/" + drug, new OverdoseTrigger(drug));
        }
        return triggersDict;
    }
}
