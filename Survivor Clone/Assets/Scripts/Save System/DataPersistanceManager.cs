using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager Instance;

    private List<IDataPersistance> dataPersistenceObjects;

    private AccountData accountData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetDataPeristanceObjects();
        LoadAccountData();
    }

    public void NewAccountData()
    {
        accountData = new AccountData();
    }

    public void LoadAccountData()
    {
        // load save data from file
        accountData = FileDataHandler.LoadData();

        // if no data found, initialize new data
        if (accountData == null)
        {
            Debug.Log("No data found. Initializing to defaults");
            NewAccountData();
        }

        // push the loaded data to all other scripts that use it
        foreach (IDataPersistance dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadAccountData(accountData);
        }
    }

    public void SaveAccountData()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistance dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveAccountData(ref accountData);
        }

        // save the data to a file using the data handler
        FileDataHandler.SaveData(accountData);
    }

    private void OnApplicationQuit()
    {
        SaveAccountData();
    }

    public void SetDataPeristanceObjects()
    {
        dataPersistenceObjects = FindAllDataPersistanceObjects();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
