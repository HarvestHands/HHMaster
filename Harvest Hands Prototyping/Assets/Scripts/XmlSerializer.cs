using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/*
[XmlRoot("PlantCollection")]
public class PlantContainer : MonoBehaviour
{    
    [XmlArray("Plants"), XmlArrayItem("Plant")]
    public PlantData[] plant;
        
    public static PlantContainer Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlantContainer));
        //TextReader textReader = StreamReader(path);

        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as PlantContainer;
        }
    }
    
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(PlantContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
}

public class GameManagerData : MonoBehaviour
{
    [XmlAttribute("ingameDay")]
    public int ingameDay;
    [XmlAttribute("currentTimeOfDay")]
    public float currentTimeOfDay;
    [XmlAttribute("nightTimeCheckDone")]
    public bool nightTimeCheckDone;

    [XmlAttribute("score")]
    public int score;
    [XmlAttribute("oldScore")]
    public int oldScore;
}

public class SoilData : MonoBehaviour
{
    //if occupied load next plant data from xml list
    [XmlAttribute("occupied")]
    public bool occupied;
}

public class PlantData : MonoBehaviour
{
    [XmlAttribute("plantName")]
    public string plantName;
    [XmlAttribute("plantState")]
    public Plantscript.PlantState plantState;
    [XmlAttribute("readyToHarvest")]
    public bool readyToHarvest;
    [XmlAttribute("isWatered")]
    public bool isWatered;
    [XmlAttribute("isAlive")]
    public bool isAlive;
    [XmlAttribute("currentDryStreak")]
    public int currentDryStreak;
    [XmlAttribute("dryDays")]
    public int DryDays;
    [XmlAttribute("harvestToRemove")]
    public int harvestsToRemove;
    [XmlAttribute("daysSinceLastHarvest")]
    public int daysSinceLastHarvest;
    [XmlAttribute("dayPlanted")]
    public int dayPlanted;
}

public class FarmToolsData : MonoBehaviour
{
    
}

//public class MoveableObjectData : MonoBehaviour
//{
//
//}

public class ProduceData : MonoBehaviour
{
    [XmlAttribute("produceName")]
    public string produceName;
    [XmlAttribute("produceAmount")]
    public int produceAmount;
}

//bucket

    */