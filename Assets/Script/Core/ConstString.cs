using UnityEngine;

public static class ConstString
{
    public struct Entity
    {
        public const string Player = "Player";
        public const string NPC = "NPC";
    }

    public struct Addressable
    {
        public static string GetDialogueWrapperAddress(int index) => $"DialogueWrapper/DialogueWrapper_{index}";
        public const string SequenceInfoLabel = "SequenceInfo";
    }
}
