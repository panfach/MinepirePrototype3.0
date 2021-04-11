using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
/// <summary>
/// Этот скрипт висит на каждом CellPointer в BuildSet'е здания.
/// </summary>
public class CellPointer : MonoBehaviour
{
    public static int[,] cornerVerticeIndices = new int[4, 6]
    {
        {1, 14, 16, 3, 9, 17}, 
        {0, 13, 23, 2, 8, 22}, 
        {6, 12, 20, 4, 10, 21}, 
        {7, 15, 19, 5, 11, 18}
    };
    public static Color[] colors;
    static float cornerShift = 0.05f;

    [Header("GridObject")]
    [SerializeField] GridObject gridObject;

    [SerializeField] bool enterCellPointer;
    [SerializeField] bool isEnableForBuild = false;

    Vector3 oldPos = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    Mesh mesh;
    Transform _transform;
    Transform _parentTransform;

    public bool EnterCellPointer { get => enterCellPointer; }
    public bool IsEnableForBuild { get => isEnableForBuild; }


    private void Awake()
    {
        if (gridObject == null) gridObject = transform.parent.parent.GetComponent<GridObject>();
        mesh = GetComponent<MeshFilter>().mesh;
        _transform = transform;
        ChangeColor();
        _parentTransform = transform.parent;
    }


    // Обновление каждого уголка меша CellPointer в зависимости от шума
    public void Refresh()
    {
        Vector3[] cornerVertices;
        Vector3[] tempVerticeArray;
        SCCoord tempSCC;

        oldPos = _transform.position;

        CheckState();

        LeanTween.cancel(gameObject);
        if (_transform.localPosition.y < 2f) _transform.localPosition += new Vector3(0f, 1f, 0f);
        LeanTween.moveLocalY(gameObject, 0f, 0.1f);

        tempVerticeArray = mesh.vertices;
        tempSCC = SCCoord.FromPos(_transform.position);
        cornerVertices = SCCoord.GetCornersRotated(GeneralBuilder.buildingAngle.Index ,tempSCC, _transform.position.y);

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                //tempVerticeArray[cornerVerticeIndices[i, j]].z += 1f;
                //Debug.Log(_transform.position);
                tempVerticeArray[cornerVerticeIndices[i, j]] = _transform.InverseTransformPoint(CellMesh.Perturb(cornerVertices[i]));
                if (j >= 3) tempVerticeArray[cornerVerticeIndices[i, j]].y += 1f;

                switch (i)
                {
                    case (0):
                        tempVerticeArray[cornerVerticeIndices[i, j]].x += cornerShift;
                        tempVerticeArray[cornerVerticeIndices[i, j]].z -= cornerShift;
                        break;
                    case (1):
                        tempVerticeArray[cornerVerticeIndices[i, j]].x -= cornerShift;
                        tempVerticeArray[cornerVerticeIndices[i, j]].z -= cornerShift;
                        break;
                    case (2):
                        tempVerticeArray[cornerVerticeIndices[i, j]].x -= cornerShift;
                        tempVerticeArray[cornerVerticeIndices[i, j]].z += cornerShift;
                        break;
                    case (3):
                        tempVerticeArray[cornerVerticeIndices[i, j]].x += cornerShift;
                        tempVerticeArray[cornerVerticeIndices[i, j]].z += cornerShift;
                        break;
                }
            }
        }

        mesh.vertices = tempVerticeArray;
        mesh.RecalculateBounds();
    }

    public void Occupy()
    {
        if (gridObject.entity is Building)
        {
            if (enterCellPointer)
                SmallCellGrid.OccupyPlaceWithBuildingEnter(SCCoord.FromPos(oldPos));
            else
                SmallCellGrid.OccupyPlaceWithBuilding(SCCoord.FromPos(oldPos));
        }
        else if (gridObject.entity is Nature)
        {
            SmallCellGrid.OccupyPlaceWithNature(gridObject.coordinates, gridObject.entity.NtrData.Index);
        }
    }

    public void Free()
    {
        if (gridObject.entity is Building)
        {
            if (enterCellPointer)
                SmallCellGrid.FreePlaceFromBuildingEnter(SCCoord.FromPos(oldPos));
            else
                SmallCellGrid.FreePlaceFromBuilding(SCCoord.FromPos(oldPos));
        }
        else if (gridObject.entity is Nature)
        {
            SmallCellGrid.FreePlaceFromNature(gridObject.coordinates, gridObject.entity.NtrData.Index);
        }
    }

    // Проверка того, в какой SmallCell находится данный CellPointer
    void CheckState()
    {
        if (!enterCellPointer)
        {
            if (!isEnableForBuild && SmallCellGrid.CheckEmptyForBuilding(oldPos) && _parentTransform.position.y > 0.5) // КОСТЫЛЬ _parentTransform.position.y > 0.5
            {
                isEnableForBuild = true;
                ChangeColor(0);
            }
            else if (isEnableForBuild && (!SmallCellGrid.CheckEmptyForBuilding(oldPos) || _parentTransform.position.y <= 0.5))
            {
                isEnableForBuild = false;
                ChangeColor(1);
            }
        }
        else
        {
            if (!isEnableForBuild && SmallCellGrid.CheckEmptyForEnter(oldPos) && _parentTransform.position.y > 0.5)
            {
                isEnableForBuild = true;
                ChangeColor(2);
            }
            else if (isEnableForBuild && (!SmallCellGrid.CheckEmptyForEnter(oldPos) || _parentTransform.position.y < 0.5))
            {
                isEnableForBuild = false;
                ChangeColor(3);
            }
        }
    }

    // Замена цвета CellPointer путем изменения цвета всех вершин в меше.
    void ChangeColor(int colorIndex = 0)
    {
        Color[] meshColors = new Color[mesh.vertexCount];
        for (int i = 0; i < meshColors.Length; i++)
        {
            meshColors[i] = colors[colorIndex];
        }
        mesh.colors = meshColors;
    }
}
