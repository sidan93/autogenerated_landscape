using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlgDiamondSquare : MonoBehaviour
{
    public int resolution = 128;
    public float length = 512;
    public bool reDraw = false;

    public float Roughness = 1;
    public float A1 = 0; // лево верх
    public float A2 = 0.5f; // право верх
    public float A3 = 0.2f;  // право низ
    public float A4 = 0.1f; // лево низ

    void Update()
    {
        if (reDraw == true)
        {
            reDraw = false;
            Debug.Log("UPDATE START");
            Terrain terrain = FindObjectOfType<Terrain>(); // Находи наш terrain
            float[,] heights = new float[resolution, resolution]; // Создаём массив вершин
            Queue<PointDS> points = new Queue<PointDS>();

            points.Enqueue(new PointDS(0, resolution - 1, 0, resolution - 1));
            heights[0, 0] = A1;
            heights[0, resolution - 1] = A2;
            heights[resolution - 1, resolution - 1] = A3;
            heights[resolution - 1, 0] = A4;
            int opertaion = 0;

            var point = points.Peek();
            var height = GetHeight(
                point.XLeft, point.YTop, heights[point.XLeft, point.YTop],
                point.XRight, point.YTop, heights[point.XRight, point.YTop],
                point.XRight, point.YBot, heights[point.XRight, point.YBot],
                point.XLeft, point.YBot, heights[point.XLeft, point.YBot]
                );
            Debug.Log(height);
            while (points.Count > 0 && opertaion++ < 4000)
            {
                point = points.Dequeue();
                height = GetHeight(
                    point.XLeft, point.YTop, heights[point.XLeft, point.YTop],
                    point.XRight, point.YTop, heights[point.XRight, point.YTop],
                    point.XRight, point.YBot, heights[point.XRight, point.YBot],
                    point.XLeft, point.YBot, heights[point.XLeft, point.YBot]
                    );

                heights[point.XCenter, point.YCenter] = 0.2f;

                if ((point.XCenter - point.XLeft) / 2 > 0 && (point.YTop - point.YCenter) / 2 > 0)
                    points.Enqueue(new PointDS(point.XLeft, point.XCenter, point.YCenter, point.YTop));
                if ((point.XRight - point.XCenter) / 2 > 0 && (point.YTop - point.YCenter) / 2 > 0)
                    points.Enqueue(new PointDS(point.XCenter, point.XRight, point.YCenter, point.YTop));
                if ((point.XRight - point.XCenter) / 2 > 0 && (point.YCenter - point.YBot) / 2 > 0)
                    points.Enqueue(new PointDS(point.XCenter, point.XRight, point.YBot, point.YCenter));
                if ((point.XCenter - point.XLeft) / 2 > 0 && (point.YCenter - point.YBot) / 2 > 0)
                    points.Enqueue(new PointDS(point.XLeft, point.XCenter, point.YBot, point.YCenter));
            }

            Debug.Log(points.Count);

            terrain.terrainData.size = new Vector3(length, length, length); // Устанавливаем размер нашей карты
            terrain.terrainData.heightmapResolution = resolution; // Задаём разрешение (кол-во высот)
            terrain.terrainData.SetHeights(0, 0, heights); // И, наконец, применяем нашу карту высот (heights)
            Debug.Log("UPDATE END");
        }

    }

    float GetHeight(int x1, int y1, float h1, int x2, int y2, float h2, int x3, int y3, float h3, int x4, int y4, float h4)
    {
        var roughness = Roughness;
        return (h1 + h2 + h3 + h4) / 4 + Random.Range(-roughness, roughness);
    }
}

class PointDS
{
    public int XLeft; 
    public int YTop;
    public int XRight; 
    public int YBot; 
    public int XCenter;
    public int YCenter;

    public PointDS(int XLeft, int XRight, int YBot, int YTop, int XCenter, int YCenter)
    {
        this.XLeft = XLeft;
        this.XRight = XRight;
        this.YBot = YBot;
        this.YTop = YTop;
        this.XCenter = XCenter;
        this.YCenter = YCenter;
    }
    public PointDS(int XLeft, int XRight, int YBot, int YTop) :
        this(XLeft, XRight, YBot, YTop, XLeft + (XRight - XLeft) / 2, YBot + (YTop - YBot) / 2)
    {
    }
    public override string ToString()
    {
        return string.Format("Left: {0}. Center: {1}. Right: {2}.", XLeft, XCenter, XRight);
    }
}