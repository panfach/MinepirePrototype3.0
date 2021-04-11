using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
[CreateAssetMenu(fileName = "ActSequenceData", menuName = "ScriptableObjects/ActSequence")]
public class ActSequence : ScriptableObject
{
    public Profession profession;
    public GeneralActSequenceIndex generalActSequenceIndex;
    public ActionUnit[] actions;

    // additional options ...
}

public enum GeneralActSequenceIndex
{
    NONE,
    EAT
}