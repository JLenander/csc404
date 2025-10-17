using UnityEngine;

[CreateAssetMenu(fileName = "TaskInfoSO", menuName = "Scriptable Objects/TaskInfoSO", order = 1)]
public class TaskInfoSO : ScriptableObject
{
    public string id;
    public string title;
    [TextArea] public string description;

    public string location;
    public string urgency;
    public int targetProgress = 1;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
