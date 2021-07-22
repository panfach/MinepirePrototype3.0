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
    [SerializeField] CreatureContainer placeOfStay;                                              // ??? Maybe create separate class for this    // No, it's no need

    public string Name { get => _name; }
    public bool Gender { get => gender; }
    public float Age { get => age; }
    public Vector3 HeightVector { get => new Vector3(0f, height / 2, 0f); }
    public CreatureContainer PlaceOfStay
    {
        get => placeOfStay;
        set
        {
            placeOfStay = value;
        }
    }


    public void Init(bool _gender, string __name, float _age, Building _home = null, Building _work = null, float _satiety = 2.0f, float _health = -1f)       // There is no need to have default values
    {
        gender = _gender;
        _name = __name;
        age = _age;
        if (entity.Appointer != null && _home != null) entity.Appointer.Appoint(_home.Appointer);
        if (entity.Appointer != null && _work != null) entity.Appointer.Appoint(_work.Appointer);
        entity.Satiety.Value = _satiety;
        entity.Health.Value = (_health == -1f) ? entity.CrtData.MaxHealth : _health;
        //if (entity.CrtData.Index == CreatureIndex.DEER) Debug.Log("_health = " + _health + ". entity.CrtData.MaxHealth = " + entity.CrtData.MaxHealth);
    }
}
