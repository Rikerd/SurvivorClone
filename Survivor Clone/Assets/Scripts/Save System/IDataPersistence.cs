using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    public void LoadAccountData(AccountData accountData);
    public void SaveAccountData(ref AccountData accountData);
}
