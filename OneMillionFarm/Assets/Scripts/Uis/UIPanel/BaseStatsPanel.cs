using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatsPanel : MonoBehaviour
{
    [SerializeField] protected RectTransform contentPanel;
    public bool moveLeft = false;
    protected bool animating;
    protected Sequence curSeq;
    protected string animID = string.Empty;
    protected float offset;

    protected virtual void Awake()
    {
        UnassignCallback();
        AssignCallback();
        animID = $"panel_{GetInstanceID()}";
        offset = contentPanel.rect.width;
        offset = moveLeft ? -offset : offset; 
    }

    protected virtual void OnDestroy()
    {
        UnassignCallback();
    }

    protected virtual void AssignCallback()
    {

    }

    protected virtual void UnassignCallback()
    {

    }

    protected virtual void AnimationShow()
    {
        animating = true;
        curSeq = DOTween.Sequence();
        curSeq.SetId(animID);
        curSeq.Append(contentPanel.DOLocalMoveX(0, 0.5f));
        curSeq.OnComplete(() =>
        {
            animating = false;
        });
    }

    protected virtual void AnimationHide()
    {
        animating = true;
        curSeq = DOTween.Sequence();
        curSeq.SetId(animID);
        curSeq.Append(contentPanel.DOLocalMoveX(offset, 0.5f));
        curSeq.OnComplete(() =>
        {
            animating = false;
        });
    }

    protected virtual void StopAnimation()
    {
        if (curSeq != null)
        {
            DOTween.Kill(curSeq);
            DOTween.Kill(contentPanel);
            DOTween.Kill(animID);
            curSeq = null;
        }
    }
}
