using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    private AsyncOperation _async;
    
    public void OnClickLoadLevel (int sceneIndex)
    {
        StartCoroutine(LoadRoutine(sceneIndex));
    }

    IEnumerator LoadRoutine(int sceneInd)
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        _async = SceneManager.LoadSceneAsync(sceneInd, LoadSceneMode.Single);
        _async.allowSceneActivation = false;
        
        
        while (!_async.isDone) //as long as async scene activation isn't done, count % of progress
        {
            float loadProgress = _async.progress;

            if (loadProgress >= 0.9f) //stop while loop when progress at 90%
            {
                Debug.Log("90%");
                break;
            }
            yield return null;
        }
        
        StartCoroutine(CompleteLoading());
    }
    
    private IEnumerator CompleteLoading()
    {
        yield return new WaitForEndOfFrame();
        _async.allowSceneActivation = true;
        yield return _async;
    }
}