using System.Collections;
using System.Linq;
using UnityEngine;

public class TriggerManager : MonoBehaviour {

    public static TriggerManager instance;

    private Trigger[] triggers;

    void Awake() {
        instance = this;
    }

    void Start () {
        triggers = CommentChainManager.commentChains.Select(chain => chain.trigger).Distinct().ToArray();
    }

    public void BeginRecording() {
        // Play random comment chain with a "FIRST" type
        Trigger trigger = triggers.Where(t => t.type == Trigger.Type.FIRST).OrderBy(t => Random.value).First();
        CommentChainManager.commentChains.Where(c => c.trigger == trigger).OrderBy(c => Random.value).First().Read();
    }
}
