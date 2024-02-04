using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel;
using UnityEngine.Scripting;
using HarmonyLib;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using pathfinding;
using NeoModLoader.api;
using NeoModLoader.services;
using NCMS;
using NCMS.Utils;
using AristocracyAndPower;
using AristocracyAndPower.Utils;
using AristocracyAndPower.UI;
using ai;
using ai.behaviours;
using Beebyte.Obfuscator;


namespace AristocracyAndPower
{
	[Serializable]
	public class KData
    {
		public Dictionary<string, Legion> Legions = new Dictionary<string, Legion>();
        public Dictionary<string, KActor> units = new Dictionary<string, KActor>();
		
	}
	public class DataManager
	{
		public void init()
		{
			// 读取数据
            KData loadedData = LoadData("D:\\worldbox\\Mods\\Aristocracy&Power\\data.json");
            if (loadedData != null)
            {
                Debug.Log("load json successfully");
            }
		}
		public void SaveData(string filePath, KData data)
		{
			string jsonData = JsonUtility.ToJson(data);
			File.WriteAllText(filePath, jsonData);
		}

		public KData LoadData(string filePath)
		{
			if (File.Exists(filePath))
			{
				string jsonData = File.ReadAllText(filePath);
				KData data = JsonUtility.FromJson<KData>(jsonData);
				return data;
			}
			else
			{
				Debug.LogError("File not found at path: " + filePath);
				return null;
			}
		}
	}
	public class KActor
    {
		public nutrition nutrition;
		public float thirst_value;
		
	}
	
}

 