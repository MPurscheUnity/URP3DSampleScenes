using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadAddressableScene : MonoBehaviour
{
    public AssetReference SceneToLoad;
    private AsyncOperationHandle<SceneInstance> loadHandle;

    IEnumerator Start()
    {
        loadHandle = Addressables.LoadSceneAsync(SceneToLoad, LoadSceneMode.Single);
        yield return loadHandle;
    }

    void OnDestroy()
    {
        Addressables.UnloadSceneAsync(loadHandle);
    }
}
