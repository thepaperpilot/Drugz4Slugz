using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommentChainManager : MonoBehaviour {

    public static CommentChainManager instance;

    public static CommentChain[] commentChains;
    public static List<CommentChain> comments = new List<CommentChain>();

    public GameObject chat;
    public GameObject messagePrefab;
    public string[] commenterNames;

    private ScrollRect scrollRect;

    void Awake() {
        instance = this;
        scrollRect = chat.GetComponentInParent<ScrollRect>();
    }
    
	void Start() {
        Object[] resources = Resources.LoadAll("", typeof(CommentChain));
        commentChains = new CommentChain[resources.Length];
        for (int i = 0; i < commentChains.Length; i++) {
            commentChains[i] = (CommentChain)resources[i];
        }
    }

    public void ReadCommentChain(CommentChain chain) {
        StartCoroutine(_ReadCommentChain(chain));
    }

    IEnumerator _ReadCommentChain(CommentChain chain) {
        DayManager.instance.excitement += chain.excitementDelta;
        comments.Add(chain);
        foreach (CommentChain.Comment comment in chain.comments) {
            yield return new WaitForSeconds(comment.delay);
            CreateComment(chat.transform, chain, comment);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            // Force layout rebuild
            VerticalLayoutGroup layout = chat.transform.GetComponent<VerticalLayoutGroup>();
            layout.enabled = false;
            layout.enabled = true;
            scrollRect.verticalNormalizedPosition = 0;
        }
    }

    public static GameObject CreateComment(Transform parent, CommentChain chain, CommentChain.Comment comment, GameObject prefab = null) {
        GameObject commentObject = Instantiate(prefab ?? instance.messagePrefab, parent);
        TextMeshProUGUI text = commentObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "<b>" + chain.GetName(comment.commenterNumber) + "</b>: " + comment.comment;
        return commentObject;
    }

    public static void Reset() {
        comments.Clear();
        foreach (Transform transform in instance.chat.transform)
            Destroy(transform.gameObject);
    }
}
