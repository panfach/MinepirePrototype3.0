using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
[System.Serializable]
public class ActionUnit
{
    //[Space(50)]
    public ActionType type;
    public ActionMode mode;
    [Range(0, 100)] public int priority;
    public ScriptableObject[] searchTarget;
    public SequenceAction[] reaction;
    public int[] actVar;
    [Range(0.0f, 5.0f)] public float afterDelay;

}

public enum ActionType
{
    NONE,
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
    CHECK
}

public enum SequenceAction
{
    NONE,
    NEXT,
    PREV,
    GOTO,
    ANEW,
    SHIFT
}

public enum ActionMode
{
    DEFAULT,
    LOCAL,
    WIDE,
    INVISEMPTY,
    TARGETINVISEMPTY,
    PUT,
    TAKE
}