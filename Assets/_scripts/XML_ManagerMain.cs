using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public static class XML_ManagerMain 
{
    public static List<FilterEntries> loadedList 
    { 
        get { return filterDatabase.xmlList; } 
        set { filterDatabase.xmlList = value; } 
    }
    
    public static FilterDatabase filterDatabase;

    private static string xmlFileName = "FilterDatabase";
    private static string xmlFilePath = Application.dataPath + "/Resources/" + xmlFileName + ".xml";
    
    
    public static void Load() 
    {
        if (filterDatabase == null)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FilterDatabase));

            var xmlFile = Resources.Load<TextAsset>(xmlFileName);

            if (xmlFile)
            {
                filterDatabase = serializer.Deserialize(GenerateStreamFromString(xmlFile.text)) as FilterDatabase;
                Debug.Log("Import done " + xmlFileName);

            }
            else
            {
                Debug.LogError("Import failed " + xmlFileName + " doesn't exist");
            }
        }
        else
        {
            Debug.Log("Character XML file was already loaded");
        }
    }

    private static Stream GenerateStreamFromString(string s) 
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(s));
    }

    private static void Save() 
    {
        var serializer = new XmlSerializer(typeof(FilterDatabase));
        var encoding = Encoding.GetEncoding("UTF-8");

        using (StreamWriter stream = new StreamWriter(xmlFilePath, false, encoding)) 
        {
            serializer.Serialize(stream, filterDatabase);
        }

        Debug.Log("FILE SAVED. filePath -> " + xmlFilePath);
    }
}