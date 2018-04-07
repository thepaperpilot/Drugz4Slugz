using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Comment Chain")]
public class CommentChain : ScriptableObject {

    [Serializable]
    public struct Comment {
        public int commenterNumber;
        public float delay;
        public string comment;
    }
    
    public string trigger = "Filler/First";
    public Comment[] comments;
    [Range(-5, 5)]
    public int excitementDelta = 0;

    private Dictionary<int, string> commenterNames = new Dictionary<int, string>();

    public void Read() {
        commenterNames.Clear();
        CommentChainManager.instance.ReadCommentChain(this);
    }

    public string GetName(int commenterNumber) {
        if (!commenterNames.ContainsKey(commenterNumber))
            commenterNames.Add(commenterNumber, CommentChainManager.instance.commenterNames
                .Where(name => !commenterNames.ContainsValue(name))
                .OrderBy(name => UnityEngine.Random.value)
                .First());

        return commenterNames[commenterNumber];
    }
}
