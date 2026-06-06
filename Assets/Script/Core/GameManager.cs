using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CinemachineCamera cineMachineCamera;

    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private SequenceManager sequenceManager;
    [SerializeField] private NotificationManager notificationManager;

    [Header("Need to make it more organized.")]
    [SerializeField] private ArrowIndicatorUI arrowIndicatorUI;
    [SerializeField] private PlayerHandler playerHandler;
    [SerializeField] private NPCInteract npcInteract;
    [SerializeField] private ObjectInteractable objectInteractable;

    public DialogueManager DialogueManager => dialogueManager;
    public SequenceManager SequenceManager => sequenceManager;
    public NotificationManager NotificationManager => notificationManager;
    public PlayerInstanceData PlayerInstanceData { get; private set; }
    public ArrowIndicatorUI ArrowIndicatorUI => arrowIndicatorUI;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayerInstanceData = new();
        cineMachineCamera.Follow = playerHandler.transform;

        dialogueManager.Initialize();
        sequenceManager.Initialize();
        notificationManager.Initialize();

        // Note need to be more organized if it in bigger project
        playerHandler.Initialize();
        npcInteract.Initialize();
    }
}
