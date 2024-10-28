using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, ISaveable
{
    public Transform playerTrans;
    public Transform slimeTrans;
    public Vector3 firstPosition;
    public Vector3 menuPosition;

    [Header("�ƥ��ť")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    public VoidEventSO sceneSaveEvent;

    [Header("�T��")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unloadedSceneEvent;

    [Header("����")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    private GameSceneSO sceneToLoad;
    private GameSceneSO currentLoadedScene;
    private Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;

    public float fadeDuration;
    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference,LoadSceneMode.Additive);
        //currentLoadedScene = firstLoadScene;
        //currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }

    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuScene, menuPosition, true);
        //NewGame();
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
    }

    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if (isLoading)
            return;
        isLoading = true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadedScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if(fadeScreen)
        {
            fadeEvent.FadeIn(fadeDuration);
        }
        sceneSaveEvent.RaiseEvent();
        //Bailey.GetSaveData(Data data);
        //EventHandler.CallBeforeSceneUnloadEvent();
        yield return new WaitForSeconds(fadeDuration);

        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

        yield return currentLoadedScene.sceneReference.UnLoadScene();
        //EventHandler.CallAfterSceneUnloadEvent();
        //off player
        playerTrans.gameObject.SetActive(false);
        slimeTrans.gameObject.SetActive(false);
        //Load new map
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadedScene = sceneToLoad;

        playerTrans.position = positionToGo;
        playerTrans.gameObject.SetActive(true);
        slimeTrans.position = positionToGo;
        slimeTrans.gameObject.SetActive(true);
        if (fadeScreen)
        {
            fadeEvent.FadeOut(fadeDuration);
        }
        isLoading = false;

        if(currentLoadedScene.sceneType != SceneType.Menu)
        //�����[�������ƥ�
        afterSceneLoadedEvent.RaiseEvent();
    }

    public DataDefinition GatDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadedScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
        var slimeID = slimeTrans.GetComponent<DataDefinition>().ID;
        if (data.characterPosDict.ContainsKey(playerID)|| data.characterPosDict.ContainsKey(slimeID))
        {
            positionToGo = data.characterPosDict[playerID];
            positionToGo = data.characterPosDict[slimeID];
            sceneToLoad = data.GetGameScene();

            OnLoadRequestEvent(sceneToLoad, positionToGo, true);
        }
    }
}
