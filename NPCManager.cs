using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour, IScene
{
    public GameObject NPCData{get; private set;}
    //private Data saveData;
    public bool state;
    
    private void OnEnable()
    {
        //newGameEvent.OnEventRaised += NewGame;
        IScene saveable = this;
        saveable.RegisterSceneSaveData();
    }

    private void OnDisable()
    {
        //newGameEvent.OnEventRaised -= NewGame;
        IScene saveable = this;
        saveable.UnRegisterSceneSaveData();
    }

    public DataDefinition GatDataID()
    {
        return GetComponent<DataDefinition>();
    }
    /*public void SetObjectState(Data data, bool state)
    {
        // 標記物件狀態
        if (!data.closedObjects.ContainsKey(GatDataID().ID))
        {
            data.closedObjects.Add(GatDataID().ID, state);
        }
        else
        {
            data.closedObjects[GatDataID().ID] = state;
        }
    }*/

    public void sceneSaveData(Data data)
    {
        // Save the state of objects //copilot給的代碼之一，測試沒任何反應
        foreach (var obj in FindObjectsOfType<DataDefinition>())
        {
            data.closedObjects[obj.ID] = !obj.gameObject.activeSelf;
        }
        //--------------------------------------
        //嘗試用教學寫的，變成全存字典，試用把方法改成(Data data,bool state)，下面的true改成state也無用(如上面陂註釋的SetObjectState)
        if (!data.closedObjects.ContainsKey(GatDataID().ID))
        {
            data.closedObjects.Add(GatDataID().ID, true);
        }
        else
        {
            data.closedObjects.Remove(GatDataID().ID);
            //gameObject.SetActive(data.closedObjects[GatDataID().ID]);
            //data.closedObjects[GatDataID().ID] = true;
        }
    }

    public void sceneLoadData(Data data)
    {
        // Load the state of objects
        foreach (var obj in FindObjectsOfType<DataDefinition>())
        {
            if (data.closedObjects.ContainsKey(obj.ID))
            {
                obj.gameObject.SetActive(!data.closedObjects[obj.ID]);
            }
        }
    }
    private GameObject FindObjectByGuid(string guid)
    {
        // 根據GUID查找物件 //copilot給的代碼之一，測試沒任何反應
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            var manager = obj.GetComponent<NPCManager>();
            if (manager != null && manager.GatDataID().ID == guid)
            {
                return obj;
            }
        }
        return null;
        //--------------------------------------
        //copilot給的代碼之一，變成存在字典的全關
        if (data.closedObjects.ContainsKey(GatDataID().ID) && !data.closedObjects[GatDataID().ID])
        {
            gameObject.SetActive(false);
        }
    }
    // Method to handle interaction with NPC //copilot給的代碼之一，沒反應
    public void InteractWithNPC(GameObject npc)
    {
        var npcData = NPCData.GetComponent<DataDefinition>().ID;
        if (npcData != null)
        {
            NPCData.SetActive(false);
            DataManager.Instance.saveData.closedObjects[GatDataID().ID] = true;
        }
    }
}
