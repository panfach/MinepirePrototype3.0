using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
[CreateAssetMenu(fileName = "ActSequenceData", menuName = "ScriptableObjects/ActSequence")]
public class ActSequence : ScriptableObject
{
    public Profession profession;
    public ActSequenceIndex generalActSequenceIndex;
    public ActionUnit[] actions;

    // additional options ...
}