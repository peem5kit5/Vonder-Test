using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TMP_Text text;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private Vector2 moveOffset = new Vector2(0f, -50f);

    private Vector2 shownPosition;
    private Tween activeTween;
    private CancellationTokenSource cts;

    public void Initialize()
    {
        shownPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = shownPosition;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public async UniTask ShowNotificationUI(string notificationText)
    {
        activeTween?.Kill();
        cts?.Cancel();
        cts?.Dispose();
        cts = new CancellationTokenSource();

        text.text = notificationText;
        canvasGroup.blocksRaycasts = true;

        if (await PlaySequence(canvasGroup.DOFade(1f, fadeDuration), rectTransform.DOAnchorPos(shownPosition, fadeDuration))) 
            return;

        bool cancelled = await UniTask.Delay(TimeSpan.FromSeconds(displayDuration), cancellationToken: cts.Token).SuppressCancellationThrow();
        if (cancelled) 
            return;

        if (await PlaySequence(canvasGroup.DOFade(0f, fadeDuration), rectTransform.DOAnchorPos(shownPosition + moveOffset, fadeDuration))) return;

        canvasGroup.blocksRaycasts = false;
    }

    private UniTask<bool> PlaySequence(Tween a, Tween b)
    {
        var utcs = new UniTaskCompletionSource<bool>();
        bool completed = false;
        activeTween = DOTween.Sequence()
            .Join(a)
            .Join(b)
            .OnComplete(() => { completed = true; utcs.TrySetResult(false); })
            .OnKill(() => { if (!completed) utcs.TrySetResult(true); });
        return utcs.Task;
    }

    private void OnDestroy()
    {
        activeTween?.Kill();
        cts?.Cancel();
        cts?.Dispose();
    }
}
