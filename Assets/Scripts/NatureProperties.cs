using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureProperties : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] Entity entity;

    [Header("Settings")]
    public Transform mainModel;


    private void Awake()
    {
        if (mainModel == null) mainModel = transform.GetChild(0);
    }


    public void Die()                                                            // Maybe all Die fuctions must be in "Properties" (In this case, in new class NatrureProperties)
    {
        // Need to clear place
        Connector.natureManager.RemoveItem(entity as Nature);

        Destroy(gameObject);
    }
}
