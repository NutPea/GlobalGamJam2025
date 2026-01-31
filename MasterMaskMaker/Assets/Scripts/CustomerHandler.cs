using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomerHandler : MonoBehaviour
{

    private CustomerData data;
    private Button button;

    public void Setup(CustomerData data)
    {
        this.data = data;
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeToCustomer);
    }

    private void ChangeToCustomer()
    {
        SGameManager.Instance.SetCustomer(data);
    }
}
