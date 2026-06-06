using UnityEngine;
using UnityEngine.Playables;

public class SequenceEndSignalReceiver : MonoBehaviour, INotificationReceiver
{
    private SequenceManager SequenceManager => GameManager.Instance.SequenceManager;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        SequenceManager.StopSequence();
    }
}
