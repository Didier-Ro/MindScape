using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IsaveScript 
{
    public void LoadData(string s);

    public string SaveData();

    public void ResetData();
}
