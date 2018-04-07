using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerManager : MonoBehaviour {

    public static TriggerManager instance;

    public static Dictionary<string, Trigger> triggersDict;

    public float drugActivationDelay = 2f;
    public Transform slugEnclosures;

    private Trigger[] triggers;

    void Awake() {
        instance = this;
    }

    void Start () {
        triggersDict = CreateTriggersDict();
        triggers = CommentChainManager.commentChains.Select(chain => chain.trigger).Distinct().Select(t => triggersDict[t]).ToArray();
    }

    public void BeginRecording() {
        // Play random comment chain with a "FIRST" type
        ReadRandomChain(triggers
            .Where(t => t.type == Trigger.Type.FIRST)
            .OrderBy(t => Random.value)
            .FirstOrDefault());

        // Start rest of broadcast
        StartCoroutine(ActivateDrugs());
    }

    IEnumerator ActivateDrugs() {
        // Temporary way of getting list of all slugs
        Slug[] slugs = slugEnclosures.GetComponentInParent<Canvas>().GetComponentsInChildren<Slug>();

        yield return new WaitForSeconds(drugActivationDelay);
        foreach (Drug.DrugState drug in slugs.SelectMany(s => s.drugs).OrderBy(r => Random.value)) {
            // Activate drug
            drug.drug.Play(drug);

            // Chat's response
            ReadRandomChain(triggers
                .Where(t => t.type == Trigger.Type.EVENT && t.CheckValid(drug.slug, drug))
                .OrderBy(t => Random.value)
                .FirstOrDefault());

            // Wait before doing next drug activation
            yield return new WaitForSeconds(drugActivationDelay);
        }

        Debug.Log("Finished broadcast");
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
        triggersDict.Add("Filler/First", new Trigger { type = Trigger.Type.FIRST, adviceRating = 1 });
        triggersDict.Add("Events/Pepper", new DrugTrigger("Pepper"));
        triggersDict.Add("Events/Cute Sneeze", new CuteSneezeTrigger());
        return triggersDict;
    }
}
