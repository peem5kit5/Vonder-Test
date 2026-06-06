using UnityEngine;

public class ObjectInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemId = "item0001";

    public string ItemId => itemId;

    public void OnEnable()
    {
        GameManager.Instance.PlayerInstanceData.OnReceiveNewItem += OnGetItem;
        GameManager.Instance.ArrowIndicatorUI.SetTarget(transform);
    }

    public void Interact()
    {
        GameManager.Instance.PlayerInstanceData.AddItem(itemId);
        Destroy(gameObject);

        GameManager.Instance.NotificationManager.ShowNotification($"Received Item : {itemId}");
    }

    private void OnGetItem(string itemId)
    {
        if (itemId == this.itemId)
        {
            GameManager.Instance.ArrowIndicatorUI.SetTarget(null);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.PlayerInstanceData.OnReceiveNewItem -= OnGetItem;
    }

    public bool CanInteract()
    {
        return GameManager.Instance.SequenceManager.CheckItemMetConditionForNextSequence(itemId);
    }
}
