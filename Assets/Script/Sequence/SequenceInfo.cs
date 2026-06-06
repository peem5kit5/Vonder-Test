using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SequenceInfo : MonoBehaviour
{
    [SerializeField] private DialogueDataList dialogueDataList;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private string itemConditionId;

    public string ItemConditionId => itemConditionId;

    public bool CanPlaySequence()
    {
        if (string.IsNullOrEmpty(itemConditionId))
            return true;

        bool hasConditionItem = GameManager.Instance.PlayerInstanceData.InventoryList.Contains(itemConditionId);

        if (!hasConditionItem)
            return false;

        return true;
    }

    public DialogueDataList GetDialogueDataList() => dialogueDataList;
    public PlayableDirector GetDirector() => director;
}
