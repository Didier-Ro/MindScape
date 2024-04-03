using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldConditions", menuName = "ScriptableObjects/WorldConditions", order = 2)]
public class WorldCondition : ScriptableObject, IsaveScript
{
    [SerializeField] Condition[] conditions;
    
    
    public void MarkCondition(int id, bool done = true)
    {
        conditions[id].isCompleted = done;
    }


    public void LoadData(string s)
    {
        string[] conditionsS = s.Split("/");
        for (int i = 0 ; i < conditions.Length; i++)
        {
            conditions[i].isCompleted = int.Parse(conditionsS[i]) >= 1;
        }
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
        PlayerPrefs.SetString("worldConditions", dataToSave);
        Debug.Log(dataToSave);
        return dataToSave;
    }

    public void ResetData()
    {
        for (int i = 0; i <conditions.Length; i++)
        {
            conditions[i].isCompleted = false;
        }
    }
}
[Serializable]
public struct Condition
{
    public string condition;
    public bool isCompleted;
}
