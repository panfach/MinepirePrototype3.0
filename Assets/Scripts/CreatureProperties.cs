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
    [SerializeField] bool gender;
    [SerializeField] float age;
    public Item[] droppedItems;                                                         // temporal

    public Vector3 HeightVector { get => new Vector3(0f, height / 2, 0f); }
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
}
