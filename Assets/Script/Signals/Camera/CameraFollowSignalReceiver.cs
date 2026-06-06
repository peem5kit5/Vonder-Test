using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class CameraFollowSignalReceiver : MonoBehaviour, INotificationReceiver
{
    [SerializeField] private CinemachineCamera cineMachineCamera;
    [SerializeField] private Transform cameraFollowTarget;

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        cineMachineCamera.Follow = cameraFollowTarget;
    }
}
