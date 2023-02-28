using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class XML_ARManager : MonoBehaviour
{
    [SerializeField] ARFaceManager sceneFaceManager;
    public List<GameObject> arFilterList;

    [SerializeField] TMP_Text titleTMP;

    private string _selectedFilterName; //actual string value under key
    private string _selectedTitelString;

    private GameObject _selectedPrefab;
    private AsyncOperation _async;

    void Awake() 
    {
        #if UNITY_EDITOR
            XML_ManagerMain.Load();
        #endif
        
        if (PlayerPrefs.HasKey("Resources.CharacterString")) 
        {
            _selectedFilterName = PlayerPrefs.GetString("Resources.CharacterString");
            
            foreach (var entry in XML_ManagerMain.loadedList)
            {
                for (int i = 0; i <= XML_ManagerMain.loadedList.Count; i++)
                {
                    if (entry.entryID == _selectedFilterName)
                    {
                        _selectedTitelString = entry.entryTitel;
                    }
                }
            }
        }
        
        
        else //give it a default string
        {
            _selectedFilterName =  XML_ManagerMain.loadedList[0].entryID;
            PlayerPrefs.SetString("Resources.CharacterString", _selectedFilterName);
            
            _selectedTitelString =  XML_ManagerMain.loadedList[0].entryTitel;
        }
    }

    void Start()
    {
        _selectedPrefab = FetchAssets<GameObject>(_selectedFilterName);

        if (_selectedPrefab != null)
        {
            sceneFaceManager.facePrefab = _selectedPrefab;
            titleTMP.text = _selectedTitelString;
        }
        
        else
        {
            _selectedPrefab = arFilterList[0];
            sceneFaceManager.facePrefab = _selectedPrefab;
            Debug.Log("_selectedPrefab wasn't set, used " + _selectedPrefab + " instead");
        }
    }
    
    public void OnClickReload(string prefabName) //called from each button to swap
    {
        StartCoroutine(LoadRoutine());
       
        PlayerPrefs.SetString("Resources.CharacterString", prefabName);
        
        StartCoroutine(Delay());
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine (CompleteLoading());
    }
    
    IEnumerator LoadRoutine()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        _async = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        _async.allowSceneActivation = false;
        
        while (!_async.isDone) //as long as async scene activation isn't done, count % of progress
        {
            float loadProgress = _async.progress;

            if (loadProgress >= 0.9f) //stop while loop when progress at 90%
            {
                break;
            }
            yield return null;
        }
    }
    
    private IEnumerator CompleteLoading()
    {
        _async.allowSceneActivation = true;
        yield return _async;
    }

    T FetchAssets<T>(string assetName) where T : UnityEngine.Object
    { 
        if (typeof(T) == typeof(GameObject))
        {	
            foreach (var var in arFilterList)
            {
                if (var.name == assetName)
                {
                    return var as T;
                }
            }
        }
        Debug.Log("FetchAssets didn't find anything named " + assetName);
        return null;
    }
}