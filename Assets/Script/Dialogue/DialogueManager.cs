using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text lineText;

    [Header("Setting")]
    [SerializeField] private float charsPerSecond = 25f;

    [Header("Testing")]
    [SerializeField] private DialogueDataList testDialogueDataList;

    [Header("Cinemachine")]
    [SerializeField] private CinemachineCamera cineMachineCamera;

    private DialogueData[] dialogueData;
    private int dataIndex;

    private Action onResumeSequence;
    private Action onReachFinishDialogue;

    private bool isTyping;
    private CancellationTokenSource typingCts;

    private Dictionary<string, Transform> dialogueCameraTargets = new();

    private bool isHoldDialogue;

    public void Initialize()
    {
        Hide();
        continueButton.onClick.AddListener(Advance);
        dialogueCameraTargets.Clear();
    }

    public void AddDialogueCameraTarget(string targetId, Transform target)
    {
        dialogueCameraTargets.Add(targetId, target);
    }

    public void Show(DialogueDataList list, Action onResumeSequence, Action onReachFinishDialogue)
    {
        this.onResumeSequence = onResumeSequence;
        this.onReachFinishDialogue = onReachFinishDialogue;

        dialogueData = list.Data;
        dataIndex = 0;

        ToggleCanvasGroup(true);
        DisplayLine();
    }

    private void ToggleCanvasGroup(bool isShow)
    {
        panelCanvasGroup.alpha = isShow ? 1f : 0f;
        panelCanvasGroup.blocksRaycasts = isShow;
        panelCanvasGroup.interactable = isShow;
    }

    public void Hide()
    {
        CancelTyping();
        ToggleCanvasGroup(false);
    }

    public void Advance()
    {
        if (!panelCanvasGroup.interactable)
        {
            ToggleCanvasGroup(true);
        }

        if (isTyping)
        {
            CancelTyping();
            lineText.text = dialogueData[dataIndex].Dialogue;
            return;
        }

        bool isPause = dialogueData[dataIndex].IsPauseDialogueAfterFinishTyping;

        if (isPause && !isHoldDialogue)
        {
            isHoldDialogue = true;

            onResumeSequence?.Invoke();
            Hide();
            return;
        }

        isHoldDialogue = false;

        dataIndex++;

        bool isReachedDialogueLength = dataIndex >= dialogueData.Length;

        if (isReachedDialogueLength)
        {
            onReachFinishDialogue?.Invoke();
            Hide();
            return;
        }

        DisplayLine();
    }

    private void DisplayLine()
    {
        string speakerName = dialogueData[dataIndex].SpeakerName;
        if (dialogueData[dataIndex].IsCameraTargetSpeaker && dialogueCameraTargets.TryGetValue(speakerName, out var target))
        {
            cineMachineCamera.Follow = target;
        }

        CancelTyping();

        speakerText.text = speakerName;
        typingCts = new CancellationTokenSource();

        TypeCharacters(dialogueData[dataIndex].Dialogue, typingCts.Token).Forget();
    }

    private async UniTaskVoid TypeCharacters(string line, CancellationToken ct)
    {
        isTyping = true;
        lineText.text = string.Empty;

        float delay = 1f / charsPerSecond;
        foreach (char c in line)
        {
            lineText.text += c;
            bool cancelled = await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: ct)
                .SuppressCancellationThrow();
            if (cancelled) break;
        }

        isTyping = false;
    }

    private void CancelTyping()
    {
        typingCts?.Cancel();
        typingCts?.Dispose();
        typingCts = null;
        isTyping = false;
    }

    void OnDestroy() => CancelTyping();

    public void TestDialogue()
    {
        if (testDialogueDataList == null) return;
        Show(testDialogueDataList, () => Debug.Log("Test dialogue resume. "), () => Debug.Log("Test dialogue reached max length. "));
    }
}
