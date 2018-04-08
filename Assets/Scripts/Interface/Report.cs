﻿using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Report : MonoBehaviour {

    public Image graph;
    public TextMeshProUGUI views;
    public TextMeshProUGUI advice;
    public Transform comments;

    public void Generate() {
        gameObject.SetActive(true);
        Debug.Log(DayManager.instance.excitement);
        
        views.text = Mathf.RoundToInt(
                Random.value * Mathf.Pow(10, 1 + 2 * Mathf.Log(Mathf.Max(1, DayManager.instance.excitement)))).ToString();

        foreach (Transform transform in comments)
            Destroy(transform.gameObject);

        foreach (CommentChain commentChain in CommentChainManager.comments.OrderBy(c => c.adviceRating).Take(3)) {
            CommentChain.Comment comment = commentChain.comments.OrderBy(c => Random.value).FirstOrDefault();
            CommentChainManager.CreateComment(comments, commentChain, comment).GetComponentInChildren<TextMeshProUGUI>().fontSize *= 1.5f;
        }
    }
}
