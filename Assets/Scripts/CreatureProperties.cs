using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureProperties : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] Entity entity;

    [Header("Settings")]
    [SerializeField] float height;

    [Header("Info")]
    [SerializeField] string _name;
    [SerializeField] bool gender;
    [SerializeField] float age;
    [SerializeField] Building placeOfStay;                                              // ??? Maybe create separate class for this
    public Item[] droppedItems;                                                         // temporal

    public string Name { get => _name; }
    public bool Gender { get => gender; }
    public float Age { get => age; }
    public Vector3 HeightVector { get => new Vector3(0f, height / 2, 0f); }
    public Building PlaceOfStay
    {
        get => placeOfStay;
        set
        {
            placeOfStay = value;
        }
    }
    public GameObject[] DroppedItemsAsGameObjects
    {
        get
        {
            GameObject[] arr = new GameObject[droppedItems.Length];
            for (int i = 0; i < droppedItems.Length; i++) arr[i] = droppedItems[i].gameObject;
            return arr;
        }
    }


    public void Init(bool _gender, float _age)
    {
        gender = _gender;
        age = _age;
    }

    public void Init(bool _gender, string __name, float _age, Building _home = null, Building _work = null, float _satiety = 2.0f)
    {
        gender = _gender;
        _name = __name;
        age = _age;
        if (entity.Appointer != null) entity.Appointer.Appoint(_home.Appointer);
        if (entity.Appointer != null) entity.Appointer.Appoint(_work.Appointer);
        satiety = _satiety;

        profession = Profession.NONE;
        workSequence = null;
        villagerAgent = null;
        defaultDamage = 1.0f;
    }
}
