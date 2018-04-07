using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Comment Chain")]
public class CommentChain : ScriptableObject {

    [Serializable]
    public struct Comment {
        public int commenterNumber;
        public float delay;
        public string comment;
    }

    public Trigger trigger;
    public Comment[] comments;
    [Range(-5, 5)]
    public int excitementDelta = 0;
}
