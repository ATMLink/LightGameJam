using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor
{
    //Editor文件需要放在Editor下，请勿更改位置
    public override void OnInspectorGUI()
    {
        // 显示默认的Inspector界面
        DrawDefaultInspector();

        // 获取当前正在编辑的 EnemyManager 实例
        EnemyManager enemyManager = (EnemyManager)target;

        // 创建一个按钮
        if (GUILayout.Button("Next Turn"))
        {
            // 调用 EnemyManager 的 Generate 方法
            enemyManager.Generate();
        }
    }
}
