using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    [Header("®∆•Û∫ ≈•")]
    public VoidEventSO saveDataEvent;

    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        saveData = new Data();
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        //EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        //EventHandler.AfterSceneUnloadEvent += OnAfterSceneUnloadEvent;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        //EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        //EventHandler.AfterSceneUnloadEvent -= OnAfterSceneUnloadEvent;
    }
    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    /*private void OnBeforeSceneUnloadEvent()
    {
        foreach (var Itme in FindAnyObjectByType<Itme>())
        {
            saveable.GetSaveData(saveData);
        }
    }

    private void OnAfterSceneUnloadEvent()
    {
        throw new System.NotImplementedException();
    }*/

    public void RegisterSaveDate(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }
    public void UnRegisterSaveDate(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }
    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key +"   "+ item.Value);
            //saveable.GetSaveData(saveData);
        }
    }
    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }
}