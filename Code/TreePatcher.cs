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
using ai;
using ai.behaviours;
using Beebyte.Obfuscator;
using AristocracyAndPower;
using AristocracyAndPower.UI;
using AristocracyAndPower.Utils;

namespace AristocracyAndPower
{
    class TreePatcher
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "newKillAction")]
        public static bool newKillAction_Prefix(Actor __instance,Actor pDeadUnit, Kingdom pPrevKingdom)
        {
            if(__instance.data.profession == UnitProfession.Warrior)
            {
                if (__instance.unit_group==null||__instance.unit_group.groupLeader==null||__instance.unit_group.units.Count == 0)
                {
                    return true;
                }
                int num=1;
                pDeadUnit.data.get("military exploit", out int num2,0);
                if(num2>1)
                {
                    num=num2/2;
                }
                if(pDeadUnit.isKing())
                {
                    num+=5;
                }
                if(pDeadUnit.isCityLeader())
                {
                    num+=2;
                }
                __instance.data.change("military exploit", num,-10000, 10000);
                Actor group_leader=__instance.unit_group.groupLeader;
                group_leader.data.change("military exploit", (num+1)/2,-10000, 10000);
                if (pDeadUnit.unit_group==null||pDeadUnit.unit_group.groupLeader==null||pDeadUnit.unit_group.units.Count == 0)
                {
                    return true;
                }
                Actor Dgroup_leader=pDeadUnit.unit_group.groupLeader;
                Dgroup_leader.data.change("military exploit", -(num+1)/2,-10000, 10000);
            }
            pDeadUnit.data.get("arrow", out int arrow, 0);
            if(arrow>0)
            {
                __instance.data.change("arrow",arrow,0,10000);
            }
            return true;
        }
        // [HarmonyPrefix]
        // [HarmonyPatch(typeof(ActorBase), "giveInventoryResourcesToCity")]
        // public static bool giveInventoryResourcesToCity(ActorBase __instance)
        // {
        //     if(__instance==null||__instance.inventory==null){return false;}
        //     if (__instance.inventory.hasResources() && __instance.city != null && __instance.city.isAlive()&&__instance.asset.unit)
        //     {
        //         foreach (ResourceContainer resourceContainer in __instance.inventory.getResources().Values)
        //         {
        //             if(resourceContainer.id=="gold"||resourceContainer.id=="arrow"){continue;}
        //             __instance.city.data.storage.change(resourceContainer.id, resourceContainer.amount);
        //         }
        //     }
        //     return true;
        // }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(WindowCreatureInfo), "OnEnable")]
        public static void OnEnable(WindowCreatureInfo __instance)
        {
            if (Config.selectedUnit == null)
            {
                return;
            }
            __instance.actor.data.get("military exploit", out int military, 0);
            if (military != 0)
            {
                Debug.Log("military exploit: "+military.ToString());
            }
            __instance.showStat("tree_text_military",military.ToString());
            return;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CityWindow), "OnEnable")]
        public static bool cityOnEnable(CityWindow __instance)
        {
            LegionWindow.currentCity = __instance.city;
            if(__instance == null||__instance.city == null || __instance.city.data == null){return true;}

            var resources=__instance.city.data.storage.resources;
            Debug.Log("City "+__instance.city.data.name+" __instance year's production report:");

            foreach(var res in resources)
            {
                __instance.city.data.get(res.Key+"produce", out int resnum, 0);
                resnum=res.Value.amount-resnum;
                Debug.Log(res.Key+": "+resnum.ToString());
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(KingdomWindow), "OnEnable")]
        public static bool KingdomOnEnable_Prefix(KingdomWindow __instance)
        {
            MunicipalWindow.currentKingdom = __instance.kingdom;
            PopulationWindow.currentKingdom = __instance.kingdom;
            EconomicWindow.currentKingdom = __instance.kingdom;
            policyWindow.currentKingdom = __instance.kingdom;
            NobleWindow.currentKingdom = __instance.kingdom;
            return true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BaseSimObject), "canAttackTarget")]
        public static void canAttackTarget(BaseSimObject pTarget, BaseSimObject __instance, ref bool __result)
        {  
            Actor actor = Reflection.GetField(__instance.GetType(), __instance, "a") as Actor;
            if (actor!=null&&actor.equipment != null && !actor.equipment.getSlot(EquipmentType.Weapon).isEmpty())
            {
                if(actor.getWeaponAsset().projectile=="arrow")
                {
                    actor.data.get("arrow", out int arrow, 0);
                    if(arrow<1){__result=false;return;}
                }
            }
            return;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "tryToAttack")]
        public static void tryToAttack(BaseSimObject pTarget, Actor __instance, ref bool __result, bool pDoChecks = true)
        {
            if(pTarget==null||__instance==null){__result=false;return;}
            if (pDoChecks)
            {
                if (__instance.s_attackType == WeaponType.Melee && pTarget.zPosition.y > 0f)
                {
                    __result=false;return;
                }
                if (__instance.isInLiquid() && !__instance.asset.oceanCreature)
                {
                    __result=false;return;
                }
                if (!__instance.isAttackReady())
                {
                    __result=false;return;
                }
                if (!__instance.isInAttackRange(pTarget))
                {
                    __result=false;return;
                }
            }
            if (__instance.equipment != null && !__instance.equipment.getSlot(EquipmentType.Weapon).isEmpty())
            {
                if(__instance.getWeaponAsset().projectile=="arrow")
                {
                    __instance.data.change("arrow",  -1);
                    __result=true;return;
                }
            }
            return;
        }

        // [HarmonyPrefix]
        // [HarmonyPatch(typeof(CityStorage), "change")]
        // public static bool change(CityStorage __instance,string pRes, int pAmount = 1)
        // {  
        //     if(__instance==null||__instance._city==null){return true;}
        //     if(pAmount>0)
        //     {
        //         __instance._city.data.change(pRes+"produce",(int)pAmount,-1000,10000);
        //     }
        //     return true;
        // }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "updateAge")]
        public static bool updateAge(City __instance)
        {  
            if(__instance==null||__instance.data==null||__instance.data.storage==null){return true;}
            
            var resources=__instance.data.storage.resources;
            foreach(var res in resources)
            {
                __instance.data.set(res.Key+"produce",  res.Value.amount);
                __instance.data.set(res.Key+"trade",  0);
            }
            __instance.data.set("Export volume", 0);
            __instance.data.set("Import volume",  0);
            return true;
        }
        // [HarmonyPrefix]
        // [HarmonyPatch(typeof(UnitGroup), "findGroupLeader")]
        // public static bool findGroupLeader(UnitGroup __instance)
        // {
        //     if(__instance==null){return false;}
        //     if (__instance.groupLeader != null)
        //     {
        //         if (__instance.groupLeader.kingdom.isCiv())
        //         {
        //             return false;
        //         }
        //         __instance.setGroupLeader(null);
        //     }
        //     if (__instance.units.Count == 0)
        //     {
        //         return false;
        //     }
        //     Legion Legion = __instance as Legion;

        //     if (Legion != null) // 检查Legion是否成功转换为Legion类型的对象
        //     {
        //         Debug.Log("test"); 
        //         Legion.FindHeir();
        //         if (Legion.hasHeir())
        //         {
        //             Actor heir = Legion.heir;
        //             if (Legion.groupLeader == null)
        //             {
        //                 Legion.setGroupLeader(heir);
        //                 Legion.clearHeirData();
        //                 return false;
        //             }
        //         }
        //     }
        //     return true;
        // }
        // [HarmonyPrefix]
        // [HarmonyPatch(typeof(UnitGroup), "setGroupLeader")]
        // public static bool setGroupLeader(UnitGroup __instance,Actor pActor)
        // {
        //     if (pActor == null && __instance.groupLeader != null)
        //     {
        //         // __instance.groupLeader.setGroupLeader(false);
        //         return true;
        //     }
        //     __instance.groupLeader = pActor;
        //     if (__instance.groupLeader != null)
        //     {
        //         Legion Legion = __instance as Legion;
        //         if (Legion != null) // 检查Legion是否成功转换为Legion类型的对象
        //         {
        //             Legion.clearHeirData();
        //             __instance.groupLeader.setGroupLeader(true);
        //         }
        //     }
        //     return true;
        // }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TooltipLibrary), "showResource")]
        public static void showResource(Tooltip pTooltip, string pType, TooltipData pData = default(TooltipData))
        {
            ResourceAsset resource = pData.resource;
            City selectedCity = pData.city ?? Config.selectedCity;
            pTooltip.name.text = LocalizedTextManager.getText(resource.id, null); 
            var resources=selectedCity.data.storage.resources;
            if (resource.id != SR.gold)
            {
                selectedCity.data.get(resource.id+"produce", out int resnum, 0);
                resnum=resources[resource.id].amount-resnum;
                if (resnum != 0)
                {
                    pTooltip.addItemText("tree_text_ResourceSurplus", resnum);
                }
                selectedCity.data.get(resource.id+"trade", out int trade, 0);
                if (trade != 0)
                {
                    pTooltip.addItemText("tree_text_trade", trade);
                }
                float num=(float)(TradeTools.getPrice(resources[resource.id],resnum,resources[resource.id].amount,selectedCity));
                if(num>0)
                {
                    pTooltip.addLineBreak("");
                    pTooltip.addLineBreak("---");
                    pTooltip.addItemText("tree_text_price",num , false, false, false, "#43FF43", false);
                }
                
            }
            else
            {
                selectedCity.data.get("Export volume", out int ExportVolume, 0);
                if (ExportVolume != 0)
                {
                    pTooltip.addItemText("tree_text_ExportVolume", ExportVolume);
                }
                selectedCity.data.get("Import volume", out int ImportVolume, 0);
                if (ImportVolume != 0)
                {
                    pTooltip.addItemText("tree_text_ImportVolume", ImportVolume);
                }
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CityBehProduceResources), "execute")]
        public static void execute(CityBehProduceResources __instance,City pCity,ref BehResult __result)
		{
            ResourceAsset resourceAsset = AssetManager.resources.get("tree_text_arrow");
            int num = pCity.data.storage.get("tree_text_arrow");
            if (num <= resourceAsset.maximum&&num <=pCity.getArmy()*3)
            {
                int pAmount = pCity.getArmy() / 5 + 1;
                if (num < resourceAsset.produce_min)
                {
                    pAmount = resourceAsset.produce_min;
                }
                __instance.tryToProduce(resourceAsset, pCity, pAmount);
            }
            ResourceAsset resourceAsset2 = AssetManager.resources.get("tree_text_tool");
            int num2 = pCity.data.storage.get("tree_text_tool");
            int pAmount2 = (int)(pCity.status.populationAdults/3);
            if (num2 < resourceAsset2.produce_min)
            {
                pAmount2 = resourceAsset2.produce_min;
            }
            if(num2<pCity.status.populationAdults)
            {
                __instance.tryToProduce(resourceAsset2, pCity, pAmount2);
            }
			__result= BehResult.Continue;
		}
        public static List<string> needTool=new List<string>(){S.builder,S.farmer,S.woodcutter,S.miner,S.miner_deposit,S.blacksmith,S.road_builder,S.cleaner};
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "checkCitizenJob")]
        public static void checkCitizenJob(CitizenJobAsset pJobAsset, City pCity, Actor pActor,ref bool __result)
        {
            if(needTool.Contains(pJobAsset.id))
            {
                pActor.data.get("tree_text_tool",out int num,0);
                if(pCity.data.storage.get("tree_text_tool")<=0&&num<=0)
                {
                    __result=false;
                }
                else if(pCity.data.storage.get("tree_text_tool")>=0||num>=0)
                {
                    __result=true;
                    if(num<=0)
                    {
                        pActor.data.change("tree_text_tool",1);
                        pCity.data.storage.change("tree_text_tool",-1);
                        return;
                    }
                }
            }

        }        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "killHimself")]
	    public static bool killHimself(ref Actor __instance)
	    {
            if (__instance != null)
            {
                if (__instance.city != null && __instance.city.isAlive())
                {
                    
                    __instance.data.get("tree_text_arrow",out int num,0);
                    __instance.city.data.storage.change("tree_text_arrow",num);
                }
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CityBehSupplyKingdomCities), "execute")]
        public static bool execute(CityBehSupplyKingdomCities __instance,City pCity,ref BehResult __result)
		{
			Culture culture = pCity.getCulture();
            Dictionary<string, List<tradeResource>> tradeResources = new Dictionary<string, List<tradeResource>>();
			if (culture == null)
			{
				__result=BehResult.Stop;
                return false;
			}
			if (pCity.kingdom.cities.Count == 1)
			{
				__result=BehResult.Stop;
                return false;
			}
			if (!culture.hasTech("trading"))
			{
				__result=BehResult.Stop;
                return false;
			}
			if (pCity.data.timer_supply > 0f)
			{
				__result=BehResult.Stop;
                return false;
			}
			__instance._resources.Clear();
			foreach (string key in pCity.data.storage.resources.Keys)
			{
				CityStorageSlot cityStorageSlot = pCity.data.storage.resources[key];
				if (cityStorageSlot.amount < cityStorageSlot.asset.supplyBoundTake)
				{
					__instance._resources.Add(cityStorageSlot);
				}
			}
			if (__instance._resources.Count == 0)
			{
				__result=BehResult.Stop;
                return false;
			}
			__instance._resources.Shuffle<CityStorageSlot>();
			foreach (City city in World.world.cities.list)
            {
                float num=0;
                if(city.kingdom!=pCity.kingdom){num=1;}
                if (city != pCity && !city.kingdom.isEnemy(pCity.kingdom))
                {
                    foreach (CityStorageSlot cityStorageSlot2 in __instance._resources)
                    {
                        if (city.data.storage.get(cityStorageSlot2.id) > cityStorageSlot2.asset.supplyBoundGive &&cityStorageSlot2.id != "gold")
                        {
                            var resources=city.data.storage.resources;
                            city.data.get(cityStorageSlot2.id+"produce", out int resnum, 0);
                            resnum=resources[cityStorageSlot2.id].amount-resnum;
                            num=(float)(TradeTools.getPrice(cityStorageSlot2,resnum,resources[cityStorageSlot2.id].amount,city)
                            +TradeTools.getTariff(city.kingdom,pCity.kingdom,cityStorageSlot2.asset.tradeCost));

                            tradeResource TradeResource=new tradeResource{city=city,cityStorageSlot=cityStorageSlot2,num=(float)num};
                            if (tradeResources.ContainsKey(cityStorageSlot2.id))
                            {
                                tradeResources[cityStorageSlot2.id].Add(TradeResource);
                            }
                            else
                            {
                                tradeResources.Add(cityStorageSlot2.id, new List<tradeResource>{TradeResource});
                            }
                        }
                    }
                }
            }

            foreach (string id in tradeResources.Keys)
            {
                tradeResources[id].Sort((x, y) => x.num > y.num ? -1 : 1);
                shareResource(tradeResources[id][0].city, pCity, tradeResources[id][0].cityStorageSlot,tradeResources[id][0].num);
                __instance.updateSupplyTimer(pCity);
            }
            
			__result=BehResult.Continue;
            return false;
		}
        public static void shareResource(City pCity, City pTargetCity, CityStorageSlot pSlot,float price)
		{
            if(pCity==null||pTargetCity==null||pSlot==null){return;}
            System.Random rand = new System.Random();
            int tradenum=rand.Next(pSlot.asset.supplyGive-2, pSlot.asset.supplyGive+5); 

            // if(pCity.kingdom!=pCity.ki){price+=1;}
            // pCity.data.storage.change(pSlot.id, -tradenum);
			// pTargetCity.data.storage.change(pSlot.id, tradenum);

            // var resources=pCity.data.storage.resources;
            // pCity.data.get(pSlot.id+"produce", out int resnum, 0);
            // resnum=resources[pSlot.id].amount-resnum;

            int num=(int)(tradenum*price);
            if(pTargetCity.data.storage.get("gold")<num*2){return;}

            pCity.data.storage.change("gold", num);
            pTargetCity.data.storage.change("gold", -num);

            pCity.data.change("Export volume",num,-10000,10000);
            pTargetCity.data.change("Import volume",-num,-10000,10000);

            pCity.data.set(pSlot.id+"trade",-tradenum);
            pTargetCity.data.set(pSlot.id+"trade",tradenum);

            pCity.gold_change+=num;
            pTargetCity.gold_change-=num;



            Debug.Log(pCity.data.name+"尝试给予"+pTargetCity.data.name+pSlot.id+tradenum.ToString()+$"({num.ToString()})");
            return;
		}
      
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "updateParallelChecks")]
        public static void updateParallelChecks(Actor __instance,float pElapsed)
        {
            //是否拉屎检查
            if(__instance!=null&&__instance.data!=null&&__instance.isAlive())
            {
                __instance.data.get("Satiety",out int Satiety, 0);
                __instance.data.get("digestion time",out float digestionTime, 0f);
                if(digestionTime>0f){__instance.data.set("digestion time",digestionTime-pElapsed);}
                else if(__instance.city!=null&&Satiety>=12)
                {
                    __instance.city.data.storage.change("tree_text_cack",1);
                    __instance.data.change("Satiety",-12);
                }
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "consumeCityFoodItem")]
        public static void consumeCityFoodItem(Actor __instance,ResourceAsset pAsset)
        {
            if(__instance!=null&&__instance.data!=null&&__instance.isAlive())
            {
                __instance.data.change("Satiety",3);
                __instance.data.get("Satiety",out int Satiety, 0);
                __instance.data.get("digestion time",out float digestionTime, 0f);
                if(Satiety>=12){ __instance.data.set("digestion time",digestionTime+5f);}
                else
                {
                    __instance.data.set("digestion time",digestionTime+3f);
                }
                // nutritions[pAsset].protein;
                // double protein,double fat,double vitamin,double calcium,double dietary_fiber
            }
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BehExtractResourcesFromBuilding), "getBonusResource")]
        public static void getBonusResource(Actor pActor, string pResourceID,ref int __result)
		{
            if(pActor==null||pResourceID==null||pActor.city==null){return;}
			if(pResourceID=="wheat"&&pActor.ai.job.id=="farmer"&&pActor.city.data.storage.get("tree_text_cack")>=2)
            {
                pActor.city.data.storage.change("tree_text_cack",-2);
                __result= 1;
                return;
            }
            __result= 0;

		}
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Wheat), "growWheat")]
        public static void growWheat(Wheat __instance)
        {
            __instance.timerGrow = 80f;
        }
    }
    public class tradeResource
    {
        public City city  { get; set; }
        public CityStorageSlot cityStorageSlot  { get; set; }
        public float num { get; set; }
    }

}
