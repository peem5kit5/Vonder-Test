using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StartDialogueSignalReceiver : MonoBehaviour, INotificationReceiver
{
    private bool isInitDialogue;

    private DialogueManager DialogueManager => GameManager.Instance.DialogueManager;
    private SequenceManager SequenceManager => GameManager.Instance.SequenceManager;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is not SignalEmitter)
            return;

        SequenceManager.PauseSequence();

        if (!isInitDialogue)
        {
            isInitDialogue = true;
            var dataList = GameManager.Instance.SequenceManager.CurrentSequenceInfo.GetDialogueDataList();
            DialogueManager.Show(dataList, OnResumeSequence, OnReachFinishDialogues);
        }
        else
        {
            DialogueManager.Advance();
        }
    }

    public void Reset()
    {
        isInitDialogue = false;
    }

    private void OnResumeSequence()
    {
        SequenceManager.ResumeSequence();
    }

    private void OnReachFinishDialogues()
    {
        isInitDialogue = false;
        SequenceManager.ResumeSequence();
    }
}