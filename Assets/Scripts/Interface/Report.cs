using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Report : MonoBehaviour {

    public Image graph;
    public TextMeshProUGUI views;
    public TextMeshProUGUI advice;
    public Transform comments;
    public GameObject messagePrefab;

    public Sprite downwardGraph;
    public Sprite neutralGraph;
    public Sprite upwardGraph;

    private int lastExcitement = 0;

    public void Generate() {
        gameObject.SetActive(true);

        int diff = DayManager.instance.excitement - lastExcitement;
        graph.sprite = diff > 0 ? upwardGraph : diff < 0 ? downwardGraph : neutralGraph;
        lastExcitement = DayManager.instance.excitement;
        
        views.text = Mathf.RoundToInt(
                Random.value * Mathf.Pow(10, 1 + 2 * Mathf.Log(Mathf.Max(1, DayManager.instance.excitement)))).ToString();
        advice.text = DayManager.note;

        foreach (Transform transform in comments)
            Destroy(transform.gameObject);

        foreach (CommentChain commentChain in CommentChainManager.comments.OrderBy(c => c.adviceRating).Take(3)) {
            CommentChain.Comment comment = commentChain.comments.OrderBy(c => Random.value).FirstOrDefault();
            CommentChainManager.CreateComment(comments, commentChain, comment, messagePrefab).GetComponentInChildren<TextMeshProUGUI>().fontSize = 16;
        }

        DayManager.Delay(0, delegate {
            // Force layout rebuild
            VerticalLayoutGroup layout = comments.GetComponent<VerticalLayoutGroup>();
            layout.enabled = false;
            layout.enabled = true;
        });
    }
}
