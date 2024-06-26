using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldConditions", menuName = "ScriptableObjects/WorldConditions", order = 2)]
public class WorldCondition : ScriptableObject, IsaveScript
{
    [SerializeField] Condition[] conditions;
    public int nGame;
    public int timePlayed;
    public Vector3 lastPosition;
    public float flashLightEnergy;

    public int GetTimePlayed()
    {
        return timePlayed;
    }
    
    public void MarkCondition(int id, bool done = true)
    {
        conditions[id].isCompleted = done;
    }

    public string RestartDataToANewGame()
    {
        string dataToSave = "";
        for (int i = 0; i <conditions.Length; i++)
        {
            dataToSave += 0 + "/";
        }
        dataToSave += 0 + "/";
        dataToSave += 0 + "/";
        dataToSave += 0 + "/";
        dataToSave += 100 + "/";
        dataToSave += nGame.ToString();
        return dataToSave;
    }

    public int GetPercentageOfGameCompleted()
    {
        float percentageCompleted = 0;
        for (int i = 1; i < conditions.Length; i++)
        {
            if (conditions[i].isCompleted)
            {
                percentageCompleted++;
            }
        }
        percentageCompleted /= conditions.Length;
        percentageCompleted *= 100;
        return (int)percentageCompleted;
    }

    public string GetData(int _gameNumber)
    {
        string dataToSave = "";
        for (int i = 0; i < conditions.Length; i++)
        {
            dataToSave += (conditions[i].isCompleted ? 1 : 0) + "/";
        }
        dataToSave += lastPosition.x+ "/";
        dataToSave += lastPosition.y+ "/";
        dataToSave += timePlayed + "/";
        dataToSave += flashLightEnergy + "/";
        dataToSave += _gameNumber.ToString();
        Debug.Log(dataToSave);
        return dataToSave;
    }

    public string GetConditions()
    {
        string dataToSave = "";
        for (int i = 0; i < conditions.Length; i++)
        {
            dataToSave += (conditions[i].isCompleted ? 1 : 0) + "/";
        }
        return dataToSave;
    }
    public void LoadData(string s)
    {
        string[] conditionsS = s.Split("/");
        for (int i = 0 ; i < conditions.Length; i++)
        {
            conditions[i].isCompleted = int.Parse(conditionsS[i]) >= 1;
        }
        lastPosition.x = float.Parse(conditionsS[conditionsS.Length - 5]);
        lastPosition.y = float.Parse(conditionsS[conditionsS.Length - 4]);
        timePlayed = int.Parse(conditionsS[conditionsS.Length - 3]);
        flashLightEnergy = float.Parse(conditionsS[conditionsS.Length - 2]);
        nGame = int.Parse(conditionsS[conditionsS.Length-1]);
    }

    public void AddSecondsToTheTimePlayed(int framesPlayed)
    {
        timePlayed += (int)framesPlayed/60;
    }

    public void SavePlayerPosition(Vector3 position)
    {
        lastPosition = position;
    }

    public void SaveFlashlightEnergy(float energy)
    {
        flashLightEnergy = energy;
    }

    public bool IsFirstTimePlayed()
    {
        bool isPlayed;
        foreach (var check in conditions)
        {
            isPlayed = check.isCompleted;
            if (isPlayed)
            {
                return false;
            }
        }
        return true;
    }

    public WorldCondition GetNumOfGame()
    {
        return this;
    }
    public bool IsConditionCompleted(int id)
    {
        return conditions[id].isCompleted;
    }

    public string SaveData()
    {
        string dataToSave = "";
        for (int i = 0; i < conditions.Length; i++)
        {
            dataToSave += (conditions[i].isCompleted ? 1 : 0) + "/";
        }
        dataToSave += lastPosition.x + "/";
        dataToSave += lastPosition.y + "/";
        dataToSave += timePlayed + "/" ;
        dataToSave += flashLightEnergy + "/";
        dataToSave += nGame.ToString();
        PlayerPrefs.SetString("worldConditions", dataToSave);
        return dataToSave;
    }

    public void ResetData()
    {
        for (int i = 0; i <conditions.Length; i++)
        {
            conditions[i].isCompleted = false;
        }
        lastPosition = Vector3.zero;
        timePlayed = 0;
        flashLightEnergy = 100f;
    }
}
[Serializable]
public struct Condition
{
    public string condition;
    public bool isCompleted;
}
