using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [Header("Scale(��ģ)")]
    public int width;
    public int height;
    [Header("Initialise(��ʼ��)")]
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private Tile tile;
    [Header("Modification(�޸�)")]
    [SerializeField] private List<TileType> tileSetblockData;
    [SerializeField] private List<TileTypeFill> tileFillData;
    [Header("RelevantParameter(��ز���)")]
    [SerializeField] private List<Tile> tileSetblockList;
    [SerializeField] private List<Tile> tileFillList;
    [SerializeField] private List<Vector3Int> tilePositionList;
    [SerializeField] private List<Vector3Int> tileFillPositionStartList;
    [SerializeField] private List<Vector3Int> tileFillPositionEndList;
    

    // Start is called before the first frame update
    void Start()
    {
        // ��������Ƿ�����
        if (tileMap != null && tile != null)
        {
            // ����һ����Ƭ��䣬����ʹ�ù̶���С������Ը�����Ҫ����
            Vector3Int size = new Vector3Int(width, height, 1);

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    // ������Ƭ��ͼ��ÿ��λ�õ���Ƭ
                    tileMap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
        for (int i = 0; i < tileSetblockData.Count; i++)
        {
            if (tileSetblockData[i] != null)
            {
                Tile temptile = ScriptableObject.CreateInstance<Tile>();
                temptile.sprite = tileSetblockData[i].Sprite;
                tileSetblockList.Add(temptile);
                tilePositionList.Add(tileSetblockData[i].tilePosition);
            }
        }
        for (int i = 0; i < tileFillData.Count; i++)
        {
            if (tileFillData[i] != null)
            {
                Tile temptile = ScriptableObject.CreateInstance<Tile>();
                temptile.sprite = tileFillData[i].Sprite;
                tileFillList.Add(temptile);
                tileFillPositionStartList.Add(tileFillData[i].startTilePosition);
                tileFillPositionEndList.Add(tileFillData[i].endTilePosition);
            }
        }
        for (int i = 0; i < tileSetblockList.Count; i++)
        {
            if (tileSetblockList[i] != null && tilePositionList[i] != null)
            {
                SetblockTile(tileSetblockList[i], tilePositionList[i]);
            }
        }
        for (int i = 0; i < tileFillList.Count; i++)
        {
            if (tileFillList[i] != null && tileFillPositionStartList[i] != null && tileFillPositionEndList[i] != null)
            {
                FillTile(tileFillList[i], tileFillPositionStartList[i], tileFillPositionEndList[i]);
            }
        }
    }
    /// <summary>
    /// ����һ��vector3Int,���ض�Ӧλ�õ���Ƭ
    /// </summary>
    public TileBase GetTile(Vector3Int position) {
        TileBase tile = tileMap.GetTile(position);
        return tile;
    }
    void SetblockTile(Tile tile,Vector3Int position) {
        tileMap.SetTile(position, tile);
    }
    void FillTile(Tile tile,Vector3Int startposition,Vector3Int endposition) {
        int xtempmin = (startposition.x < endposition.x) ? startposition.x : endposition.x; 
        int xtempmax = (startposition.x > endposition.x) ? startposition.x : endposition.x; 
        int ytempmin = (startposition.y < endposition.y) ? startposition.y : endposition.y; 
        int ytempmax = (startposition.y > endposition.y) ? startposition.y : endposition.y;
        for (int x = xtempmin; x <= xtempmax; x++)
        {
            for (int y = ytempmin; y <= ytempmax; y++)
            {
                // ������Ƭ��ͼ��ÿ��λ�õ���Ƭ
                tileMap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        


    }
}
