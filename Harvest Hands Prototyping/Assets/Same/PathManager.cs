using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour
{
    public enum GoalPlace
    {
        NONE = -1,
        Market,
        Castle,
        Farm,
    }

    public static PathManager instance = null;

    public List<Transform> farmPoints;
    public List<Transform> marketPoints;
    public List<Transform> castlePoints;
    public List<Transform> marketFarmPath;
    public List<Transform> marketCastlePath;
    //[HideInInspector]
    public List<Transform> farmMarketPath;
    //[HideInInspector]
    public List<Transform> castleMarketPath;


    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        Debug.Log("MarketFarmPath.Count = " + marketFarmPath.Count);
        for (int i = marketFarmPath.Count; i > 0; --i)
        {
            farmMarketPath.Add(marketFarmPath[i-1]);
        }
        Debug.Log("FarmMarketPath.Count = " + farmMarketPath.Count);
        Debug.Log("MarketCastlePath.Count = " + marketCastlePath.Count);
        for (int i = marketCastlePath.Count; i > 0; --i)
        {
            castleMarketPath.Add(marketCastlePath[i-1]);
        }
        Debug.Log("CastleMarketPath.Count = " + castleMarketPath.Count);

        //farmMarketPath = marketFarmPath;
        //farmMarketPath.Reverse();
        //castleMarketPath = marketCastlePath;
        //castleMarketPath.Reverse();
    }

    public List<Transform> GetNewPath(ref GoalPlace currentPlace)
    {
        List<Transform> newPath = new List<Transform>();
        GoalPlace newGoal = GoalPlace.NONE;
        newGoal = (GoalPlace)Random.Range(0, 3); ;
        //int stanewGoalte = Random.Range(0, 3);
        ////Choose new area to go to
        //switch (0)
        //{
        //    case 0:
        //        {
        //            newGoal = GoalPlace.Market;
        //        }
        //        break;
        //    case 1:
        //        {
        //            newGoal = GoalPlace.Castle;
        //        }
        //        break;
        //    case 2:
        //        {
        //            newGoal = GoalPlace.Farm;
        //        }
        //        break;
        //}

        //if go to current area
        if (currentPlace == newGoal)
        {
            newPath.Add(GetTransformFromArea(newGoal));
        }
        else
        {
            newPath = GetPathBetweenAreas(currentPlace, newGoal);
            newPath.Add(GetTransformFromArea(newGoal));            
        }

        currentPlace = newGoal;    
        return newPath;

    }

    public Transform GetTransformFromArea(GoalPlace area)
    {
        if (area == GoalPlace.Castle)
        {
            int i = Random.Range(0, castlePoints.Count);
            return castlePoints[i];
        }
        else if (area == GoalPlace.Market)
        {
            int i = Random.Range(0, marketPoints.Count);
            return marketPoints[i];
        }
        else if (area == GoalPlace.Farm)
        {
            int i = Random.Range(0, farmPoints.Count);
            return farmPoints[i];
        }
        else
            return marketPoints[0];
    }

    public List<Transform> GetPathBetweenAreas(GoalPlace startingArea, GoalPlace goalArea)
    {
        List<Transform> newPath = new List<Transform>();
        if (startingArea == GoalPlace.Farm)
        {
            if (goalArea == GoalPlace.Market)
            {
                for (int i = 0; i < farmMarketPath.Count; ++i)
                {
                    newPath.Add(farmMarketPath[i]);
                }
                return newPath;
            }
            else if (goalArea == GoalPlace.Castle)
            {
                for (int i = 0; i < farmMarketPath.Count; ++i)
                {
                    newPath.Add(farmMarketPath[i]);
                }
                for (int i = 0; i < marketCastlePath.Count; ++i)
                {
                    newPath.Add(marketCastlePath[i]);
                }
                return newPath;
            }
            else
                return new List<Transform>();
        }
        else if (startingArea == GoalPlace.Market)
        {
            if (goalArea == GoalPlace.Farm)
            {
                for (int i = 0; i < marketFarmPath.Count; ++i)
                {
                    newPath.Add(marketFarmPath[i]);
                }
                return newPath;
            }
            else if (goalArea == GoalPlace.Castle)
            {
                for (int i = 0; i < marketCastlePath.Count; ++i)
                {
                    newPath.Add(marketCastlePath[i]);
                }
                return newPath;
            }
            else
                return new List<Transform>();
        }
        else if (startingArea == GoalPlace.Castle)
        {
            if (goalArea == GoalPlace.Market)
            {
                for (int i = 0; i < castleMarketPath.Count; ++i)
                {
                    newPath.Add(castleMarketPath[i]);
                }
                return newPath;
            }
            else if (goalArea == GoalPlace.Farm)
            {
                for (int i = 0; i < castleMarketPath.Count; ++i)
                {
                    newPath.Add(castleMarketPath[i]);
                }
                for (int i = 0; i < marketFarmPath.Count; ++i)
                {
                    newPath.Add(marketFarmPath[i]);
                }
                return newPath;
            }
            else
                return new List<Transform>();
        }
        else
            return new List<Transform>();
    }

}
