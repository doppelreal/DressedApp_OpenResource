using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class XML_ManagerScene : MonoBehaviour 
{
	[Tooltip("TMP components in same order as called by this script = Title, Beschreibung")]
	public TMP_Text[] displayText;
    
	[Tooltip("Holder for background Images")]
	public GameObject bgHolder;
	
	[Tooltip("all of the characters to be instantiated")]
	public List<GameObject> imgPrefabs;
	
	private int _selectedCharacterInt;
	private string _selectedCharacterString;
	private string _selectedTitelString;

	private void Awake()
	{
		#if UNITY_EDITOR
            XML_ManagerMain.Load();
		#endif
	}

	void Start () 
	{
		FetchCharacter();
	}
	
	private void FetchCharacter()
    {
        //loads last character if there's one saved, otherwise sets character to 1st position in list
        _selectedCharacterInt = PlayerPrefs.GetInt("Resources.CharacterInt", 0);
        
        //method to actually populate scene with character specific content
        SetCharacter();
    }
    
    public void NextCharacter()
    {
	    _selectedCharacterInt++;

	    if (_selectedCharacterInt >= XML_ManagerMain.loadedList.Count)
        {
	        _selectedCharacterInt = 0;
        }
	    
        SetCharacter();
    }

    public void PreviousCharacter()
    {
	    _selectedCharacterInt--;

	    if (_selectedCharacterInt < 0)
        {
	        _selectedCharacterInt = XML_ManagerMain.loadedList.Count-1;
        }
	    
        SetCharacter();
    }

    void SetCharacter()
    {
	    //delete all previously instantiated prefabs that are children under the prefab holder GO
        for (int i=0; i< bgHolder.transform.childCount; i++)
        {
            var d = bgHolder.transform.GetChild(i);
            Destroy(d.gameObject);
        }
        
        //set up texts
        _selectedTitelString = XML_ManagerMain.loadedList[_selectedCharacterInt].entryTitel;
        var beschText = XML_ManagerMain.loadedList[_selectedCharacterInt].entryBeschr;
        
        displayText[0].text = _selectedTitelString;
        displayText[1].text = beschText;
        
        //save position of current character in list
        PlayerPrefs.SetInt("Resources.CharacterInt", _selectedCharacterInt);
        if(Application.isEditor) Debug.Log(_selectedCharacterInt);
        
        //save character image name
        _selectedCharacterString = XML_ManagerMain.loadedList[_selectedCharacterInt].entryID;
        PlayerPrefs.SetString("Resources.CharacterString", _selectedCharacterString);
        if(Application.isEditor) Debug.Log(_selectedCharacterString);

        //set up character prefab
        var setPrefab = FetchAssets<GameObject>(_selectedCharacterString);
        
        if (setPrefab != null) //checking if there actually is a prefab in the list
        {
            GameObject instance = Instantiate(setPrefab, bgHolder.transform);
            instance.transform.localPosition = Vector3.zero;
        }
        else
        {  if(Application.isEditor) Debug.Log(_selectedCharacterString + " not set"); }
    }
    
    T FetchAssets<T>(string assetName) where T : UnityEngine.Object
    { 
        if (typeof(T) == typeof(GameObject))
        {	
            foreach (var prefab in imgPrefabs)
            {
                if (prefab.name == assetName)
                {
                    return prefab as T;
                }
            }
        }
        if(Application.isEditor) Debug.Log("nothing with name " + assetName + " found");
        return null;
    }
}