using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MatchingButton))]
[CanEditMultipleObjects]
[System.Serializable]
public class MatchingLevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MatchingButton myScript = target as MatchingButton;
        
        switch (myScript.ButtonType)
        {
            case MatchingButton.EButtonType.PairNumberBtn:
                myScript.PairNumber = (PairsManager.EPairNumber)EditorGUILayout.EnumPopup("Pair Number", myScript.PairNumber);
                break;

            case MatchingButton.EButtonType.PuzzelCatagoryBtn:
                myScript.PuzzleCatagories = (PairsManager.EPuzzleCatagories)EditorGUILayout.EnumPopup("Puzzel Catagories", myScript.PuzzleCatagories);
                break;  
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

    }   
    
}
