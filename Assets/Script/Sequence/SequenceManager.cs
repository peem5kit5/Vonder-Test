using System;
using UnityEngine;
using UnityEngine.Playables;

public class SequenceManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform sequenceContainer;
    [SerializeField] private StartDialogueSignalReceiver dialogueSignalReceiver;

    private PlayableDirector currentPlayableDirector;
    public SequenceInfo CurrentSequenceInfo { get; private set; }
    public int CurrentSequenceIndex { get; private set; }

    private SequenceInfo[] sequenceInfos;

    private PlayerInstanceData PlayerInstanceData => GameManager.Instance.PlayerInstanceData;

    public void Initialize()
    {
        // Note : I'm not sure using Addressable or make it assigned from inspector instead
        // Idea : Created sequence info gameobject and set as your wish in scene, Addressable can load but it mostly not correctly set positions, I don't know.
        sequenceInfos = sequenceContainer.GetComponentsInChildren<SequenceInfo>(true);
    }

    public bool CanPlayerStartSequence()
    {
        if (PlayerInstanceData.CurrentPlayerStepIndex > sequenceInfos.Length)
        {
            return false;
        }

        if (!sequenceInfos[PlayerInstanceData.CurrentPlayerStepIndex].CanPlaySequence())
        {
            return false;
        }

        return true;
    }

    public bool CheckItemMetConditionForNextSequence(string itemId)
    {
        int currentStepIndex = PlayerInstanceData.CurrentPlayerStepIndex;

        if (currentStepIndex >= sequenceInfos.Length)
            return false;

        return sequenceInfos[currentStepIndex].ItemConditionId == itemId;
    }

    public void TrySetSequence(int index)
    {
        PlayerInstanceData.SetControl(false);

        if (!CanPlayerStartSequence())
        {
            PlayerInstanceData.SetControl(true);
            return;
        }

        if (CurrentSequenceInfo != null && !string.IsNullOrEmpty(CurrentSequenceInfo.ItemConditionId))
            GameManager.Instance.PlayerInstanceData.RemoveItem(CurrentSequenceInfo.ItemConditionId);

        CurrentSequenceIndex = index;
        CurrentSequenceInfo  = sequenceInfos[index];

        GameManager.Instance.NotificationManager.ShowNotification($"Start Sequence {CurrentSequenceIndex + 1}");

        if (currentPlayableDirector != null)
        {
            currentPlayableDirector.stopped -= OnDirectorStopped;
            currentPlayableDirector.Stop();
        }

        currentPlayableDirector = CurrentSequenceInfo.GetDirector();
        currentPlayableDirector.stopped += OnDirectorStopped;
        currentPlayableDirector.Play();
    }

    public void ResumeSequence()
    {
        currentPlayableDirector.Resume();
    }

    public void PauseSequence()
    {
        if (currentPlayableDirector != null)
            currentPlayableDirector.Pause();
    }

    private void OnDirectorStopped(PlayableDirector director)
    {
        dialogueSignalReceiver.Reset();
        GameManager.Instance.DialogueManager.Hide();
    }

    public void StopSequence()
    {
        GameManager.Instance.NotificationManager.ShowNotification($"End sequence {CurrentSequenceIndex + 1}");

        if (currentPlayableDirector != null)
        {
            currentPlayableDirector.Stop();
            currentPlayableDirector = null;

            PlayerInstanceData.SetPlayerStepIndex(CurrentSequenceIndex + 1);
            PlayerInstanceData.SetControl(true);
        }
    }
}
