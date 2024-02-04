using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using NeoModLoader.api;
using NeoModLoader.services;using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
using System.Reflection;
using Newtonsoft.Json;
using System;
using NCMS;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Threading.Tasks;
using static Config;
using System.Reflection.Emit;
using UnityEngine.Tilemaps;
using UnityEngine.Purchasing.MiniJSON;
using System.Collections;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using UnityEngine.CrashReportHandler;
using System.IO.Compression;
using System.Threading;
using System.Text;
using Beebyte.Obfuscator;
using ai;
using ai.behaviours;
using EpPathFinding.cs;
using life.taxi;
using SleekRender;
using tools;
using tools.debug;
using UnityEngine.EventSystems;
using WorldBoxConsole;
using UnityEngine.UI;
using AristocracyAndPower.UI;

namespace AristocracyAndPower
{
    [ModEntry]
    class Main : MonoBehaviour
    {

        private ModDeclare _declare;
        private GameObject _gameObject;
        public static KData data=new KData (){};
        
        public ModDeclare GetDeclaration()
        {
            return _declare;
        }
        public GameObject GetGameObject()
        {
            return _gameObject;
        }
        public string GetUrl()
        {
            return "http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=cG5sOwCqrKlrkbX0q7n-pVoX63j1LVQ9&authKey=OVseJaqeHdS8%2BZ742cTwS2ui%2B%2FOXcz9kgDmMoMFRkDl%2BGNB%2FApqB3D7cQkqJBLna&noverify=0&group_code=781471990";
        }
        public void OnLoad(ModDeclare pModDecl, GameObject pGameObject)
        {
            Dictionary<string, ScrollWindow> allWindows = (Dictionary<string, ScrollWindow>)Reflection.GetField(typeof(ScrollWindow), null, "allWindows");
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
            allWindows["inspect_unit"].gameObject.SetActive(false);
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "village");
            allWindows["village"].gameObject.SetActive(false);
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "kingdom");
            allWindows["kingdom"].gameObject.SetActive(false);
            
