using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadInfo
{
    public RoadPoint road;
    public float weight = 1;
}


public class RoadPoint : MonoBehaviour
{
    //估计这辈子都走不到这个点，因为这个点在光棱塔内部
    [SerializeField]
    private bool isEndOfTheRoad = false;

    public RoadInfo[] roadInfos;

    public bool IsEndOfTheRoad()
    {
        return isEndOfTheRoad;
    }
    public Vector2 GetPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public RoadPoint getNextPoint()
    {
        if (isEndOfTheRoad || roadInfos.Length == 0)
        {
            return null;
        }

        float totalWeight = 0;
        foreach (var roadInfo in roadInfos)
        {
            if (roadInfo.road != null)
            {
                totalWeight += roadInfo.weight;
            }
        }
        float selectPath = Random.Range(0, totalWeight);
        foreach (var roadInfo in roadInfos)
        {
            if (roadInfo.road != null)
            {
                selectPath -= roadInfo.weight;
                if (selectPath <= 0)
                {
                    return roadInfo.road;
                }
            }
        }
        Debug.Log("不应该出问题");
        return null;
    }
}
