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
    FINDWORK,
    PUT,
    CONSTRUCT,
    GOTOCREATURE,
    ATTACK,
    FINDTARGET
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




// Сделать охоту на оленей. Охотники должны вместе нападать на одну цель:
//
// Действие GoToCreature: параметр расстояния, специальный мод WORKGROUP (Есть нулевой рабочий (лидер) - он главный. За ним ходят остальные)
// Действие при этом моде заканчивается, когда среднее арифметическое расстояний до оленя всех рабочих меньше параметра расстояния
// Примечание: В самом начале действия GoToCreature запоминаются в отдельный список назначенные охотники.
// Таким образом, если охота уже началась, то внезапно назначенный новый житель не будет учитываться в среднем арифметическом расстояний
// Жители должны ждать друг друга (Лидер идет шагом, если не все охотники близко к нему, иначе бегом)
// (Остальные охотники бегут, если далеко от лидера, идут, если близко к нему)
// В GeneralUI сделать специальные методы перехода на шаг и бег?
