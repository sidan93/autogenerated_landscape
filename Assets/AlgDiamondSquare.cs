using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlgDiamondSquare : MonoBehaviour
{
    public int resolution = 128;
    public float length = 512;
    public bool reDraw = false;

    public float Roughness = 1;
    public float A1 = 0;
    public float A2 = 0.5f;

    void Update()
    {
        if (reDraw == true)
        {
            reDraw = false;
            Debug.Log("UPDATE START");
            Terrain terrain = FindObjectOfType<Terrain>(); // Находи наш terrain
            float[,] heights = new float[resolution, resolution]; // Создаём массив вершин
            Queue<PointDS> points = new Queue<PointDS>();

            points.Enqueue(new PointDS(0, resolution - 1, (resolution - 1) / 2));
            heights[0, 0] = A1;
            heights[0, resolution - 1] = A2;
            int opertaion = 0;
            while (points.Count > 0 && opertaion++ < resolution*resolution)
            {
                var point = points.Dequeue();
                var height = GetHeight(point.XLeft, heights[0, point.XLeft], point.XRight, heights[0, point.XRight]);
                heights[0, point.XCenter] = height;
                if ((point.XCenter - point.XLeft) / 2 > 0)
                    points.Enqueue(new PointDS(point.XLeft, point.XCenter, point.XLeft + (point.XCenter - point.XLeft) / 2));
                if ((point.XRight - point.XCenter) / 2 > 0)
                    points.Enqueue(new PointDS(point.XCenter, point.XRight, point.XCenter + (point.XRight - point.XCenter) / 2));
            }

            terrain.terrainData.size = new Vector3(length, length, length); // Устанавливаем размер нашей карты
            terrain.terrainData.heightmapResolution = resolution; // Задаём разрешение (кол-во высот)
            terrain.terrainData.SetHeights(0, 0, heights); // И, наконец, применяем нашу карту высот (heights)
            Debug.Log("UPDATE END");
        }

    }

    float GetHeight(int x1, float h1, int x2, float h2)
    {
        var roughness = Roughness * (x2 - x1);
        return (h1 + h2) / 2 + Random.Range(-roughness, roughness);
    }
}

class PointDS
{
    public int XLeft; // Лево верх
    public int XRight; // Право верх
    public int XCenter;

    public PointDS(int XLeft, int XRight, int XCenter)
    {
        this.XLeft = XLeft;
        this.XRight = XRight;
        this.XCenter = XCenter;
    }
    public override string ToString()
    {
        return string.Format("Left: {0}. Center: {1}. Right: {2}.", XLeft, XCenter, XRight);
    }
}