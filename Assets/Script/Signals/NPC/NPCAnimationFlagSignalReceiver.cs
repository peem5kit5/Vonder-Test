using UnityEngine;
using UnityEngine.Playables;

public class NPCAnimationFlagSignalReceiver : MonoBehaviour, INotificationReceiver
{
    [SerializeField] private NPCAnimationType animationType;
    [SerializeField] private NPCInteract npcInteract;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        npcInteract.PlayAnimation($"NPC_{animationType.ToString()}");
    }

    public void SetNewAnimation(NPCAnimationType npcAnimationType)
    {
        animationType = npcAnimationType;
    }
}

public enum NPCAnimationType
{
    Idle,
    Talk,
    Finish
}

