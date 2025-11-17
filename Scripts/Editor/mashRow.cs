using UnityEngine;
using UnityEditor;
using System.IO;

public class GridTextureGenerator : EditorWindow
{
    private int textureSize = 512;
    private int gridSize = 32;
    private Color gridColor = Color.gray;
    private Color backgroundColor = Color.white;
    private string savePath = "Assets/Material/environment";

    [MenuItem("Tools/生成网格纹理")]
    public static void ShowWindow()
    {
        GetWindow<GridTextureGenerator>("网格纹理生成器");
    }

    void OnGUI()
    {
        GUILayout.Label("网格纹理设置", EditorStyles.boldLabel);

        textureSize = EditorGUILayout.IntField("纹理尺寸", textureSize);
        gridSize = EditorGUILayout.IntField("网格大小", gridSize);
        gridColor = EditorGUILayout.ColorField("网格颜色", gridColor);
        backgroundColor = EditorGUILayout.ColorField("背景颜色", backgroundColor);
        savePath = EditorGUILayout.TextField("保存路径", savePath);

        if (GUILayout.Button("生成网格纹理"))
        {
            GenerateGridTexture();
        }
    }

    void GenerateGridTexture()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);

        // 填充背景色
        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = backgroundColor;
        }

        // 绘制网格线
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                // 检查是否在网格线上（水平和垂直）
                if (x % gridSize == 0 || y % gridSize == 0)
                {
                    pixels[y * textureSize + x] = gridColor;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        // 保存纹理
        byte[] pngData = texture.EncodeToPNG();

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        string filePath = Path.Combine(savePath, "gridTexture.png");
        File.WriteAllBytes(filePath, pngData);

        AssetDatabase.Refresh();

        // 显示生成的纹理
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);

        EditorUtility.DisplayDialog("完成", $"网格纹理已生成到:\n{filePath}", "确定");

        DestroyImmediate(texture);
    }
}