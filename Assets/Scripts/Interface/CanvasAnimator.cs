using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CanvasAnimator : MonoBehaviour {

    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void PrepareExperiment() {
        animator.SetFloat("Speed", 1);
        animator.Play("Slide", 0, 0);
    }

    public void PrepareSlugs() {
        animator.SetFloat("Speed", -1);
        animator.Play("Slide", 0, 1);
    }

    public void Reset() {
        animator.SetFloat("Speed", 0);
        animator.Play("Slide", 0, 0);
    }
}
