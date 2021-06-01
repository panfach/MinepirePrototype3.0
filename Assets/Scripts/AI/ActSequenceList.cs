using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActSequenceSystem;
using System;

public class ActSequenceList : MonoBehaviour
{
    [Header("Array of act sequences")]
    public ActSequenceGraph[] actSequences;

    //static Dictionary<GeneralActSequenceIndex, ActSequence> generalDict = new Dictionary<GeneralActSequenceIndex, ActSequence>();
    static Dictionary<ActSequenceIndex, ActSequenceGraph> actSeqDict = new Dictionary<ActSequenceIndex, ActSequenceGraph>();

    private void Awake()
    {
        //foreach (ActSequence item in generalSequences)
        //{
        //    generalDict.Add(item.generalActSequenceIndex, item);
        //}

        foreach (ActSequenceGraph item in actSequences)
        {
            actSeqDict.Add(item.Index, item);
        }
    }

    public static ActSequenceGraph GetSequence(ActSequenceIndex ind) => (ActSequenceGraph)actSeqDict[ind].Copy();
}
