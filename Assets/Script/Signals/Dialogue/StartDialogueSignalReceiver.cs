using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StartDialogueSignalReceiver : MonoBehaviour, INotificationReceiver
{
    private DialogueManager DialogueManager => GameManager.Instance.DialogueManager;
    private SequenceManager SequenceManager => GameManager.Instance.SequenceManager;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is not SignalEmitter)
            return;

        SequenceManager.PauseSequence();
        DialogueManager.Advance();
    }
}