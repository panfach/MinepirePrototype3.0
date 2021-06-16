using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOrder
{
    /*WorkOrderType type;
    Entity target;
    
    public bool CanDo              // In fututre production buildings can have several interaction points
    {
        get => false;
    }


    public WorkOrder(WorkOrderType _type, Entity _target)
    {
        type = _type;
        _target = target;
    }


    public void Occupy()
    {

    }

    public bool CanDo()
    {
        if (occupied) return false;

        switch (type)
        {
            case WorkOrderType.PRODUCE:
                
                break;
        }
    }*/
}

public enum WorkOrderType
{
    CONSTRUCT,
    PRODUCE,
    EXTRACT,
    REAP
}
