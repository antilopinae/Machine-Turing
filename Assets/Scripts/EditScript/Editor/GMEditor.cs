using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GameManager))]
public class GMEditor : Editor
{
    private GameManager gm;
    //pri activacii scriptaà 
    public void OnEnable()
    {
        gm = (GameManager)target;
    }
    public override void OnInspectorGUI()
    {
        if (gm.Items.Count > 0)
        {
            foreach (Item item in gm.Items)
            {
                
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(15)))
                {
                    gm.Items.Remove(item);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                //pole int znachenie
                item.Id = EditorGUILayout.IntField("Id Level", item.Id);
                item.StartWord = EditorGUILayout.TextField("Start word", item.StartWord);
                item.FinishWord = EditorGUILayout.TextField("Finish word", item.FinishWord);
                //item.Background = (Sprite)EditorGUILayout.ObjectField("Background", item.Background, typeof(Sprite), false);
                //item.Colors = EditorGUILayout.DelayedIntField("Colors", item.Colors);
                item.ABC = EditorGUILayout.TextField("Alphabet", item.ABC);
                EditorGUILayout.EndVertical();
            }
        }
        else EditorGUILayout.LabelField("No elements in list!");
        if (GUILayout.Button("Creat New Level", GUILayout.Width(130)))
        {
            gm.Items.Add(new Item());
        }
        if (GUI.changed) SetObjectDirty(gm.gameObject);
    }
    public static void SetObjectDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}
