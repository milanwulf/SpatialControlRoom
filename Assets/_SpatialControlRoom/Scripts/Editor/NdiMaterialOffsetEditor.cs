using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NdiMaterialOffset))]
public class NdiMaterialOffsetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 

        NdiMaterialOffset script = (NdiMaterialOffset)target;

        if (GUI.changed) 
        {
            script.ApplyNdiMaterialOffset(); 
        }
    }
}
