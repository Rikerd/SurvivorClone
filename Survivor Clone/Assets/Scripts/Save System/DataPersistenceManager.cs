using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager Instance;

    private List<IDataPersistence> dataPersistenceObjects;

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
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadAccountData(accountData);
        }
    }

    public void SaveAccountData()
    {
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveAccountData(ref accountData);
        }

        // save the data to a file using the data handler
        FileDataHandler.SaveData(accountData);
    }

    // Only save data on explict cases
    //private void OnApplicationQuit()
    //{
    //    SaveAccountData();
    //}

    public void SetDataPeristanceObjects()
    {
        dataPersistenceObjects = FindAllDataPersistanceObjects();
    }

    private List<IDataPersistence> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistanceObjects);
    }
}
