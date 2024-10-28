using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    DataDefinition GatDataID();
    void RegisterSaveData() => DataManager.Instance.RegisterSaveDate(this);
    void UnRegisterSaveData() => DataManager.Instance.UnRegisterSaveDate(this);
    void GetSaveData(Data data);
    void LoadData(Data data);
}