using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScene
{
    DataDefinition GatDataID();
    void RegisterSceneSaveData() => DataManager.Instance.RegisterSceneSaveDate(this);
    void UnRegisterSceneSaveData() => DataManager.Instance.UnRegisterSceneSaveDate(this);
    void sceneSaveData(Data data);
    void sceneLoadData(Data data);
    //,bool state
}