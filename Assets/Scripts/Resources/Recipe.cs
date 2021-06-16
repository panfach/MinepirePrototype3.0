using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
    public int[] interactionSpotIndex;
    public float laborIntensity = 1f;
    public ResourceQuery requiredRes;
    public ResourceQuery receivedRes;

    Production production;
    int queue;
    bool process;
    bool harvest;
    float progress;
    GeneralAI worker;

    public InteractionSpot Spot() { return production.entity.Interactive.Spot(interactionSpotIndex[0]); }
    public InteractionSpot Spot(int i) { return production.entity.Interactive.Spot(interactionSpotIndex[i]); }
    public bool Process { get => process; set { process = value; } }
    public bool Harvest { get => harvest; set { harvest = value; } }
    public bool NeedToProduce { get => (queue > 0 && process == false && harvest == false && !IsOccupied); }
    public bool NeedToReap { get => (harvest == true && !IsOccupied); }
    public bool IsOccupied { get => (worker != null); }
    public int Queue
    {
        get => queue;
        set
        {
            queue = value;
            queue = Mathf.Clamp(queue, 0, 20);
        }
    }
    public float Progress
    {
        get => progress;
        set
        {
            progress = value;
            progress = Mathf.Clamp01(progress);
        }
    }


    public void Init(Production _production)
    {
        production = _production;
    }

    public void Occupy(GeneralAI _worker)
    {
        worker = _worker;
        worker.DestRecipe = this;
        worker.DestEntity = production.entity;
    }

    public void RemoveOccupation()
    {
        worker.DestRecipe = null;
        worker = null;
    }

    public void StopProcess()
    {
        Process = false;
        Progress = 0f;
        Harvest = true;
    }
}


// Recipe должен иметь ссылку на InteractionSpot
// Начать писать алгоритм поведения в лоб. Действие LookForJob должно выдавать на выбор последовательность действий: последовательность строительства, производства, жатвы и тд
// Если один человек уже занимается данным Recipe, то как это поймет другой житель:
// Другой житель перебирает список Productions, если InteractionSpot.actor != null, значит этот recipe занят, пропускаем его. 
// Каждый recipe может иметь ровно один привязанный к нему InteractionSpot
// Сделать  новый enum "действие". Он будет характеризовать действие, с которым хочет взаимодействовать житель
// То есть, при взаимодействии жителя с InteractionSpot, у жителя есть например выбор: производство или жатва. Этот enum должен вырабатываться заранее.
// В зависимости от того, с каким действием пришел житель, метод Interact будет работать по разному.
// Судя по всему, InteractionSpot также должен иметь ссылку на Recipe. Только так метод Interact поймет, что делать (С каким именно recipe взаимодействовать)
// Житель, пришедший с производнством, должен сначала выгрузить инвентарь. То есть, это отдельный вид взаимодействия, соответственно отельная переменная enum inventoryInteractionPut и Take
// КОРОЧЕ. У каждого InteractionPoint есть набор флагов, который означает допустимые действия с классом interactive. А житель может прийти с абсолютно любым действием.
// Задача в том, чтобы житель не смог например сделать жатву в здании, где флаг жатвы выключен в допустимых действиях.
// Итак продолжаем. Житель приходит с действием inventoryInteractionPut. Вызывается метод Interact, который вызывает Interact из самого Interactive, и передает параметром желаемое действие
// Там уже происходит проверка, можно ли это действие выполгить. И если можно, то StartCoroutine соответсвтующий.
// В будущем еще потребуется сделать ограничения на инвентари, чтобы житель не смог положить в сушилку для шкур что то лишнее, что у него случайно оказалось,
// но наверное это необязательно.



// Нужно пределать Recipe и InteractionSpot
// InteractionSpot сам должен определять, какой тип взаимодействия. А после вызывать метод из interactive, например IndicatorInteractionProcess
// (Все, что делает этот метод - просто вызов корутина и все).
// МИНИМИЗИРОВАТЬ вызовы чего либо из InteractionSpot в Interactive (И вызовы из Recipe в Production). И еще мысль: ЗАПРЕТИТЬ взаимодействовать с interactionSpot из вне класса interactive
// Сравнить классы Recipe и InteractionSpot. Они слишком похожи, как так? Нужно что то сделать
// 