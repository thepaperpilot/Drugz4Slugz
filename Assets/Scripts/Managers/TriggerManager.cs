using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggerManager : MonoBehaviour {

    public static TriggerManager instance;

    public static Dictionary<string, Trigger> triggersDict;

    public float drugActivationDelay = 2f;
    public float randomChatMin = 1f;
    public float randomChatMax = 8f;
    public Transform slugEnclosures;
    public Animator fade;
    public Button liveButton;
    public Button endDayButton;

    private Trigger[] triggers;

    void Awake() {
        instance = this;
    }

    void Start () {
        triggersDict = CreateTriggersDict();
        triggers = CommentChainManager.commentChains.Select(chain => chain.trigger).Distinct().Select(t => triggersDict[t]).ToArray();
    }

    public void BeginRecording() {
        liveButton.interactable = false;
        // Play random comment chain with a "FIRST" type
        ReadRandomChain(triggers
            .Where(t => t.type == Trigger.Type.FIRST)
            .OrderBy(t => Random.value)
            .FirstOrDefault());

        // Start rest of broadcast
        StartCoroutine(ActivateDrugs());
        StartCoroutine(RandomChat());
    }

    public void NewDay() {
        foreach (Slug slug in slugEnclosures.GetComponentsInChildren<Slug>())
            foreach (Drug.DrugState state in slug.drugs)
                state.drug.Overnight(state);
        foreach (Transform enclosure in slugEnclosures) {
            if (enclosure.childCount > 3) {
                if (enclosure.GetChild(2).gameObject.activeSelf) {
                    enclosure.GetChild(2).gameObject.SetActive(false);
                    enclosure.GetChild(3).gameObject.SetActive(true);
                    enclosure.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text =
                        enclosure.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text;
                }
            } else {
                enclosure.GetChild(0).gameObject.SetActive(true);
                enclosure.GetChild(1).gameObject.SetActive(false);
                enclosure.GetChild(2).gameObject.SetActive(false);
            }
        }
        liveButton.interactable = true;
        endDayButton.gameObject.SetActive(false);
        endDayButton.GetComponentInParent<Canvas>().GetComponent<CanvasAnimator>().Reset();
        CommentChainManager.Reset();
        fade.SetFloat("Speed", -1);
    }

    public void EndDay() {
        CommentChainManager.instance.StopAllCoroutines();
        fade.SetFloat("Speed", 1);
        Delay(2, NewDay);
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
        StopAllCoroutines();
        endDayButton.gameObject.SetActive(true);
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
        triggersDict.Add("Filler/First", new Trigger { type = Trigger.Type.FIRST, adviceRating = 1 });
        triggersDict.Add("Filler/Random", new Trigger { type = Trigger.Type.RANDOM });
        triggersDict.Add("Events/Salt", new DrugTrigger("Salt"));
        triggersDict.Add("Events/Pepper", new DrugTrigger("Pepper"));
        triggersDict.Add("Events/Cute Sneeze", new CuteSneezeTrigger());
        return triggersDict;
    }

    public static void Delay(float seconds, System.Action callback) {
        instance.StartCoroutine(instance._Delay(seconds, callback));
    }

    IEnumerator _Delay(float seconds, System.Action callback) {
        yield return new WaitForSeconds(seconds);
        callback();
    }
}
