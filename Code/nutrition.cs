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
using NeoModLoader.services;using NCMS;
using NCMS.Utils;
using AristocracyAndPower;
using AristocracyAndPower.Utils;
using AristocracyAndPower.UI;
using ai;
using ai.behaviours;
using Beebyte.Obfuscator;


namespace AristocracyAndPower
{
	public class nutrition
    {
		
		public double protein;		//蛋白质：提高玩家的肌肉力量和生命值
		public double fat;    		//脂肪：减速度，体积增大
		public double vitamin;		//维生素：恢复血量
		public double calcium;		//钙：提高玩家的体积和抗打击能力
		public double dietary_fiber; //膳食纤维素：促进消化
	}
	
}

 