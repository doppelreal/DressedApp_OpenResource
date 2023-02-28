using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

[System.Serializable] public class FilterDatabase
{
    [XmlElement("Character")]
    public List<FilterEntries> xmlList; //refers to class below
}

[System.Serializable] public class FilterEntries //was XML_Questions
{
    [XmlAttribute("ID")] public string entryID;
    [XmlAttribute("Img")] public string entryImgID;
    [XmlAttribute("Region")] public string entryRegion;
    [XmlAttribute("Kreis")] public string entryKreis;

    [XmlElement(ElementName = "Titel")] public string entryTitel;
    [XmlElement(ElementName = "Data")] public string entryData;
    [XmlElement(ElementName = "Beschreibung")] public string entryBeschr;
    [XmlElement(ElementName = "Kuenstler")] public string entryKunst;
    
    public FilterEntries() //was XML_Questions
    {
        entryID = "";
        entryImgID = "";
        entryRegion = "";
        entryKreis = "";
        entryTitel = "";
        entryData = "";
        entryBeschr = "";
        entryKunst = "";
    }
}