using UnityEngine;

[ExecuteInEditMode]
public class AlgRandom: MonoBehaviour
{
    public int resolution = 2048;
    public float width = 1000;
    public float height = 1000;
    public float length = 1000;
    public bool reDraw = false;
    public float maxHeight = 0.01f;

    void Update()
    {
        if (reDraw == true)
        {
            reDraw = false;
            Debug.Log("UPDATE");
            Terrain terrain = FindObjectOfType<Terrain>(); // Находи наш terrain
            float[,] heights = new float[resolution, resolution]; // Создаём массив вершин

            for(int i = 0; i < resolution; i ++)
                for (int j = 0; j < resolution; j++)
                {
                    heights[i, j] = Random.Range(-maxHeight, maxHeight);
                }

            terrain.terrainData.size = new Vector3(width, height, length); // Устанавливаем размер нашей карты
            terrain.terrainData.heightmapResolution = resolution; // Задаём разрешение (кол-во высот)
            terrain.terrainData.SetHeights(0, 0, heights); // И, наконец, применяем нашу карту высот (heights)
        }
        
    }
}
