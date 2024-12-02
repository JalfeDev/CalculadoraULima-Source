using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


// public class ReviewPlayerPrefs : EditorWindow
// {
//     [MenuItem("Jobs/My Statistics")] 
//     public static void ShowWindow()
//     {
//         ReviewPlayerPrefs newEditor = GetWindow<ReviewPlayerPrefs>("PlayerPrefs");
//     }
    
//     RawImage raw;
//     int posx,posy,width,height = 1;

//     private void OnGUI()
//     {
//         EditorGUILayout.LabelField("IndexCourse: ", PlayerPrefs.GetInt("IndexCourse").ToString());
//         EditorGUILayout.LabelField("IndexSemester: ", PlayerPrefs.GetInt("IndexSemester").ToString());

//         raw = (RawImage)EditorGUILayout.ObjectField("RawImage",raw, typeof(RawImage), true);
//         GUILayout.BeginHorizontal();
//         posx = EditorGUILayout.IntField("PosX", posx);
//         posy = EditorGUILayout.IntField("PosY", posy);
//         GUILayout.EndHorizontal();
        
//         GUILayout.BeginHorizontal();
//         width = EditorGUILayout.IntField("Width", width);
//         height = EditorGUILayout.IntField("Height", height);
//         GUILayout.EndHorizontal();

//         if (GUILayout.Button("Set uvRect") && raw != null)
//         {
//             raw.uvRect = new Rect(posx, posy, width, height);
//         }
//     }
// }
