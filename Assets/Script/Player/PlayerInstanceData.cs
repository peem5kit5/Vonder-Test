using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInstanceData
{
    public IInteractable CurrentInteractable { get; private set; }

    public bool IsPlayerCanControl { get; private set; }

    public List<string> InventoryList { get; private set; }

    public int CurrentPlayerStepIndex { get; private set; }

    public event Action<string> OnReceiveNewItem;
    public event Action<string> OnRemoveItem;

    public PlayerInstanceData()
    {
        CurrentInteractable = null;
        IsPlayerCanControl = true;
        InventoryList = new();
        CurrentPlayerStepIndex = 0;
    }

    public void EnterInteractable(IInteractable interactable)
    {
        CurrentInteractable = interactable;
    }

    public void ClearInteractable()
    {
        CurrentInteractable = null;
    }

    public void SetControl(bool canControl)
    {
        IsPlayerCanControl = canControl;
    }

    public void AddItem(string itemId)
    {
        InventoryList.Add(itemId);
        OnReceiveNewItem?.Invoke(itemId);
    }

    public void RemoveItem(string itemId)
    {
        InventoryList.Remove(itemId);
        OnRemoveItem?.Invoke(itemId);
    }

    public void SetPlayerStepIndex(int stepIndex)
    {
        CurrentPlayerStepIndex = stepIndex;
    }
}