using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class collidertest : MonoBehaviour
{
    private BoxCollider2D[] boxColliders;
    private PolygonCollider2D polygonCollider;

    void Start()
    {
        boxColliders = GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D boxCollider in boxColliders)
        {
            Debug.Log(boxCollider.transform.position);
        }
        polygonCollider = GetComponent<PolygonCollider2D>();
    }
    private void Update()
    {
        CombineVertices();
    }
    void SetFlyingLand()
    {
        // 第一个矩形
        Vector2[] rect1Points = new Vector2[4];
        rect1Points[0] = new Vector2(0, 0);
        rect1Points[1] = new Vector2(1, 0);
        rect1Points[2] = new Vector2(1, 1);
        rect1Points[3] = new Vector2(0, 1);

        // 第二个矩形
        Vector2[] rect2Points = new Vector2[4];
        rect2Points[0] = new Vector2(2, 2);
        rect2Points[1] = new Vector2(3, 2);
        rect2Points[2] = new Vector2(3, 3);
        rect2Points[3] = new Vector2(2, 3);

        // 将两个矩形的顶点数组合并成一个数组
        Vector2[] allPoints = new Vector2[8];
        rect1Points.CopyTo(allPoints, 0);
        rect2Points.CopyTo(allPoints, 4);

        polygonCollider.points = allPoints;
    }
    void CalculateVertices(BoxCollider2D boxCollider, out Vector2[] vertices)
    {
        Vector2 size = boxCollider.size;
        vertices = new Vector2[4];
        vertices[0] = new Vector2(boxCollider.transform.position.x + size.x / 2, boxCollider.transform.position.y - size.y / 2);
        vertices[1] = new Vector2(boxCollider.transform.position.x - size.x / 2, boxCollider.transform.position.y - size.y / 2);
        vertices[2] = new Vector2(boxCollider.transform.position.x + size.x / 2, boxCollider.transform.position.y + size.y / 2);
        vertices[3] = new Vector2(boxCollider.transform.position.x - size.x / 2, boxCollider.transform.position.y + size.y / 2);
    }
    void CombineVertices()
    {
        HashSet<Vector2> allVertices = new HashSet<Vector2>();
        foreach (BoxCollider2D boxCollider in boxColliders)
        {
            Vector2[] vertices;
            CalculateVertices(boxCollider, out vertices);
            foreach (Vector2 vertex in vertices)
            {
                allVertices.Add(vertex);
            }
        }
        Vector2[] uniqueVertices = allVertices.ToArray();
        foreach (Vector2 vertex in uniqueVertices)Debug.Log(vertex);
        polygonCollider.points = uniqueVertices;
    }
}

