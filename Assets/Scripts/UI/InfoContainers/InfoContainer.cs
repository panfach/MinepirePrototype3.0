using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InfoContainer : MonoBehaviour
{
    public abstract void Set(bool state);
    public abstract void Refresh();
}
