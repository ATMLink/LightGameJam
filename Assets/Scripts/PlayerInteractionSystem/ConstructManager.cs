using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ConstructManager : MonoBehaviour
{
    [SerializeField] private MainResourceManagement resourceManagement;
    public Light2D _light;
    public GameObject lightgameobject;
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private TowerPool towerPool;
    [SerializeField] private LaserManager _laserManager;
    [SerializeField] private ReflectionManager reflectionManager;
    [SerializeField] private MusicManager musicManager;
    //[SerializeField] private GameObject reflect;
    //[SerializeField] private Text reflectText;
    //[SerializeField] private LightSystem lightSystem;
    private TowerAttributes selectedTowerAttributes;
    private void Start()
    {
        LightSystem.Instance.AddLight(_light);
        resourceManagement = GameObject.Find("ResourceManager").GetComponent<MainResourceManagement>();
    }
    // 设置当前选择的塔
    public void SelectTower(TowerAttributes towerAttributes)
    {
        selectedTowerAttributes = towerAttributes;
    }

    public void PlaceTower(Vector3 position)
    {
        if (selectedTowerAttributes != null)
        {
            if (CanPlaceTower(selectedTowerAttributes,position)) // 检查是否可以放置塔
            {
                if(musicManager != null)
                    musicManager.PlaySound("BeginBuildTower");
                towerManager.AddTower(position, selectedTowerAttributes);
                if(musicManager != null)
                    musicManager.PlaySound("FinishBuildTower");
                //_laserManager.UpdateState();
            }
            else
            {
                //reflectText.text = "无法放置塔";
                //reflect.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y,0);
                //reflect.SetActive(true);
                Debug.Log("无法在此位置放置塔。");
            }
        }
    }

    // 检查指定位置是否允许放置塔
    private bool CanPlaceTower(TowerAttributes towerAttributes,Vector3 position)
    {
        //Debug.LogWarning(towerAttributes.spendElement_1);
        //Debug.LogWarning(towerAttributes.elementNumber_1);
        //Debug.LogWarning(towerAttributes.spendElement_2);
        //Debug.LogWarning(towerAttributes.elementNumber_2);
        //Debug.LogWarning(resourceManagement.JudgeAfford(towerAttributes.spendElement_1, towerAttributes.elementNumber_1));
        //Debug.LogWarning(resourceManagement.JudgeAfford(towerAttributes.spendElement_2, towerAttributes.elementNumber_2));

        for (int i = 0; i < towerAttributes.elements.Count; i++) {
            if (!resourceManagement.JudgeAfford(towerAttributes.elements[i], towerAttributes.elementSpendNumber[i])) { reflectionManager.Reflect("资源不足"); return false; }
        }
        if (!LightSystem.Instance.IsIrradiated(new Vector2(position.x, position.y))) { reflectionManager.Reflect("太黑了"); return false; }
        //bool canConstruct = false;
        int tilecount = 0;
        int count = 0;
        float radius = 0f;
        TilemapFeature temp;
        Collider2D[] collider = Physics2D.OverlapCircleAll(position, radius);
        //if (collider.Length == 1) { return (towerAttributes.name == "Miner") ? false : true; }
        foreach (Collider2D col in collider)
            {
                GameObject foundObject = col.gameObject;
                //Debug.LogWarning(foundObject.transform.position);
            if (foundObject.tag == "Tilemap") continue;
            else if (foundObject.tag == "Tile")
            {
                tilecount++;
                temp = foundObject.GetComponent<TilemapFeature>();
                if (towerAttributes.name == "Miner" && temp.canMinerConstruct) return true;
                if (!temp.canConstruct)
                {
                    reflectionManager.Reflect("这里不能建塔");
                    //Debug.LogWarning(1);
                    return false;
                }
            }
            else if (foundObject.tag == "Tower")
            {
                if (foundObject.transform.position == position)
                {
                    count++;
                }
                if (count == 1) { reflectionManager.Reflect("已经有塔了"); return false; }

            }
            }
        if (tilecount == 0 && towerAttributes.name == "Miner")
        {
            reflectionManager.Reflect("采矿机只能建在矿脉上"); return false;
        }
        return true; // 可以放置
    }
    
    
}
