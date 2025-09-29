using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptableObj", menuName = "Scriptable Objects/DialogueScriptableObj")]
public class DialogueScriptableObj : ScriptableObject
{
    public string[] lines;
    public Sprite spirteA;
    public Sprite spirteB;
}
