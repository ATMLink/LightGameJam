using System;
using System.Data;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoadPoint))]
public class RoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoadPoint roadList = (RoadPoint)target;

        // 初始化
        if (roadList.roadInfos == null || roadList.roadInfos.Length == 0)
        {
            roadList.roadInfos = new RoadInfo[1];
            roadList.roadInfos[0] = new RoadInfo();
        }

        int newSize = EditorGUILayout.IntField("Size", roadList.roadInfos.Length);
        if (newSize != roadList.roadInfos.Length)
        {
            Array.Resize(ref roadList.roadInfos, newSize);

            // 初始化
            for (int i = 0; i < newSize; i++)
            {
                if (roadList.roadInfos[i] == null)
                {
                    roadList.roadInfos[i] = new RoadInfo();
                }
            }
        }

        for (int i = 0; i < roadList.roadInfos.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // 显示 RoadPoint 和 float
            roadList.roadInfos[i].road = (RoadPoint)EditorGUILayout.ObjectField("Road Point", roadList.roadInfos[i].road, typeof(RoadPoint), true);
            roadList.roadInfos[i].weight = EditorGUILayout.FloatField("Weight", roadList.roadInfos[i].weight);
            EditorGUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
