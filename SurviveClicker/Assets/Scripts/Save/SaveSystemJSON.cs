using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveSystemJSON
{
    string savePath = Application.dataPath.Replace('/', '\\');
    
    public void SaveToJson(List<PlayerInfo> playerInfos)
    {
        PlayerInfoList playerInfoList = new PlayerInfoList();
        playerInfoList.list = playerInfos;
        
        string infoJson = JsonUtility.ToJson(playerInfoList, true);
        File.WriteAllText(Path.Combine(savePath, "PlayerData.json"), infoJson);
    }

    public List<PlayerInfo> LoadFromJson()
    {
        string infoJson = File.ReadAllText(Path.Combine(savePath, "PlayerData.json"));

        // When the save file is empty
        if (string.IsNullOrWhiteSpace(infoJson))
        {
            return new List<PlayerInfo>();
        }
        
        PlayerInfoList playerInfoList = JsonUtility.FromJson<PlayerInfoList>(infoJson);

        return playerInfoList.list;
    }
}
