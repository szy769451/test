using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string sceneToSave;
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();
    public Dictionary<string, bool> deletedObjects = new Dictionary<string, bool>();
    public void SaveGameScene(GameSceneSO gameScene)
    {
        sceneToSave = JsonUtility.ToJson(gameScene);
        Debug.Log(sceneToSave);
    }

    public GameSceneSO GetGameScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave,newScene);

        return newScene;
    }
}
