using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Reflection;
using NeoModLoader.api;
using NeoModLoader.services;using NCMS;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ReflectionUtility;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AristocracyAndPower.UI
{
    class PopulationWindow : MonoBehaviour
    {
        public static GameObject contents;
        private static GameObject scrollView;

        public static ScrollRect scrollRect;
        private static Vector2 originalSize;
        public static PopulationWindow instance;

        public static Kingdom currentKingdom;
        public static GameObject smallButton;
        public static Text contentText;
        public static Dictionary<string, string> CustomTextColors = new Dictionary<string, string>();
        public static Dictionary<string, Text> CustomTexts = new Dictionary<string, Text>();
        public static Dictionary<string, double> CustomNums = new Dictionary<string, double>();
        public static List<string> CustomTextIds = new List<string>();
        public static Dictionary<Culture, double> culture = new Dictionary<Culture, double>();
        public static Dictionary<string, double> races = new Dictionary<string, double>();

        public static void init()
        {
            contents = WindowManager.windowContents["PopulationWindow"];

            scrollView = GameObject.Find(
                "Canvas Container Main/Canvas - Windows/windows/PopulationWindow/Background/Scroll View"
            );
            GameObject Background = GameObject.Find(
                "Canvas Container Main/Canvas - Windows/windows/PopulationWindow/Background"
            );
            // Image image1 = Background.GetComponent<Image>();
            // image1.sprite = Mod.EmbededResources.LoadSprite(
            //     $"{Mod.Info.Name}.Resources.UI.政府总览窗口.png"
            // );
            // 获取ScrollRect组件
            scrollRect = scrollView.GetComponentInChildren<ScrollRect>();
            scrollView.GetComponent<RectTransform>().sizeDelta -= new Vector2(50, 0);
            scrollView.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 10);
            instance = new GameObject("PopulationWindow").AddComponent<PopulationWindow>();

            ContentSizeFitter contentSizeFitter = contents.AddComponent<ContentSizeFitter>();

            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            contents.GetComponent<RectTransform>().anchoredPosition += new Vector2(2f, 0f);

            originalSize = contents.GetComponent<RectTransform>().sizeDelta;

            contents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100);

            VerticalLayoutGroup layoutGroup = contents.AddComponent<VerticalLayoutGroup>();
            layoutGroup.childControlHeight = true;
            layoutGroup.childForceExpandHeight = false;


            int x = 30;
            int y=-10;  
            NewText("Population","#eca55f",contents.transform,new Vector3 (x,y));
            y-=10;

            NewText("Knoble","#450c79",contents.transform,new Vector3 (x,y));
            y-=10;
            // NewText("noble","#450c79",contents.transform,new Vector3 (x,y));
            // y-=10;
            NewText("armyNum","#F0FFFF",contents.transform,new Vector3 (x,y));
            y-=10;
            NewText("workerNum","#a7535a",contents.transform,new Vector3 (x,y));
            y-=10;
            NewText("farmerNum","#96c24e",contents.transform,new Vector3 (x,y));
            y-=20;
            NewText("Unemployed","#3170a7",contents.transform,new Vector3 (x,y));
            y-=10;
            NewText("culture","#FFD700",contents.transform,new Vector3 (x,y));
            y-=10;
            NewText("race","#eca55f",contents.transform,new Vector3 (x,y));
            y-=10;


            // NewUI.createBGWindowButton(
            //     GameObject.Find($"Canvas Container Main/Canvas - Windows/windows/PopulationWindow"),
            //     posY: -40,
            //     iconName: "iconPolicy",
            //     buttonName: "扇形",
            //     buttonTitle: "扇形",
            //     buttonDesc: "扇形",
            //     call: openOpenPieChart
            // );
        }


        
        // public static void openOpenPieChart()
        // {
        //     UIGrap=new UIGraphManager();
        //     UIGrap.transform.SetParent(contents.transform);
            
        // }
        public static void openWindow()
        {
            Windows.ShowWindow("PopulationWindow");

            Ui();
        }

        public static void Ui()
        {
            Kingdom kingdom = currentKingdom;
            if (kingdom == null)
            {
                Debug.LogWarning("No kingdom selected.");
                return;
            }

            UpdateDictionaryWithZero(CustomNums);

            culture = new Dictionary<Culture, double>();
            races = new Dictionary<string, double>();
            Clan kingclan=null;
            Clan leaderclan=null;
            if (kingdom.king != null && kingdom.king.data.clan != string.Empty)
            {
                kingclan = kingdom.king.getClan();
            }


            foreach (City city in kingdom.cities)
            {
                // 统计职业数量
                CustomNums["farmerNum"] += (city.jobs.countOccupied(CitizenJobLibrary.farmer)+city.jobs.countOccupied(CitizenJobLibrary.gatherer_bushes) + 
                                        city.jobs.countOccupied(CitizenJobLibrary.gatherer_herbs) +
                                        city.jobs.countOccupied(CitizenJobLibrary.hunter)+
                                        city.jobs.countOccupied(CitizenJobLibrary.woodcutter)
                                        );
                CustomNums["workerNum"] += (city.jobs.countOccupied(CitizenJobLibrary.miner) + city.jobs.countOccupied(CitizenJobLibrary.miner_deposit)+
                                        city.jobs.countOccupied(CitizenJobLibrary.road_builder)+city.jobs.countOccupied(CitizenJobLibrary.builder));
                CustomNums["armyNum"] += city.getArmy();


                if (city.leader != null && city.leader.data.clan != string.Empty&& city.leader.getClan() != kingclan)
                {
                    leaderclan = city.leader.getClan();
                }
                // 统计文化数量
                foreach (Actor unit in city.units.getSimpleList())
                {
                    if(unit.asset.unit)
                    {
                        if(unit.citizen_job==null&&unit.data.profession != UnitProfession.Warrior){CustomNums["Unemployed"]++;}
                        CustomNums["Population"]++;
                        string unitRace = unit.asset.race;
                        races[unitRace] = races.ContainsKey(unitRace) ? races[unitRace] + 1 : 1;

                        Culture unitCulture = unit.getCulture();
                        if (unitCulture != null)
                        {
                            culture[unitCulture] = culture.ContainsKey(unitCulture) ? culture[unitCulture] + 1 : 1;
                        }
                        if(kingclan!=null&&unit.data.clan != string.Empty&&unit.getClan()==kingclan){CustomNums["Knoble"]++;}
                        // if(leaderclan!=null&&unit.data.clan != string.Empty&&unit.getClan()!=kingclan){CustomNums["noble"]++;}
                    }
                }
            }




            //从大到小排序
            culture=SortDictionary(culture);
            races=SortDictionary(races);

            StringBuilder str = new StringBuilder();
            foreach (string pid in CustomTextIds)
            {
                if (CustomTexts.TryGetValue(pid, out var customText) && customText != null && CustomTextColors.TryGetValue(pid, out var textColor))
                {
                    double proportion = Math.Round(CustomNums[pid] * 100 / CustomNums["Population"], 2);

                    if (pid == "culture")
                    {
                        UpdateCultureText(pid, str);
                    }
                    else if (pid == "race")
                    {
                        UpdateRaceText(pid, str);
                    }
                    else if (pid == "Population")
                    {
                        customText.text = $"<color={textColor}><b>{LocalizedTextManager.getText("tree_text_" + pid, null)}: {CustomNums[pid]}</b></color>";
                    }
                    else
                    {
                        customText.text = $"<color={textColor}><b>{LocalizedTextManager.getText("tree_text_" + pid, null)}: {CustomNums[pid]} ({proportion}%)</b></color>";
                    }
                }
            }
        }

        // 提取pid为culture的处理逻辑
        public static void UpdateCultureText(string pid, StringBuilder str)
        {
            if(culture.Keys.Count==0){return;}
            str.Clear();
            string t="";
            str.AppendLine("\n"+LocalizedTextManager.getText("tree_text_" + pid, null) + "\n");
            foreach (var cultureData in culture)
            {
                var proportion = Math.Round(cultureData.Value * 100 / CustomNums["Population"], 2);
                t=$"{cultureData.Key.name} {cultureData.Value}";
                str.AppendLine($"{Colored(t,cultureData.Key.getColor().getColorText())} ({proportion}%)");
            }
            CustomTexts[pid].text = str.ToString();
        }

        // 提取pid为race的处理逻辑
        public static void UpdateRaceText(string pid, StringBuilder str)
        {
            str.Clear();
            str.AppendLine(LocalizedTextManager.getText("tree_text_" + pid, null));
            foreach (var raceData in races)
            {
                var proportion = Math.Round(raceData.Value * 100 / CustomNums["Population"], 2);
                str.AppendLine($"{raceData.Key} {raceData.Value} ({proportion}%)");
            }
            CustomTexts[pid].text = str.ToString();
        }




        public static Button NewLaw(string id, string icon, Vector3 xy, UnityAction call, GameObject CT)
        {
        var OtherTabButton = NCMS.Utils.GameObjects.FindEvenInactive("Button_Other");
        if (OtherTabButton == null){return null;}
        var BVC = xy;
        var NButton = GameObject.Instantiate(OtherTabButton);
        NButton.transform.SetParent(CT.transform);
        NButton.transform.localPosition = BVC;
        NButton.GetComponent<Image>().sprite = NCMS.Utils.Sprites.LoadSprite($"{Mod.Info.Path}/GameResources/ui/nothing.png");
        NButton.name = id;
        NButton.transform.Find("Icon").GetComponent<Image>().sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.Icons.{icon}.png");
        var NewButton = NButton.GetComponent<Button>();
        NewButton.onClick = new Button.ButtonClickedEvent();
        NewButton.onClick.AddListener(call);
        TipButton ButtonTip = NewButton.gameObject.GetComponent<TipButton>();
        ButtonTip.textOnClick = id;
        ButtonTip.textOnClickDescription = id;
        ButtonTip.text_description_2 = id + " Description";
        NewButton.gameObject.SetActive(true);
        return NewButton;
        }


    
        public static Text NewText(string id, string color,Transform parentr, Vector3 position)
        {
        GameObject MRef = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/PopulationWindow/Background/Name");
        GameObject MObj = GameObject.Instantiate(MRef, parentr);
        MObj.SetActive(true);
        Text MText = MObj.GetComponent<Text>();
        string text = LocalizedTextManager.getText("tree_text_" + id, null);
        MText.text = $"<color={color}><b>{text}</b></color>";
        MText.supportRichText = true;
        MText.transform.SetParent(parentr);
        var MObjRTF = MObj.GetComponent<RectTransform>();
        MObjRTF.position = new Vector3(0f, 0f, 0f);
        MObjRTF.localPosition = position;
        CustomTextColors.Add(id, color);
        CustomTexts.Add(id, MText);
        CustomNums.Add(id, 0);
        CustomTextIds.Add(id);
        return MText;
        }
        public static Dictionary<TKey, TValue> SortDictionary<TKey, TValue>(Dictionary<TKey, TValue> input)
        {
            var sortedDict = input.OrderByDescending(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            return sortedDict;
        }
        

        public static string Colored(string text, Color color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + $"><b>{text}</b></color>";
        }
        public static Dictionary<TKey, TValue> UpdateDictionaryWithZero<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            foreach (var key in dictionary.Keys.ToList())
            {
                dictionary[key] = (TValue)Convert.ChangeType(0, typeof(TValue));
            }
            return dictionary;
        }
    }
}