using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DayManager : MonoBehaviour {

    public static DayManager instance;

    public static Slug[] slugs;

    [HideInInspector]
    public int excitement = 0;

    [SerializeField]
    private Animator fade;
    [SerializeField]
    private Transform slugEnclosures;
    public Button liveButton;
    public Button endDayButton;

    void Awake() {
        instance = this;
    }

    public void NewDay() {
        slugs = slugEnclosures.GetComponentsInChildren<Slug>();
        foreach (Slug slug in slugs)
            slug.Sleep();
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
        DeskManager.instance.report.Generate();
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
    }
}