            moodFix();
            translation();
            ResourceFix.init();
            ActorFix.init();
            EraFix.init();
            WindowManager.init();
            Harmony.CreateAndPatchAll(typeof(Main));
            Harmony.CreateAndPatchAll(typeof(TreePatcher));
            LogService.LogInfo($"[{pModDecl.Name}]: load successfully!");
        }
        // public void Update()
        // {
        //     if (!Config.gameLoaded) return;
        // }
        //军团函数补丁
        // [HarmonyPostfix]
        // [HarmonyPatch(typeof(UnitGroupManager), "createNewGroup")]
        // public void createNewGroup(City pCity, UnitGroupManager __instance)
        // { 
        //     UnitGroup unitGroup=FindUnitGroupByCity(pCity);
        //     LegionGroup LegionGroup=new LegionGroup(unitGroup);
        //     LegionGroups.Add(pCity,LegionGroup);
        // }
        // public UnitGroup FindUnitGroupByCity(City city) 
        // {
        //     foreach(UnitGroup group in UnitGroupManager.groups) {
        //         if(group.city == city) {
        //             return group;
        //         }
        //     }
        //     return null;  //如果未找到符合条件的UnitGroup
        // }


        public void translation()
        {
            easyTranslate("en", "Population", "pop");
            easyTranslate("cz", "Population", "人口");
            easyTranslate("ch", "Population", "人口");
            easyTranslate("en", "PopulationDescription", "Displays the detailed number of people");
            easyTranslate("cz", "PopulationDescription", "显示人口结构，类似于阶级的东西");
            easyTranslate("ch", "PopulationDescription", "显示人口结构，类似于阶级的东西");
            easyTranslate("en", "noble", "Honorable");
            easyTranslate("cz", "noble", "官僚贵族");
            easyTranslate("ch", "noble", "官僚贵族");
            easyTranslate("en", "nobleDescription", "Show Honorable nobility");
            easyTranslate("cz", "nobleDescription", "官僚贵族");
            easyTranslate("ch", "nobleDescription", "官僚贵族");
            easyTranslate("en", "Money", "treasury");
            easyTranslate("cz", "Money", "经济");
            easyTranslate("ch", "Money", "经济");
            easyTranslate("en", "MoneyDescription", "Show treasury");
            easyTranslate("cz", "MoneyDescription", "显示经济");
            easyTranslate("ch", "MoneyDescription", "显示经济");
            easyTranslate("en", "Policy", "policy");
            easyTranslate("cz", "Policy", "政策");
            easyTranslate("ch", "Policy", "政策");
            easyTranslate("en", "PolicyDescription", "Show policy");
            easyTranslate("cz", "PolicyDescription", "显示政策");
            easyTranslate("ch", "PolicyDescription", "显示政策");

            easyTranslate("en", "tree_text_Population", "Population");
            easyTranslate("cz", "tree_text_Population", "总人口");
            easyTranslate("ch", "tree_text_Population", "总人口");
            easyTranslate("en", "tree_text_farmerNum", "farmer");
            easyTranslate("cz", "tree_text_farmerNum", "农林渔");
            easyTranslate("ch", "tree_text_farmerNum", "农林渔");
            easyTranslate("en", "tree_text_workerNum", "worker");
            easyTranslate("cz", "tree_text_workerNum", "工矿建筑");
            easyTranslate("ch", "tree_text_workerNum", "工矿建筑");
            easyTranslate("en", "tree_text_armyNum", "army");
            easyTranslate("cz", "tree_text_armyNum", "军人阶层");
            easyTranslate("ch", "tree_text_armyNum", "军人阶层");
            easyTranslate("en", "tree_text_Knoble", "royal");
            easyTranslate("cz", "tree_text_Knoble", "皇族");
            easyTranslate("ch", "tree_text_Knoble", "皇族");
            easyTranslate("en", "tree_text_noble", "noble");
            easyTranslate("cz", "tree_text_noble", "地方贵族");
            easyTranslate("ch", "tree_text_noble", "地方贵族");
            easyTranslate("en", "tree_text_Unemployed", "Unemployed");
            easyTranslate("cz", "tree_text_Unemployed", "不在工作岗位");
            easyTranslate("ch", "tree_text_Unemployed", "不在工作岗位");
            easyTranslate("en", "tree_text_culture", "culture");
            easyTranslate("cz", "tree_text_culture", "文化");
            easyTranslate("ch", "tree_text_culture", "文化");
            easyTranslate("en", "tree_text_race", "race");
            easyTranslate("cz", "tree_text_race", "种族");
            easyTranslate("ch", "tree_text_race", "种族");
            easyTranslate("en", "tree_text_military", "military");
            easyTranslate("cz", "tree_text_military", "军功");
            easyTranslate("ch", "tree_text_military", "军功");
            easyTranslate("en", "tree_text_Legion", "Legion");
            easyTranslate("cz", "tree_text_Legion", "军团");
            easyTranslate("ch", "tree_text_Legion", "军团");
            easyTranslate("en", "tree_text_ArmoredSoldier", "Armored Soldier");
            easyTranslate("cz", "tree_text_ArmoredSoldier", "着甲士兵");
            easyTranslate("ch", "tree_text_ArmoredSoldier", "着甲士兵");

            easyTranslate("en", "tree_text_ResourceSurplus", "Resource surplus");
            easyTranslate("cz", "tree_text_ResourceSurplus", "资源结余");
            easyTranslate("ch", "tree_text_ResourceSurplus", "资源结余");
            easyTranslate("en", "tree_text_trade", "trade");
            easyTranslate("cz", "tree_text_trade", "贸易");
            easyTranslate("ch", "tree_text_trade", "贸易");
            easyTranslate("en", "tree_text_price", "price");
            easyTranslate("cz", "tree_text_price", "价格");
            easyTranslate("ch", "tree_text_price", "价格");

            easyTranslate("en", "tree_text_ExportVolume", "Export volume");
            easyTranslate("cz", "tree_text_ExportVolume", "出口额");
            easyTranslate("ch", "tree_text_ExportVolume", "出口额");
            easyTranslate("en", "tree_text_ImportVolume", "Import Volume");
            easyTranslate("cz", "tree_text_ImportVolume", "进口额");
            easyTranslate("ch", "tree_text_ImportVolume", "进口额");

            easyTranslate("en", "tree_text_arrow", "arrow");
            easyTranslate("cz", "tree_text_arrow", "箭矢");
            easyTranslate("ch", "tree_text_arrow", "箭矢");
            easyTranslate("en", "tree_text_cack", "cack");
            easyTranslate("cz", "tree_text_cack", "平平无奇的一坨屎");
            easyTranslate("ch", "tree_text_cack", "平平无奇的一坨屎");
            easyTranslate("en", "tree_text_tool", "tool");
            easyTranslate("cz", "tree_text_tool", "工具");
            easyTranslate("ch", "tree_text_tool", "工具");
        }
        public void moodFix()
        {
            var a=AssetManager.moods.get("sad");
            a.base_stats[S.loyalty_mood] = -10f;
		    a.base_stats[S.opinion] = -10f;
            var b=AssetManager.moods.get("dark");
            b.base_stats[S.loyalty_mood] = -25f;
		    b.base_stats[S.opinion] = -25f;
            var c=AssetManager.moods.get("angry");
            c.base_stats[S.loyalty_mood] = -20f;
		    c.base_stats[S.opinion] = -20f;
        }
        public static Actor getUnitById(string id)
        {
            Actor actor = World.world.units.get(id);
            return actor;
        }
        
        public static void easyTranslate(string pLanguage, string id, string name)
        {
            string language = Reflection.GetField(LocalizedTextManager.instance.GetType(), LocalizedTextManager.instance, "language") as string;
            if (language != "en" && language != "ch" && language != "cz")
            {
                language = "en";
            }
            if (pLanguage != language)
            {
                return;
            }
            Localization.addLocalization(id, name);
        }
        
    }
}
