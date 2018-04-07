using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CommentChainManager : MonoBehaviour {

    public static CommentChainManager instance;

    public static CommentChain[] commentChains;

    public GameObject chat;
    public GameObject messagePrefab;
    public string[] commenterNames;

    void Awake() {
        instance = this;
    }
    
	void Start () {
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
            Text text = Instantiate(messagePrefab, chat.transform).GetComponentInChildren<Text>();
            text.text = chain.GetName(comment.commenterNumber) + ": " + comment.comment;
        }
    }
}
