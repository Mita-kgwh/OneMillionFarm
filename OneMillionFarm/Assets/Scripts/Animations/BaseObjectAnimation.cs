using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectStateAnimation
{
    NONE = -1,
    IDLE = 0,
    MOVE = 1,
    INTERACTTO = 2,
    INTERACTED = 3,
}

public class BaseObjectAnimation : MonoBehaviour
{
    [SerializeField] protected Transform targetAnimation;
    protected BaseObject hostObj;
    protected Sequence curTween;
    protected string animID = string.Empty;
    protected ObjectStateAnimation stateAnimation;
    protected System.Action OnComplete;

    public virtual void Init(BaseObject host)
    {
        this.hostObj = host;
        this.stateAnimation = ObjectStateAnimation.NONE;
        animID = $"worker_{GetInstanceID()}";
    }

    public virtual void PlayIdle()
    {
        if (stateAnimation == ObjectStateAnimation.IDLE)
        {
            return;
        }
        if (curTween != null)
        {
            StopAnimation();
        }
        this.stateAnimation = ObjectStateAnimation.IDLE;
        curTween = DOTween.Sequence();
        curTween.SetId(animID);
        curTween.Append(targetAnimation.DOLocalMoveY(0.3f, 0.5f));
        //curTween.AppendInterval(0.1f);
        curTween.SetLoops(-1, LoopType.Yoyo);
    }

    public virtual void PlayMove(System.Action onComplete = null)
    {
        this.stateAnimation = ObjectStateAnimation.MOVE;
        this.OnComplete = onComplete;
    }

    /// <summary>
    /// Interact to other thing
    /// </summary>
    public virtual void PlayInteracting()
    {
        this.stateAnimation = ObjectStateAnimation.INTERACTTO;
    }

    /// <summary>
    /// Be interacted
    /// </summary>
    public virtual void PlayInteracted()
    {
        this.stateAnimation = ObjectStateAnimation.INTERACTED;
    }

    public virtual void StopAnimation()
    {
        if (curTween != null)
        {
            DOTween.Kill(curTween);
            DOTween.Kill(targetAnimation);
            DOTween.Kill(animID);
            curTween = null;
        }

        this.OnComplete = null;
        this.stateAnimation = ObjectStateAnimation.NONE;
        targetAnimation.localPosition = Vector3.zero;
        targetAnimation.localRotation = Quaternion.identity;
    }
}
