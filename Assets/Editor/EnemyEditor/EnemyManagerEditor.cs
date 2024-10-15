using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor
{
    //Editor�ļ���Ҫ����Editor�£��������λ��
    public override void OnInspectorGUI()
    {
        // ��ʾĬ�ϵ�Inspector����
        DrawDefaultInspector();

        // ��ȡ��ǰ���ڱ༭�� EnemyManager ʵ��
        EnemyManager enemyManager = (EnemyManager)target;

        // ����һ����ť
        if (GUILayout.Button("Next Turn"))
        {
            // ���� EnemyManager �� Generate ����
            enemyManager.Generate();
        }
    }
}
