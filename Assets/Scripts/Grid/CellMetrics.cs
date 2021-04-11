using System.Collections.Generic;
using UnityEngine;

public static class CellMetrics
{
    // Размер чанка в клетках
    public const int chunkSizeX = 2, chunkSizeZ = 2;

    // "Радиус" клетки (Половина стороны квадрата)
    public const float outerRadius = 2f;

    // Половина диагонали квадрата
    public const float innerRadius = outerRadius * 1.414213562f; // "внутренний" радиус больше внешнего.

    // "Радиус" малой клетки (половина стороны квадрата)
    public const float smallOuterRadius = 0.5f;

    // Отношение горизонтальных размеров гладкой части клетки и склона
    public const float solidFactor = 0.75f;

    // Обратное прошлой величины
    public const float blendFactor = 1f - solidFactor;

    // Вертикальная величина единичной высоты карты
    public const float elevationStep = 0.8f;

    // Сила шума для клеток (Не относится к Y координате гладкой части клетки) 
    public const float cellPerturbStrength = 1.0f;

    // Сила шума для Y координаты гладкой части клетки
    public const float elevationPerturbStrength = 0.4f;

    // Соотношение размера текстуры шума
    public const float noiseScale = 0.009f;

    // Сдвиг клеток
    public const float cellShift = 0.5f;

    // Сдвиг для получения координаты центра малой клетки, зная координаты угла
    public static Vector3 smallCellCenterShift = new Vector3(smallOuterRadius, 0f, smallOuterRadius);

    public static Vector3 hidedObjectsUI = new Vector3(-100f, -100f, 0f);
    public static Vector3 hidedObjects = new Vector3(-10f, 0f, -10f);

    public static Vector3 Yaxis = new Vector3(0f, 1f, 0f);
    public static Vector3 XYdir = new Vector3(1f, 1f, 0);

    // Координаты углов квадратой клетки
    public static List<Vector3> corners = new List<Vector3>() {
        new Vector3(outerRadius, 0, outerRadius), 
        new Vector3(outerRadius, 0, -outerRadius),
        new Vector3(-outerRadius, 0, -outerRadius),
        new Vector3(-outerRadius, 0, outerRadius),
        new Vector3(outerRadius, 0, outerRadius)
    };

    // Типы биомов
    public static Color[] colors;

    // Текстура разноцветного шума
    public static Texture2D noiseSource;

    public static Vector3 GetFirstCorner(CellDirection direction)
    {
        //Debug.Log("Here is " + (int)direction);
        int i = ((int)direction == 0 || (int)direction == 1) ? 8 : (int)direction; // Если прийдет CellDirection.N или CellDirection.NE, то могут быть проблемы
        return corners[i / 2 - 1];
    }

    public static Vector3 GetSecondCorner(CellDirection direction)
    {
        return corners[(int)direction / 2];
    }

    public static Vector3 GetFirstSolidCorner(CellDirection direction)
    {
        int i = ((int)direction == 0 || (int)direction == 1) ? 8 : (int)direction; // Если прийдет CellDirection.N или CellDirection.NE, то могут быть проблемы
        return corners[i / 2 - 1] * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner(CellDirection direction)
    {
        return corners[(int)direction / 2] * solidFactor;
    }

    public static Vector3 GetBridge(CellDirection direction)
    {
        int i = ((int)direction == 0 || (int)direction == 1) ? 8 : (int)direction; // Если прийдет CellDirection.N или CellDirection.NE, то могут быть проблемы
        return (corners[i / 2 - 1] + corners[i / 2]) * blendFactor;
    }

    public static Vector4 SampleNoise(Vector3 position)
    {
        return noiseSource.GetPixelBilinear(position.x * noiseScale, position.z * noiseScale);
    }
}
