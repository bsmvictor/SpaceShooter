using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator pAnimator;

    private void Awake()
    {
        pAnimator = GetComponent<Animator>();
    }

    public void PlayAnimation(string newAnimation)
    {
        pAnimator.Play(newAnimation);
    }
}