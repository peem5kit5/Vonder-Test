using UnityEngine;

public class ObjectInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemId = "item0001";

    private PlayerInstanceData PlayerInstanceData => GameManager.Instance.PlayerInstanceData;
    private SequenceManager SequenceManager => GameManager.Instance.SequenceManager;
    private ArrowIndicatorUI ArrowIndicatorUI => GameManager.Instance.ArrowIndicatorUI;
    private NotificationManager NotificationManager => GameManager.Instance.NotificationManager;

    public string ItemId => itemId;

    public void OnEnable()
    {
        // it can be more organized and maintainable in bigger project.
        PlayerInstanceData.OnReceiveNewItem += OnGetItem;
        ArrowIndicatorUI.SetTarget(transform);
    }

    public void Interact()
    {
        PlayerInstanceData.AddItem(itemId);
        Destroy(gameObject);
    }

    private void OnGetItem(string itemId)
    {
        if (itemId == this.itemId)
        {
            ArrowIndicatorUI.SetTarget(null);
        }

        NotificationManager.ShowNotification($"Received Item : {itemId}");
    }

    private void OnDestroy()
    {
        PlayerInstanceData.OnReceiveNewItem -= OnGetItem;
    }

    public bool CanInteract()
    {
        return GameManager.Instance.SequenceManager.CheckItemMetConditionForSequence(itemId);
    }
}
