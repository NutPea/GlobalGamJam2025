using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Customer", order = 1)]
public class CustomerData : ScriptableObject
{
    public MaskShapeTool mask;

    public Tool secondaryDetail;

    public Tool minorDetail;

    public GameObject SpawnedCustomer;

    public GameObject CustomerPrefab;

    public float time = 10f;

}
