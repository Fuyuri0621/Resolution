using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader
{
    public static List<MissionTableItem> LoadMissions(string filePath)
    {
        List<MissionTableItem> missionList = new List<MissionTableItem>();

        if (!File.Exists(filePath))
        {
            Debug.LogError("CSV �ɮפ��s�b: " + filePath);
            return missionList;
        }

        string[] lines = File.ReadAllLines(filePath); 

        for (int i = 1; i < lines.Length; i++) // ���L���D��
        {
            string[] values = lines[i].Split(','); // �H�r�����j

            if (values.Length < 9) continue; // �T�O���

            MissionTableItem mission = new MissionTableItem
            {
                id = int.Parse(values[0]),
                type = (MissionType)int.Parse(values[1]),
                getItemID = int.Parse(values[2]),
                Interactprompt = values[3].Trim('"'),
                MissionName = values[4].Trim('"'),
                shortDescription = values[5].Trim('"'),
                description = values[6].Trim('"'),
                AccomplishDialogue = values[7].Trim('"'),
                DialogueSpeaker = values[8].Trim('"'),
                Waypointsid = ParseWaypoints(values.Length > 9 ? values[9] : "") 
            };

            missionList.Add(mission);
        }

        return missionList;
    }

    private static List<int> ParseWaypoints(string waypointsString)
    {
        List<int> waypoints = new List<int>();
        if (!string.IsNullOrEmpty(waypointsString))
        {
            string[] parts = waypointsString.Split(',');
            foreach (string part in parts)
            {
                if (int.TryParse(part.Trim(), out int waypoint))
                {
                    waypoints.Add(waypoint);
                }
            }
        }
        return waypoints;
    }
}

