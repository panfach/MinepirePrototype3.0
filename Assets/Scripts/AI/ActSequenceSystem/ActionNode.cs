using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class ActionNode : Node
{
    abstract public ActionType Type { get; }
    abstract public IEnumerator Algorithm(Creature creature);
}

public enum ActionType
{
    NONE,
    START,
    RNDWALK,
    GOTOBUILDING,                             // Add modes: EnterPoint, InteractionPoint, Near
    ENTERBUILDING,
    ISENDOFDAY,
    SWITCHSEQ,
    GETBUILDING,                       
    EQUALITY,
    EXITBUILDING,
    REST,
    CHECKINDICATOR,
    GETQUERY,
    FINDBUILDING,
    TAKE,
    WAIT,
    GOTOINTERACTION,
    INTERACT,
    CHECK,
    MATHCOMPARING,
    GETAMOUNT,
    RANDOMGENERATOR,
    FINDLABORERWORK
}





public enum ActionType_old
{
    NONE,
    START,
    GOTOWORK,                // Идти в рабочее здание
    TAKE,                    // Пойти за указанным предметом и взять его
    EXITHOUSE,               // Выйти из текущего здания
    FIND,                    // Найти указанное животное, здание, ресурс
    KILLANIMAL,              // Убить указанное животное
    FINDLABORERWORK,         // Найти свободную работу для разнорабочего
    BUILD,                   // Строить указанное здание
    GOTOWARE,                // Идти на рабочий склад
    FREEINV,                 // Освободить инвентарь
    EXTRACT,                 // Добыть ресурс
    EAT,
    GOTOPOINT,
    CHECK,
    ENTERBUILDING,
    RNDWALK
}
