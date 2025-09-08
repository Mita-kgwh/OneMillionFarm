using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnimation : BaseObjectAnimation
{
    private WorkerActor actor;

    public override void Init(BaseObject host)
    {
        base.Init(host);
        if (host == null)
        {
            Debug.LogError($"Host null");
            return;
        }

        actor = (WorkerActor)host;
        if (actor == null)
        {
            Debug.LogError($"Host is not worker");
        }
    }

    public override void PlayMove(System.Action onComplete = null)
    {
        if (stateAnimation == ObjectStateAnimation.MOVE)
        {
            return;
        }

        if (actor == null || actor.IsFree)
        {
            return;
        }

        if (curTween != null)
        {
            StopAnimation();
        }
        base.PlayMove();
        var targetTf = actor.WorkableObject.transform;
        var moveObjTf = targetAnimation.transform.parent;
        Vector3 directionVec = actor.transform.position - targetTf.position;
        float durationMove = directionVec.magnitude / actor.moveSpeed;
        curTween = DOTween.Sequence();
        curTween.SetId(animID);
        curTween.Append(moveObjTf.DOMove(targetTf.position - directionVec.normalized, durationMove)
                                 .SetEase(Ease.Linear));
        curTween.OnComplete(() =>
        {
            OnComplete?.Invoke();
            OnComplete = null;
        });

    }
}
