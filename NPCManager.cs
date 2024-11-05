using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour, IScene
{
    public GameObject NPCData{get; private set;}
    //private Data saveData;
    public bool state;
    /*void Start()
    {
        // 檢查物件是否已被標記為關閉
        *//*if (objectStates.ContainsKey(objectGuid) && !objectStates[objectGuid])
        {
            gameObject.SetActive(false);
        }*//*
        var sceneObjID = sceneObj.GetComponent<DataDefinition>().ID;
        if (saveData.closedObjects.ContainsKey(GatDataID().ID) && !saveData.closedObjects[GatDataID().ID])
        {
            gameObject.SetActive(false);//data.closedObjects[GatDataID().ID] = false;
        }
    }*/
    /*private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }*/
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
        // Save the state of objects
        foreach (var obj in FindObjectsOfType<DataDefinition>())
        {
            data.closedObjects[obj.ID] = !obj.gameObject.activeSelf;
        }
        /*if (!data.closedObjects.ContainsKey(GatDataID().ID))
        {
            data.closedObjects.Add(GatDataID().ID, state);
        }
        else
        {
            data.closedObjects.Remove(GatDataID().ID);
            //gameObject.SetActive(data.closedObjects[GatDataID().ID]);
            //data.closedObjects[GatDataID().ID] = state;
        }*/
        /*foreach (var objName in data.closedObjects.Keys)
        {
            GameObject objToSet = GameObject.Find(GatDataID().ID);
            if (objToSet != null)
            {
                objToSet.SetActive(data.closedObjects[GatDataID().ID]);
            }
        }*/
        //gameObject.SetActive(false);
        /*//data.deletedObjects.Add(GatDataID().ID, bailey);
        if (data.deletedObjects.ContainsKey(GatDataID().ID))
        {
            data.deletedObjects.Remove(GatDataID().ID);
            data.characterPosDict[GatDataID().ID] = transform.position;
            data.floatSaveData[GatDataID().ID + "moveTo"] = this.moveTime;
        }
        else
        {
            data.deletedObjects.Add(GatDataID().ID, bailey);
            data.characterPosDict.Add(GatDataID().ID, transform.position);
            data.floatSaveData.Add(GatDataID().ID + "moveTo", this.moveTime);
        }*/
        /*if (data.deletedObjects.ContainsKey(GatDataID().ID))
        {
        }*/
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

        /*data.closedObjects.TryGetValue(GatDataID().ID, out state);
        if (state)
        {
            gameObject.SetActive(false);
        }*/
        /*if (data.closedObjects.ContainsKey(GatDataID().ID))
        {
            sceneToLoad = data.GetGameScene();
            gameObject.SetActive(false);//data.closedObjects[GatDataID().ID] = false;
            //data.closedObjects.Add(GatDataID().ID, state);
        }*/
        //gameObject.SetActive(data.closedObjects[GatDataID().ID]);GatDataID().ID != null&& 

        /*foreach (var item in data.closedObjects)
        {
            GameObject objToSet = FindObjectByGuid(item.Key);
            if (objToSet != null)
            {
                objToSet.SetActive(item.Value);
            }
        }
        var sceneObjID = sceneObj.GetComponent<DataDefinition>().ID;
        if (data.closedObjects.ContainsKey(sceneObjID) && !data.closedObjects[GatDataID().ID])
        {
            gameObject.SetActive(false);//data.closedObjects[GatDataID().ID] = false;
        }*/

        /*if (data.closedObjects.ContainsKey(GatDataID().ID) && !data.closedObjects[GatDataID().ID])
        {
            gameObject.SetActive(false);
        }*/
        /*if (data.closedObjects.ContainsKey(GatDataID().ID) && data.closedObjects[GatDataID().ID])
        {
            //data.closedObjects[GatDataID().ID] = true;
            gameObject.SetActive(false);
        }*/
        /*data.deletedObjects.TryGetValue(GatDataID().ID, out bailey);
        if (bailey)
        {
            BaileyRun.SetActive(false);
            data.characterPosDict[GatDataID().ID] = transform.position;
            data.floatSaveData[GatDataID().ID + "moveTo"] = this.moveTime;
        }
        if (data.characterPosDict.ContainsKey(GatDataID().ID))
        {
            transform.position = data.characterPosDict[GatDataID().ID];
            moveTime = data.floatSaveData[GatDataID().ID + "moveTo"];
        }*/
    }
    private GameObject FindObjectByGuid(string guid)
    {
        // 根據GUID查找物件
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            var manager = obj.GetComponent<NPCManager>();
            if (manager != null && manager.GatDataID().ID == guid)
            {
                return obj;
            }
        }
        return null;
    }
    // Method to handle interaction with NPC
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
