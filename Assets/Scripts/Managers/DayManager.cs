using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DayManager : MonoBehaviour {

    public static DayManager instance;

    public static Slug[] slugs;
    public static string note;

    [HideInInspector]
    public int excitement = 0;

    [SerializeField]
    private Animator fade;
    [SerializeField]
    private Transform slugEnclosures;
    public Button liveButton;
    public Button endDayButton;
    [SerializeField]
    private Procedure[] procedures;

    private int day = 0;

    void Awake() {
        instance = this;
    }

    void Start() {
        if (day < procedures.Length)
            DeskManager.instance.procedures.Generate(procedures[day++]);
        else DeskManager.instance.procedures.gameObject.SetActive(false);
    }

    public void NewDay() {
        slugs = slugEnclosures.GetComponentsInChildren<Slug>();
        foreach (Slug slug in slugs)
            slug.Sleep();
        foreach (Transform enclosure in slugEnclosures.GetComponentsInChildren<Button>().Select(s => s.transform)) {
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
        DeskManager.instance.report.Generate();
        if (day < procedures.Length)
            DeskManager.instance.procedures.Generate(procedures[day++]);
        else DeskManager.instance.procedures.gameObject.SetActive(false);
        CommentChainManager.Reset();
        if (excitement > 0)
            excitement -= 1;
        fade.SetFloat("Speed", -1);
    }

    public void EndDay() {
        CommentChainManager.instance.StopAllCoroutines();
        fade.SetFloat("Speed", 1);
        fade.Play("Fade", 0, 0);
        Delay(2, NewDay);
    }

    public static void Delay(float seconds, System.Action callback) {
        instance.StartCoroutine(instance._Delay(seconds, callback));
    }

    IEnumerator _Delay(float seconds, System.Action callback) {
        yield return new WaitForSeconds(seconds);
        callback();
    }

    public static void BeginRecording() {
        slugs = instance.slugEnclosures.GetComponentsInChildren<Slug>();
        instance.liveButton.interactable = false;

        note = "Good Work!";
        if (slugs.Length == 0)
            note = "Where were the slugs?";
        else if (slugs.Any(s => s.drugs.Where(d => d.strength - d.resistance > d.drug.maxDosage).Count() != 0))
            note = "Do not OD the slugs";
        else if (slugs.Where(s => s.drugs.Count == 0).Count() == 0)
            note = "Include a control group";
        else if (!FollowedInstructions())
            note = "Follow the procedures!";
    }

    static bool FollowedInstructions() {
        Procedure proc = instance.procedures[instance.day];
        Procedure.Dose[][] conditionsLists = new Procedure.Dose[][] { proc.firstSlug, proc.secondSlug, proc.thirdSlug, proc.fourthSlug };
        if (!conditionsLists.Any(c => c.Length > 0)) return true;

        Dictionary<Procedure.Dose[], IEnumerable<Slug>> filteredSlugs = new Dictionary<Procedure.Dose[], IEnumerable<Slug>>();
        foreach (Procedure.Dose[] conditions in conditionsLists)
            filteredSlugs.Add(conditions, slugs.Where(s => conditions.Any(d => s.drugs.Any(drug => drug.drug.GetType().Name == d.drug && drug.strength == d.dosage))));

        conditionsLists = conditionsLists.OrderBy(l => filteredSlugs[l].Count()).ToArray();
        if (conditionsLists[0].Length == 0) {
            if (conditionsLists[1].Length == 0) {
                if (conditionsLists[2].Length == 0) {
                    if (conditionsLists[3].Length == 0 || filteredSlugs[conditionsLists[3]].Count() > 0) {
                        return true;
                    }
                } else {
                    foreach (Slug slug in filteredSlugs[conditionsLists[2]]) {
                        if (conditionsLists[3].Length == 0 || filteredSlugs[conditionsLists[3]].Except(new Slug[] { slug }).Count() > 0) {
                            return true;
                        }
                    }
                }
            } else {
                foreach (Slug slug1 in filteredSlugs[conditionsLists[1]]) {
                    if (conditionsLists[2].Length == 0) {
                        if (conditionsLists[3].Length == 0 || filteredSlugs[conditionsLists[3]].Count() > 0) {
                            return true;
                        }
                    } else {
                        foreach (Slug slug2 in filteredSlugs[conditionsLists[2]].Except(new Slug[] { slug1 })) {
                            if (conditionsLists[3].Length == 0 || filteredSlugs[conditionsLists[3]].Except(new Slug[] { slug1, slug2 }).Count() > 0) {
                                return true;
                            }
                        }
                    }
                }
            }
        } else {
            foreach (Slug slug1 in filteredSlugs[conditionsLists[0]]) {
                foreach (Slug slug2 in filteredSlugs[conditionsLists[1]].Except(new Slug[] { slug1 })) {
                    if (conditionsLists[2].Length == 0) {
                        if (conditionsLists[3].Length == 0 || filteredSlugs[conditionsLists[3]].Count() > 0) {
                            return true;
                        }
                    } else {
                        foreach (Slug slug3 in filteredSlugs[conditionsLists[2]].Except(new Slug[] { slug1, slug2 })) {
                            if (conditionsLists[3].Length == 0 || filteredSlugs[conditionsLists[3]].Except(new Slug[] { slug1, slug2, slug3 }).Count() > 0) {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
}
