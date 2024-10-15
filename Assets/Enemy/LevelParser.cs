using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelParser
{
    public static List<string[]> ParsePSV(string levelName)
    {

        // 从 Resources 文件夹读取
        TextAsset file = Resources.Load<TextAsset>("Enemy/LevelManager/"+ levelName);
        List<string[]> result = new List<string[]>();

        // 按行分割
        string[] lines = file.text.Split('\n');
        foreach (string line in lines)
        {
            string cleanLine = line.Trim();
            if (!string.IsNullOrEmpty(cleanLine))
            {
                string[] fields = cleanLine.Split(';');
                result.Add(fields);
            }
        }

        return result;
    }
}
