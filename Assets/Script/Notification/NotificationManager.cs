using Cysharp.Threading.Tasks;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private NotificationUI notificationUI;

    public void Initialize()
    {
        notificationUI.Initialize();
    }

    public void ShowNotification(string message)
    {
        notificationUI.ShowNotificationUI(message).Forget();
    }
}
