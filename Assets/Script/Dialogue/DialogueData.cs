using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Data/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string SpeakerName;
    [TextArea]
    public string Dialogue;

    [Header("Additional")]
    public bool IsCameraTargetSpeaker;
}
