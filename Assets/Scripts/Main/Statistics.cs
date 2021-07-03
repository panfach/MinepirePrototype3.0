using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    [SerializeField] float[] receivedResources;

    public event SimpleEventHandler changedEvent;

    public float ReceivedResource(ResourceIndex resInd) { return receivedResources[(int)resInd]; }
    public void ChangeReceivedResource(ResourceIndex resInd, float amount)
    {
        if (resInd == ResourceIndex.NONE || resInd == ResourceIndex.EXECUTEDQUERY) return;

        receivedResources[(int)resInd] += amount;
        receivedResources[(int)resInd] = Mathf.Clamp(receivedResources[(int)resInd], 0f, 999999f);
        changedEvent?.Invoke();
    }


    void OnEnable()
    {
        receivedResources = new float[Enum.GetNames(typeof(ResourceIndex)).Length];
    }


    public bool CheckResources(ResourceIndex ind, float amount)
    {
        return receivedResources[(int)ind] >= amount;
    }

    public bool CheckResources(ResourceQuery query)
    {
        for (int i = 0; i < query.index.Length; i++)
        {
            if (!CheckResources(query.index[i], query.indexVal[i])) return false;
        }
        return true;
    }
}
