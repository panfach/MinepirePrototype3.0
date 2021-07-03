using System.Linq;
using UnityEngine;
using System;
using System.Collections;

// ------------------------------------------- // MINEPIRE // ------------------------------------------- //
[CreateAssetMenu(fileName = "ResourceSourceData", menuName = "ScriptableObjects/ResourceSource")]
public class NatureData : ScriptableObject
{
    [System.Serializable]
    [SerializeField] struct ExtractedResource
    {
        public ResourceData resource;
        public int amount;
        public float laborIntensity;
        public float recoverySpeed;
        public bool restorable;
        public bool sourceDeletionAfterExhaustion;
    }

    [SerializeField] string _name;
    [SerializeField] string _name_rus;
    [SerializeField] NatureIndex index;
    [SerializeField] ExtractedResource[] resources;
    [SerializeField] NatureIndex[] friends;

    public IEnumerable ResData() { return from item in resources select item.resource; }
    public ResourceData ResData(int i) => resources[i].resource;
    public IEnumerable Amount() { return from item in resources select item.amount; }
    public int Amount(int i) => resources[i].amount;
    public float LaborIntensity(int i) => resources[i].laborIntensity;
    public float RecoverySpeed(int i) => resources[i].recoverySpeed;
    public bool Restorable(int i) => resources[i].restorable;
    public bool DeletionAfterExhaustion(int i) => resources[i].sourceDeletionAfterExhaustion;
    public string Name { get => _name; }
    public string Name_rus { get => _name_rus; }
    public NatureIndex Index { get => index; }
    public int ResourceDepositSize { get => resources.Length; }


    public static bool CheckFriends(NatureIndex[] mainIndices, NatureIndex index)
    {
        foreach (NatureIndex mainIndex in mainIndices)
        {
            if (!DataList.GetNature(mainIndex).CheckFriend(index)) return false;
        }
        return true;
    }

    public bool CheckFriend(NatureIndex index)
    {
        return Array.Exists(friends, i => i == index);
    }
}
