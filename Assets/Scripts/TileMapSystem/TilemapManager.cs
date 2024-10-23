using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TilemapManager : MonoBehaviour
{
    [Header("Map(��ͼ)")]
    [SerializeField] private Tilemap tileMap;
    [Header("Scale(��ģ)")]
    public int width;
    public int height;
    [Header("Initialise1(��ʼ��)")]
    [SerializeField] private TileTypeRandom tileBase;
    private Tile tile;
    //[SerializeField] private string URL;
    [SerializeField] private List<TileType> tiles;
    [SerializeField] private List<GameObject> prefabs;
    //[Header("Modification(�޸�)")]
    //[SerializeField] private List<TileType> tileSetblockData;
    [Header("RelevantParameter(��ز���)")]
    //[SerializeField] private string txtcontain;
    //[SerializeField] private byte[] tileData;
    //[SerializeField] private List<Tile> tileSetblockList;
    //[SerializeField] private List<Tile> tileFillList;
    //[SerializeField] private List<Vector3Int> tilePositionList;
    //[SerializeField] private List<Vector3Int> tileFillPositionStartList;
    //[SerializeField] private List<Vector3Int> tileFillPositionEndList;
    [SerializeField] private GameObject dad;


    // Start is called before the first frame update
    public void Initialize()
    {
        // ��������Ƿ�����
        if (tileMap != null && tile != null)
        {
            // ����һ����Ƭ��䣬����ʹ�ù̶���С������Ը�����Ҫ����
            Vector3Int size = new Vector3Int(width, height, 1);
            int midx = width / 2;
            int midy = height / 2;
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    // ������Ƭ��ͼ��ÿ��λ�õ���Ƭ
                    tileMap.SetTile(new Vector3Int(x - midx, y - midy, 0), tile);
                }
            }
        }
        else //(URL != null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("tilemaptxt");//���ﲻҪ���ļ���չ��
            if (textAsset != null)
            {
                string text = textAsset.text;
            }
            else
            {
                Debug.LogError("Text file not found in Resources!");
            }
            //FileStream txt = new FileStream(URL, FileMode.Open);
            //tileData = new byte[txt.Length];
            //txt.Read(tileData);
            //txtcontain = System.Text.Encoding.Default.GetString(tileData);
            //txt.Close();
            //string filePath = URL;
            string[] lines = textAsset.text.Split('\n');
            int midy = lines.Length / 2;
            int y = lines.Length-2;
            foreach (string line in lines)
            {
                int midx = line.Length / 2;
                for (int x = 0; x < line.Length; x++)
                {
                    char c = line[x];
                    if ((int)c == 32)
                    {
                        tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite = tileBase.sprites[Random.Range(0, tileBase.sprites.Count)];
                        tileMap.SetTile(new Vector3Int(x - midx, y - midy, 0), tile);
                    }
                    else if ((int)c - 48 < 0 || (int)c - 48 > 9) continue;
                    else if (tiles[(int)c - 48] != null && prefabs[(int)c - 48] != null)
                    {
                        tile = ScriptableObject.CreateInstance<Tile>();
                        tile.sprite = tiles[(int)c - 48].sprites[0];
                        tileMap.SetTile(new Vector3Int(x - midx, y - midy, 0), tile);
                        TilemapFeature temp = Instantiate(prefabs[(int)c - 48], new Vector3(x - midx + 0.5f, y - midy + 0.5f, 0), Quaternion.identity, dad.transform).GetComponent<TilemapFeature>();
                        temp.sprites = tiles[(int)c - 48].sprites;
                        temp.tileName = tiles[(int)c - 48].tileName;
                        temp.gameObject.name = tiles[(int)c - 48].tileName;
                        temp.canConstruct = tiles[(int)c - 48].canConstruct;
                        temp.canLightThrough = tiles[(int)c - 48].canLightThrough;
                        temp.canSlowEnemy = tiles[(int)c - 48].canSlowEnemy;
                        temp.canEnemyThrough = tiles[(int)c - 48].canEnemyThrough;
                        temp.canAttackTowerConstruct = tiles[(int)c - 48].canAttackTowerConstruct;
                        temp.canMinerConstruct = tiles[(int)c - 48].canMinerConstruct;

                    }
                }
                y--;
            }
        }
        //for (int i = 0; i < tileSetblockData.Count; i++)
        //{
        //    if (tileSetblockData[i] != null)
        //    {
        //        Tile temptile = ScriptableObject.CreateInstance<Tile>();
        //        temptile.sprite = tileSetblockData[i].Sprite;
        //        tileSetblockList.Add(temptile);
        //        tilePositionList.Add(tileSetblockData[i].tilePosition);
        //    }
        //}
        //for (int i = 0; i < tileFillData.Count; i++)
        //{
        //    if (tileFillData[i] != null)
        //    {
        //        Tile temptile = ScriptableObject.CreateInstance<Tile>();
        //        temptile.sprite = tileFillData[i].Sprite;
        //        tileFillList.Add(temptile);
        //        tileFillPositionStartList.Add(tileFillData[i].startTilePosition);
        //        tileFillPositionEndList.Add(tileFillData[i].endTilePosition);
        //    }
        //}
        //for (int i = 0; i < tileSetblockList.Count; i++)
        //{
        //    if (tileSetblockList[i] != null && tilePositionList[i] != null)
        //    {
        //        SetblockTile(tileSetblockList[i], tilePositionList[i]);
        //    }
        //}
        //for (int i = 0; i < tileFillList.Count; i++)
        //{
        //    if (tileFillList[i] != null && tileFillPositionStartList[i] != null && tileFillPositionEndList[i] != null)
        //    {
        //        FillTile(tileFillList[i], tileFillPositionStartList[i], tileFillPositionEndList[i]);
        //    }
        //}
    }
    /// <summary>
    /// ����һ��vector3Int,���ض�Ӧλ�õ���Ƭ
    /// </summary>
    public TileBase GetTile(Vector3Int position) {
        TileBase tile = tileMap.GetTile(position);
        return tile;
    }
    /// <summary>
    /// ����һ��vector3,���ض�Ӧλ�õ���Ƭ
    /// </summary>
    public TileBase GetTile(Vector3 position) {
        Vector3Int tilePosition = tileMap.WorldToCell(position);
        TileBase tile = tileMap.GetTile(tilePosition);
        return tile;
    }
    //void SetblockTile(Tile tile,Vector3Int position) {
    //    tileMap.SetTile(position, tile);
    //}
    //void FillTile(Tile tile,Vector3Int startposition,Vector3Int endposition) {
    //    int xtempmin = (startposition.x < endposition.x) ? startposition.x : endposition.x; 
    //    int xtempmax = (startposition.x > endposition.x) ? startposition.x : endposition.x; 
    //    int ytempmin = (startposition.y < endposition.y) ? startposition.y : endposition.y; 
    //    int ytempmax = (startposition.y > endposition.y) ? startposition.y : endposition.y;
    //    for (int x = xtempmin; x <= xtempmax; x++)
    //    {
    //        for (int y = ytempmin; y <= ytempmax; y++)
    //        {
    //            // ������Ƭ��ͼ��ÿ��λ�õ���Ƭ
    //            tileMap.SetTile(new Vector3Int(x, y, 0), tile);
    //        }
    //    }
    //}

}
