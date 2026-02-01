using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SGameManager : MonoBehaviour
{
    public static SGameManager Instance;

    [SerializeField] private List<Customer> customers;
    private int currentCustomerIndex = 0;
    [SerializeField] private Transform mask;
    [SerializeField] private bool startGame = true;
    private void Awake()
    {
        Instance = this;
        foreach(Customer customer in customers)
        {
            CustomerData copyData = Instantiate(customer.customerData);
            customer.customerData = copyData;
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        if (startGame)
        {
            //StartGame();
        }
    }

    public CustomerData currentCustomer;
    public List<Tool> UsedTools = new List<Tool>();

    public UnityEvent<CustomerData,CustomerData> OnNewCustomers = new UnityEvent<CustomerData,CustomerData>();
    public UnityEvent OnFinishedGame = new UnityEvent();
    public UnityEvent OnChangeCustomer = new UnityEvent();

    public List<GameObject> copiedCustomers = new List<GameObject>();

    public void AddTool(Tool tool)
    {
        UsedTools.Add(tool);
    }

    public void TryRemoveTool(Tool tool)
    {
        if(tool == null)
        {
            return;
        }

        if (UsedTools.Contains(tool))
        {
            UsedTools.Remove(tool);
        }
    }

    public void RemoveAll()
    {
        UsedTools.Clear();
    }

    public void StartGame()
    {
        currentCustomerIndex = 0;
        CheckForMoreCustomers();
    }

    public void GiveMask()
    {
        GameObject maskCopy = Instantiate(mask.gameObject);
        Transform maskPosition = currentCustomer.SpawnedCustomer.GetComponent<CustomerHandler>().maskPosition;
        maskCopy.transform.SetParent(maskPosition);
        maskCopy.transform.localPosition = Vector3.zero;
        RectTransform rect = maskCopy.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(770.5001f, 617.7f);


        // Optional: zentrieren
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = new Vector3(0.25f,0.25f,1f);

        GameObject copiedCustomer = Instantiate(currentCustomer.SpawnedCustomer);
        copiedCustomers.Add(copiedCustomer);
        copiedCustomer.gameObject.SetActive(false);


        CheckForMoreCustomers();
    }



    private void CheckForMoreCustomers()
    {

        if (currentCustomerIndex < customers.Count)
        {

            OnNewCustomers.Invoke(currentCustomer,customers[currentCustomerIndex].customerData);
            currentCustomerIndex++;
        }
        else
        {
            OnFinishedGame.Invoke();
        }
            SUIManager.Instance.ChangeUIState("Customer");
    }

    public void SetCustomer(CustomerData customerData)
    {
        if(customerData != currentCustomer)
        {
            currentCustomer = customerData;
            OnChangeCustomer.Invoke();
        }

        SUIManager.Instance.ChangeUIState("Mask");
    }

}

[System.Serializable]
public class Customer
{
    public CustomerData customerData;
    public int amountTrigger = 0;
    private bool hasBeenSpawned = false;
    public bool CustomerDone = false;
}
