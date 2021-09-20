using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    public GameObject cube;
    public GameObject sphere;
    public GameObject capsule;

    private void Start()
    {
        Log.logError("beginLoad", Time.frameCount);
        string pathCube = "Assets/GameAssets/Test/Cube.prefab";
        string pathSphere = "Assets/GameAssets/Test/Sphere.prefab";
        string pathCapsule = "Assets/GameAssets/Test/Capsule.prefab";
        Addressables.InstantiateAsync(pathCube).Completed += AddressableManager_Completed;
        Addressables.InstantiateAsync(pathSphere).Completed += AddressableManager_Completed1;
        Addressables.InstantiateAsync(pathCapsule).Completed += AddressableManager_Completed2;
    }

    [ContextMenu("测试清除1")]
    void TestClear()
    {
        DestroyImmediate(cube);
    }

    [ContextMenu("测试清除2")]
    void TestClear2()
    {
        Addressables.ReleaseInstance(sphere);
    }

    [ContextMenu("测试清除3")]
    void TestClear3()
    {
        Addressables.ReleaseInstance(cube);
        Addressables.ReleaseInstance(sphere);
        Addressables.ReleaseInstance(capsule);
    }

    private void AddressableManager_Completed2(AsyncOperationHandle<GameObject> obj)
    {
        capsule = obj.Result;
        Log.logError("instantiate capsule completed", Time.frameCount);
    }

    private void AddressableManager_Completed1(AsyncOperationHandle<GameObject> obj)
    {
        sphere = obj.Result;
        Log.logError("instantiate sphere completed", Time.frameCount);
    }

    private void AddressableManager_Completed(AsyncOperationHandle<GameObject> obj)
    {
        cube = obj.Result;
        Log.logError("instantiate cube completed", Time.frameCount);
    }
}
