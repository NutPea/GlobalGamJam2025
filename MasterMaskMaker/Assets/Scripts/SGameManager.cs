using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SGameManager : MonoBehaviour
{
    public static SGameManager Instance;

    [SerializeField] private List<Customer> customers;
    private int currentAmountOfDoneCustomers = 0;
    private void Awake()
    {
        Instance = this;
        foreach(Customer customer in customers)
        {
            CustomerData copyData = Instantiate(customer.customerData);
            customer.customerData = copyData;
        }
    }
    

    public CustomerData currentCustomer;
    public List<Tool> UsedTools = new List<Tool>();

    public UnityEvent<CustomerData,List<CustomerData>> OnNewCustomers = new UnityEvent<CustomerData,List<CustomerData>>();

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

    public void StartGame()
    {
        CheckForMoreCustomers();
    }

    public void GiveMask()
    {
        SUIManager.Instance.ChangeUIState("Customer");
        currentAmountOfDoneCustomers++;
        
        CheckForMoreCustomers();
        AllCusomersDone();
    }

    private void CheckForMoreCustomers()
    {
        List<CustomerData> foundCustomers = new List<CustomerData>();
        foreach(Customer customer in customers)
        {
            if(customer.amountTrigger <= currentAmountOfDoneCustomers)
            {
                if (!customer.CustomerDone)
                {
                    foundCustomers.Add(customer.customerData);
                }
            }
        }
        Debug.Log("Check");
        OnNewCustomers.Invoke(currentCustomer,foundCustomers);
        currentCustomer = null;
    }

    private bool AllCusomersDone()
    {
        foreach(Customer customer in customers)
        {
            if (customer.CustomerDone)
            {
                return false;
            }
        }

        return true;
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
