using System;
using System.Collections.Generic;

[Serializable]
public class PlayerInfo
{
    public string playerName;
    public int daysPlayed;
}

[Serializable]
public class PlayerInfoList
{
    public List<PlayerInfo> list;
}