using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
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


namespace AristocracyAndPower.Utils
{
    public static class TradeTools
    {
		public static float getPrice(CityStorageSlot pSlot,int output,int amount,City city)
        {
			int total=0;
			var resources=city.data.storage.resources;
			foreach(CityStorageSlot Slot in resources.Values)
			{
				if(Slot.asset.type==ResType.Food)
				{
					total+=resources[Slot.id].amount;
				}
			}
			//原价*(2-(资源结余*0.03+资源数量*0.02))
			float num=(float)(pSlot.asset.tradeCost*(2-(output*0.03+amount*0.02)));
			int RaceHunger=AssetManager.actor_library.get("unit_"+city.race.id).maxHunger;
			// if(pSlot.asset.type==ResType.Food)
			// {
			// 	//原价*(恢复饥饿值/当地种族饥饿值/2*人口数/总食物数)
			// 	num=(float)(pSlot.asset.tradeCost*((pSlot.asset.restoreHunger/RaceHunger)*(city.getPopulationTotal(false)/total))*1000);
			// 	// num=1000;
			// 	Debug.Log((pSlot.asset.restoreHunger*100/RaceHunger).ToString()+" "+pSlot.asset.restoreHunger.ToString()+"/"+RaceHunger.ToString()+"  "+(city.getPopulationTotal(false)*2/total).ToString());
			// }
			if(num<1){num=1;}
            return num;
        }
		public static float getTariff(Kingdom pKingdom,Kingdom kingdom,int productValue)
        {
			int total=0;
			int total2=0;
			KingdomOpinion opinion = World.world.diplomacy.getRelation(kingdom, pKingdom).getOpinion(pKingdom, kingdom);
			foreach (OpinionAsset opinionAsset in opinion.results.Keys)
			{
				total += opinion.results[opinionAsset];
			}
			KingdomOpinion opinion2 = World.world.diplomacy.getRelation(pKingdom, kingdom).getOpinion(kingdom, pKingdom);
			foreach (OpinionAsset opinionAsset in opinion2.results.Keys)
			{
				total2 += opinion2.results[opinionAsset];
			}
			int relationDifference = Math.Abs(total - total2); // 计算关系差异的绝对值
			float baseTariff = (float)(relationDifference * 0.05 * productValue); // 根据关系差异和商品基础价值计算基础关税
			if (baseTariff > 100) // 基础关税上限为100
			{
				return 100;
			}
			return baseTariff;
		}
		
	}
}

 