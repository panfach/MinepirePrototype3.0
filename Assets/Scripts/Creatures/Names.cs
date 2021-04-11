using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Names : MonoBehaviour
{
    public string[] maleNames;
    //public string[] femaleNames

    //Random rand = new Random();

    public string GetRndName(bool gender)
    {
        return maleNames[(int)Random.Range(0f, maleNames.Length)];
    }
}
