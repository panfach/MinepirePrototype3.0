using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
[CreateAssetMenu(fileName = "ResourceData", menuName = "ScriptableObjects/Resource")]
public class ResourceData : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField] string _name_rus;
    [SerializeField] ResourceIndex index;
    [SerializeField] ResourceType[] type;
    [Space][SerializeField] int weight;
    [SerializeField] int size;

    public string Name { get => _name; }
    public string Name_rus { get => _name_rus; }
    public ResourceIndex Index { get => index; }
    public ResourceType Type(int i) => type[i];
    public int TypeLength { get => type.Length; }
    public int Weight { get => weight; }
    public int Size { get => size; }
}
