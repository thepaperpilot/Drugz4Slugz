using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommentChainManager : MonoBehaviour {

    public static CommentChainManager instance;

    public static CommentChain[] commentChains;

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
        foreach (CommentChain.Comment comment in chain.comments) {
            yield return new WaitForSeconds(comment.delay);
            TextMeshProUGUI text = Instantiate(messagePrefab, chat.transform).GetComponentInChildren<TextMeshProUGUI>();
            text.text = "<b>" + chain.GetName(comment.commenterNumber) + "</b>: " + comment.comment;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            scrollRect.verticalNormalizedPosition = 0;
        }
    }

    public static void Reset() {
        foreach (Transform transform in instance.chat.transform)
            Destroy(transform.gameObject);
    }
}
