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
    class LegionWindow : MonoBehaviour
    {
        public static GameObject contents;
        private static GameObject scrollView;

        public static ScrollRect scrollRect;
        private static Vector2 originalSize;
        public static LegionWindow instance;

        public static City currentCity;
        public static GameObject smallButton;
        public static Text contentText;
        public static Dictionary<string, string> CustomTextColors = new Dictionary<string, string>();
        public static Dictionary<string, Text> CustomTexts = new Dictionary<string, Text>();
        public static Dictionary<string, double> CustomNums = new Dictionary<string, double>();
        public static List<string> CustomTextIds = new List<string>();
        
        public static GameObject NameHolder;
        public static Image NameHolderImg;
        public static NameInput nameInput;

        public static void init()
        {
            contents = WindowManager.windowContents["LegionWindow"];

            scrollView = GameObject.Find(
                "Canvas Container Main/Canvas - Windows/windows/LegionWindow/Background/Scroll View"
            );
            GameObject Background = GameObject.Find(
                "Canvas Container Main/Canvas - Windows/windows/LegionWindow/Background"
            );
            Image image1 = Background.GetComponent<Image>();
            image1.sprite = Mod.EmbededResources.LoadSprite(
                $"{Mod.Info.Name}.Resources.UI.LegionWindow.png"
            );
            // 获取ScrollRect组件
            scrollRect = scrollView.GetComponentInChildren<ScrollRect>();
            scrollView.GetComponent<RectTransform>().sizeDelta -= new Vector2(50, 0);
            scrollView.GetComponent<RectTransform>().anchoredPosition -= new Vector2(-70, 45);
            instance = new GameObject("LegionWindow").AddComponent<LegionWindow>();

            ContentSizeFitter contentSizeFitter = contents.AddComponent<ContentSizeFitter>();

            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            contents.GetComponent<RectTransform>().anchoredPosition += new Vector2(2f, 0f);

            originalSize = contents.GetComponent<RectTransform>().sizeDelta;

            contents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100);

            VerticalLayoutGroup layoutGroup = contents.AddComponent<VerticalLayoutGroup>();
            layoutGroup.childControlHeight = true;
            // layoutGroup.childForceExpandHeight = false;


            int x = 10;
            int y=-10;  
            NewText("ArmoredSoldier","#eca55f",contents.transform,new Vector3 (x,y));
            y-=10;
            // NewText("Knoble","#450c79",contents.transform,new Vector3 (x,y));
            // y-=10;
            // NewText("armyNum","#F0FFFF",contents.transform,new Vector3 (x,y));
            // y-=10;
            UpdateDictionaryWithZero(CustomNums);


            // NewUI.createBgWindowButtonLeft(
            //     GameObject.Find($"Canvas Container Main/Canvas - Windows/windows/village"),
            //     posY: 20,
            //     iconName: "iconPolicy",
            //     buttonName: "军团",
            //     buttonTitle: "军团",
            //     buttonDesc: "军团",
            //     call: openWindow
            // );

            NameHolder = new GameObject("NameHolder");
            NameHolderImg = NameHolder.AddComponent<Image>();
            NameHolderImg.color = new Color(0f, 0f, 0f, 0.01f);
            NameHolder.transform.SetParent(contents.transform);
            nameInput = NewUI.createInputOption(
                "LegionNameInput",
                "",
                "",
                -50,
                NameHolder,
                ""
            );
            // nameInput.transform.parent.GetComponent<RectTransform>().localPosition = new Vector3(0, -50, 0);

        }


        

        public static void openWindow()
        {
            Windows.ShowWindow("LegionWindow");

            if (currentCity == null)
            {
                Debug.LogWarning("No city selected.");
                return;
            }
            if (currentCity.army == null)
            {
                return;
            }
            Ui();
        }

        public static void Ui()
        {
            City city = currentCity;

            UpdateDictionaryWithZero(CustomNums);

            UnitGroup unit_group=city.army;
            List<Actor> simpleList = unit_group.units.getSimpleList();

            for (int i = 0; i < simpleList.Count; i++)
            {
                if(simpleList[i]!=null&&simpleList[i].asset!=null)
                {
                if(simpleList[i].asset.unit)
                {
                    if (Armour(simpleList[i]))
                    {
                        CustomNums["ArmoredSoldier"]++;
                    }
                }
                }
            }
            foreach (string pid in CustomTextIds)
            {
                if (CustomTexts.TryGetValue(pid, out var customText) && customText != null && CustomTextColors.TryGetValue(pid, out var textColor))
                {
                    double proportion = Math.Round(CustomNums[pid] * 100 / city.getArmy(), 2);

                    // if (pid == "culture")
                    // {
                    //     UpdateCultureText(pid, str);
                    // }
                    // else if (pid == "race")
                    // {
                    //     UpdateRaceText(pid, str);
                    // }
                    // else if (pid == "Population")
                    // {
                    //     customText.text = $"<color={textColor}><b>{LocalizedTextManager.getText("tree_text_" + pid, null)}: {CustomNums[pid]}</b></color>";
                    // }
                    // else
                    // {
                        customText.text = $"<color={textColor}><b>{LocalizedTextManager.getText("tree_text_" + pid, null)}: {CustomNums[pid]} ({proportion}%)</b></color>";
                    // }
                }

            }


            
        }
        private void applyInputName(string pInput)
        {
            if (string.IsNullOrEmpty(pInput))
            {
                return;
            }
            if (currentCity == null || currentCity.data == null)
            {
                return;
            }
            currentCity.data.name = pInput;
        }

        public static bool Armour(Actor pActor = null)
        {
            if (pActor == null) 
            {
                return false;
            }

            if (pActor.asset.unit && pActor.equipment != null && ActorEquipment.getList(pActor.equipment) != null)
            {
                var Ar = pActor.equipment.armor;
                if (Ar != null && Ar.data != null) // 检查Ar是否为空
                {
                    return true;
                }
            }
            // pActor.city.giveItem(pActor, pActor.city.getEquipmentList(EquipmentType.Armor), pActor.city);
            return false;
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


    
        public static Text NewText(string id, string color,Transform parentr, Vector2 position)
        {
            GameObject MRef = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/LegionWindow/Background/Name");
            GameObject MObj = GameObject.Instantiate(MRef, parentr);
            MObj.SetActive(true);
            Text MText = MObj.GetComponent<Text>();
            string text = LocalizedTextManager.getText("tree_text_" + id, null);
            MText.text = $"<color={color}><b>{text}</b></color>";
            MText.supportRichText = true;
            MText.alignment = TextAnchor.MiddleLeft;
            MText.transform.SetParent(parentr);
            var MObjRTF = MText.GetComponent<RectTransform>();
            MObjRTF.position = new Vector3(0f, 0f, 0f);
            MObjRTF.localPosition = position;
            MObjRTF.sizeDelta = new Vector2(20, 10);
            CustomNums.Add(id, 0);
            CustomTextIds.Add(id);
            CustomTextColors.Add(id, color);
            CustomTexts.Add(id, MText);
            return MText;
        }

        public static string NewString(string id, string color, Text MText)
        {
            string text = LocalizedTextManager.getText("tree_text_" + id, null);
            CustomNums.Add(id, 0);
            CustomTexts.Add(id, MText);
            CustomTextColors.Add(id, color);
            CustomTextIds.Add(id);
            return "";
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