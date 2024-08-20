using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    public List<PassiveItem> passiveItems = new List<PassiveItem>();
    public int maxActivePassives = 5;

    public static PassiveItemManager Instance;

    private List<PassiveItem> activePassiveItems = new List<PassiveItem>();

    // Start is called before the first frame update
    void Start()
    {
        
    }
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<PassiveItem> GetPassivesToLevel(int numOfPassives)
    {
        if (numOfPassives > passiveItems.Count)
        {
            numOfPassives = passiveItems.Count;
        }


        if (activePassiveItems.Count >= maxActivePassives)
        {
            return CreatePassiveLevelUpList(activePassiveItems, numOfPassives);
        }
        else
        {
            return CreatePassiveLevelUpList(passiveItems, numOfPassives);
        }
    }

    private List<PassiveItem> CreatePassiveLevelUpList(List<PassiveItem> passives, int numOfPassives)
    {
        List<PassiveItem> passiveList = new List<PassiveItem>();
        int passivesFound = 0;

        HelperFunctions.ShuffleList(ref passives);

        foreach (PassiveItem passive in passives)
        {
            if (passive.GetCurrentPassiveLevel() < 4)
            {
                passiveList.Add(passive);
            }

            passivesFound++;

            if (passivesFound == numOfPassives)
            {
                break;
            }
        }

        return passiveList;
    }

    public void ActivatePassiveItem(PassiveItem item)
    {
        activePassiveItems.Add(item);
        GameManager.Instance.UpdatePassiveHUDUI(item.stat.uiSprite);
    }

    public void IncreasePassiveItemLevel(PassiveItem item)
    {
        item.currentLevel++;
    }

    public bool IsPassiveActive(PassiveItem passive)
    {
        return activePassiveItems.Contains(passive);
    }

    public PassiveItem IsPassiveActiveById(PassiveItemStats.PassiveId id)
    {
        return activePassiveItems.Where(x => x.stat.id.Equals(id)).FirstOrDefault();
    }
}

[Serializable]
public class PassiveItem
{
    public int currentLevel = 0;

    public PassiveItemStats stat;

    public int GetCurrentPassiveLevel()
    {
        return currentLevel;
    }
}
