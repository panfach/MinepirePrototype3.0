using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceQuery
{
    public ResourceIndex[] index;
    public ResourceType[] type;

    public float[] indexVal;
    public float[] typeVal;

    public ResourceQuery(ResourceIndex _ind, float _val)
    {
        index = new ResourceIndex[1] { _ind };
        indexVal = new float[1] { _val };
    }

    public ResourceQuery(ResourceType _type, float _val)
    {
        type = new ResourceType[1] { _type };
        typeVal = new float[1] { _val };
    }

    public ResourceQuery(ResourceIndex[] _ind, float[] _val)
    {
        index = _ind;
        if (_val.Length < _ind.Length)
            indexVal = new float[_ind.Length];
        else
            indexVal = _val;
    }

    public ResourceQuery(ResourceType[] _type, float[] _val)
    {
        type = _type;
        if (_val.Length < _type.Length)
            typeVal = new float[_type.Length];
        else
            typeVal = _val;
    }

    public ResourceQuery(ResourceIndex[] _ind, float[] _indVal, ResourceType[] _type, float[] _typeVal)
    {
        index = _ind;
        type = _type;
        if (_indVal.Length < _ind.Length)
            indexVal = new float[_ind.Length];
        else
            indexVal = _indVal;
        if (_typeVal.Length < _type.Length)
            typeVal = new float[_type.Length];
        else
            typeVal = _typeVal;
    }
}
