using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUIState : UIState
{

    [SerializeField] private Button backButton;
    [SerializeField]private bool startGame;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private List<CustomerData> spawnedCustomers = new List<CustomerData>();
    private int maxAmountOfCustomers = 3;

    public override void OnInit()
    {
        base.OnInit();
        backButton.onClick.AddListener(Back);
        SGameManager.Instance.OnNewCustomers.AddListener(StartCustomerPipeline);
    }

    private void StartCustomerPipeline(CustomerData currentCustomer,List<CustomerData> customerData)
    {

        if (currentCustomer != null)
        {
            Debug.Log("StartPipeline");
        }
        else
        {
            SpawnRemainingCustomer(customerData);
        }
    }


    private void SpawnRemainingCustomer(List<CustomerData> customerData)
    {
        int toSpawnAmountOfCustomers = maxAmountOfCustomers-spawnedCustomers.Count;

        for (int i = 0; i< toSpawnAmountOfCustomers; i++)
        {
            if(i > customerData.Count-1)
            {
                break;
            }
            CustomerData toSpawnCustomerData = customerData[i];
            Transform spawnPoint = spawnPoints[spawnedCustomers.Count];
            GameObject customer = Instantiate(toSpawnCustomerData.CustomerPrefab);
            customer.transform.SetParent(spawnPoint);
            customer.transform.localPosition = Vector3.zero;
            customer.transform.localScale = Vector3.one;
            spawnedCustomers.Add(toSpawnCustomerData);
        }
    }


    public override void OnEnter()
    {
        base.OnEnter();
        if (startGame)
        {
            SGameManager.Instance.StartGame();
        }
    }

    private void Back()
    {
        SUIManager.Instance.ChangeUIState("Mask");
    }


}
