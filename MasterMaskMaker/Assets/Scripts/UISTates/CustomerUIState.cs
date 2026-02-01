using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUIState : UIState
{

    [SerializeField] private Transform spawnPoint;

    private bool hasNewCustomers;
    private CustomerData currentCustomer;
    private CustomerData newCustomer;
    private bool gameEnd;
    public override void OnInit()
    {
        base.OnInit();
        SGameManager.Instance.OnNewCustomers.AddListener(ToChange);
        SGameManager.Instance.OnFinishedGame.AddListener(EndGame);
    }

    private void ToChange(CustomerData currentCustomer, CustomerData newCustomer)
    {

        this.hasNewCustomers = true;
        this.currentCustomer = currentCustomer;
        this.newCustomer = newCustomer;
    }


    private void EndGame()
    {
        gameEnd = true;
    }
    private void StartCustomerPipeline(CustomerData currentCustomer)
    {

        if (currentCustomer != null)
        {
            currentCustomer.SpawnedCustomer.GetComponent<CustomerHandler>().FadeOut();         
        }

        Invoke(nameof(SpawnRemainingCustomer), 1.5f);
    }



    private void SpawnRemainingCustomer()
    {
        if (gameEnd)
        {
            SUIManager.Instance.ChangeUIState("EndDay");
            Debug.Log("EndDay");
        }
        else
        {
            GameObject customer = Instantiate(newCustomer.CustomerPrefab);
            customer.transform.SetParent(spawnPoint);
            customer.transform.localPosition = Vector3.zero;
            customer.transform.localScale = Vector3.one;
            newCustomer.SpawnedCustomer = customer;
            CustomerHandler customerHandler = customer.GetComponent<CustomerHandler>();
            customerHandler.Setup(newCustomer);
            customerHandler.FadeIn();
        }

    }


    public override void OnEnter()
    {
        base.OnEnter();

        if (hasNewCustomers)
        {
            StartCustomerPipeline(currentCustomer);
            hasNewCustomers = false;
        }

        if (gameEnd)
        {
            Invoke(nameof(EndGameChangeUIState), 1.5f);
        }
    }

    private void EndGameChangeUIState() { 
        SUIManager.Instance.ChangeUIState("EndDay");
        Debug.Log("EndDay");
    }



}
