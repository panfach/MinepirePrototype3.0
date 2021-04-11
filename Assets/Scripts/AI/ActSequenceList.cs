using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActSequenceList : MonoBehaviour
{
    [Header("Array of general act sequences")]
    public ActSequence[] generalSequences;
    [Header("Array of work sequences")]
    public ActSequence[] workSequences;

    static Dictionary<GeneralActSequenceIndex, ActSequence> generalDict = new Dictionary<GeneralActSequenceIndex, ActSequence>();
    static Dictionary<Profession, ActSequence> workDict = new Dictionary<Profession, ActSequence>();

    private void Awake()
    {
        foreach (ActSequence item in generalSequences)
        {
            generalDict.Add(item.generalActSequenceIndex, item);
        }

        foreach (ActSequence item in workSequences)
        {
            workDict.Add(item.profession, item);
        }
    }

    public static ActSequence GetGeneralSequence(GeneralActSequenceIndex ind) => generalDict[ind];
    public static ActSequence GetWorkSequence(Profession prof) => workDict[prof];
}
