using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BaseDialog : MonoBehaviour
{
    [SerializeField] protected DialogType dialogType;
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Transform panel;

    public DialogType DialogType => this.dialogType;

    protected float transitionTime = 0.2f;

    public static System.Action<BaseDialog> OnShowDialog;
    public static System.Action<BaseDialog> OnCompleteShowDialog;
    public static System.Action<BaseDialog> OnHideDialog;
    public static System.Action<BaseDialog> OnCompleteHideDialog;

    public virtual void ShowDialog()
    {
        this.gameObject.SetActive(true);
        AnimationShow();
    }

    protected virtual void AnimationShow()
    {
        this.panel.localScale = Vector3.zero;
        if (this.canvasGroup != null)
        {
            this.canvasGroup.alpha = 0;

        }
        OnShowDialog?.Invoke(this);
        Sequence seq = DOTween.Sequence();
        seq.Join(this.panel.DOScale(1f, this.transitionTime).SetEase(Ease.OutBack).OnComplete(this.OnCompleteShow));
        if (this.canvasGroup != null)
        {
            seq.Join(this.canvasGroup.DOFade(1, this.transitionTime));
        }
    }

    protected virtual void OnCompleteShow()
    {
        OnCompleteShowDialog?.Invoke(this);
    }

    public virtual void CloseDialog()
    {
        AnimationHide();
    }
    protected virtual void AnimationHide()
    {
        OnHideDialog?.Invoke(this);
        Sequence seq = DOTween.Sequence();
        seq.Join(this.panel.DOScale(0.0f, this.transitionTime).SetEase(Ease.Linear).OnComplete(this.OnCompleteHide));
        if (this.canvasGroup != null)
        {
            seq.Join(this.canvasGroup.DOFade(0, this.transitionTime));
        }
    }

    protected virtual void OnCompleteHide()
    {
        this.gameObject.SetActive(false);
        OnCompleteHideDialog?.Invoke(this);
    }
}
