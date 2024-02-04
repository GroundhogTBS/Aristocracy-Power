using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    class policyWindow : MonoBehaviour
    {
        public static GameObject contents;
        private static GameObject scrollView;

        public static ScrollRect scrollRect;
        private static Vector2 originalSize;
        public static policyWindow instance;

        public static Kingdom currentKingdom;
        public static GameObject smallButton;
        public static Dictionary<string, string> CustomTextColors = new Dictionary<string, string>();
        public static Dictionary<string, Text> CustomTexts = new Dictionary<string, Text>();
        public static List<string> CustomTextIds = new List<string>();

        public static void init()
        {
            contents = WindowManager.windowContents["policyWindow"];

            scrollView = GameObject.Find(
                "Canvas Container Main/Canvas - Windows/windows/policyWindow/Background/Scroll View"
            );
            GameObject Background = GameObject.Find(
                "Canvas Container Main/Canvas - Windows/windows/policyWindow/Background"
            );
            // Image image1 = Background.GetComponent<Image>();
            // image1.sprite = Mod.EmbededResources.LoadSprite(
            //     $"{Mod.Info.Name}.Resources.UI.政府总览窗口.png"
            // );
            // 获取ScrollRect组件
            scrollRect = scrollView.GetComponentInChildren<ScrollRect>();
            scrollView.GetComponent<RectTransform>().sizeDelta -= new Vector2(50, 0);
            scrollView.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 10);
            instance = new GameObject("policyWindow").AddComponent<policyWindow>();

            ContentSizeFitter contentSizeFitter = contents.AddComponent<ContentSizeFitter>();

            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            contents.GetComponent<RectTransform>().anchoredPosition += new Vector2(2f, 0f);

            originalSize = contents.GetComponent<RectTransform>().sizeDelta;

            contents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100);

        }

        

        public static void openWindow()
        {
            Windows.ShowWindow("policyWindow");

            Ui();
        }

        public static void Ui()
        {
            Kingdom kingdom = currentKingdom;
            
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


        public static Text NewText(string id, string color, Transform parentr, Vector3 position)
        {
        GameObject MRef = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/SHAuxHelper/Background/Name");
        GameObject MObj = GameObject.Instantiate(MRef, parentr);
        MObj.SetActive(true);
        Text MText = MObj.GetComponent<Text>();
        string text = LocalizedTextManager.getText("sh_text_" + id, null);
        MText.text = $"<color={color}><b>{text}</b></color>";
        MText.supportRichText = true;
        MText.transform.SetParent(parentr);
        var MObjRTF = MObj.GetComponent<RectTransform>();
        MObjRTF.position = new Vector3(0f, 0f, 0f);
        MObjRTF.localPosition = position;
        CustomTextColors.Add(id, color);
        CustomTexts.Add(id, MText);
        CustomTextIds.Add(id);
        return MText;
        }
    }
}