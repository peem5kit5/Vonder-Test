using UnityEngine;

public class NPCInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator animator;
    private PlayerInstanceData PlayerInstanceData => GameManager.Instance.PlayerInstanceData;
    private SequenceManager SequenceManager => GameManager.Instance.SequenceManager;

    public void Initialize()
    {
        GameManager.Instance.DialogueManager.AddDialogueCameraTarget(ConstString.Entity.NPC, transform);
    }

    public void Interact()
    {
        SequenceManager.TrySetSequence(PlayerInstanceData.CurrentPlayerStepIndex);
    }

    public bool CanInteract()
    {
        return SequenceManager.CanPlayerStartSequence();
    }

    public void PlayAnimation(string animationId)
    {
        animator.Play(animationId);
    }
}
