using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlantProduce : NetworkBehaviour
{
    public string produceName = "Produce Name";
    public int ProduceAmount = 1;
    public int score = 10;

    void Start()
    {
        if (GetComponent<Mushroom>() == null)
            SaveAndLoad.SaveEvent += SaveFunction;
    }

    void OnDestroy()
    {
        if (GetComponent<Mushroom>() == null)
            SaveAndLoad.SaveEvent -= SaveFunction;
    }

    public void SaveFunction(object sender, string args)
    {
        SavedProduce produce = new SavedProduce();
        produce.PosX = transform.position.x;
        produce.PosY = transform.position.y;
        produce.PosZ = transform.position.z;
        produce.produceName = produceName;
        produce.produceAmount = ProduceAmount;
        produce.scoure = score;

        SaveAndLoad.localData.savedProduce.Add(produce);
    }

    [Command]
    public void CmdAddSaveEvent()
    {
        SaveAndLoad.SaveEvent += SaveFunction;
    }

    
}
