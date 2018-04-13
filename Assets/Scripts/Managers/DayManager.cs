using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine.UI;

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
        if (slugs.Length == 0) {
            note = "Where were the slugs?";
        } else if (slugs.Where(s => s.drugs.Count == 0).Count() == 0)
            note = "Include a control group";
    }
}
