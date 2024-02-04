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
using AristocracyAndPower;
using DG.Tweening;

namespace AristocracyAndPower.UI
{
    class MunicipalWindow : MonoBehaviour
    {
        public static GameObject contents;
        private static GameObject scrollView;

        public static ScrollRect scrollRect;
        private static Vector2 originalSize;
        public static MunicipalWindow instance;

        public static Kingdom currentKingdom;
        public static GameObject smallButton;
        public static Dictionary<string, Sprite> dataCollective = new Dictionary<string, Sprite>();
        public static Dictionary<string, GameObject> dataCollectiveButton =
            new Dictionary<string, GameObject>();
        public static Dictionary<string, string> CustomTextColors = new Dictionary<string, string>();
        public static Dictionary<string, Text> CustomTexts = new Dictionary<string, Text>();
        public static List<string> CustomTextIds = new List<string>();

        public static void init()
        {
            contents = WindowManager.windowContents["MunicipalWindow"];

            scrollView = GameObject.Find(
                "Canvas Container Main/Canvas - Windows/windows/MunicipalWindow/Background/Scroll View"
            );
            GameObject Background = GameObject.Find(
                "Canvas Container Main/Canvas - Windows/windows/MunicipalWindow/Background"
            );
            Image image1 = Background.GetComponent<Image>();
            image1.sprite = Mod.EmbededResources.LoadSprite(
                $"{Mod.Info.Name}.Resources.UI.政府总览窗口.png"
            );
            // 获取ScrollRect组件
            scrollRect = scrollView.GetComponentInChildren<ScrollRect>();
            scrollView.GetComponent<RectTransform>().sizeDelta -= new Vector2(50, 0);
            scrollView.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 10);
            instance = new GameObject("MunicipalWindow").AddComponent<MunicipalWindow>();

            ContentSizeFitter contentSizeFitter = contents.AddComponent<ContentSizeFitter>();

            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            contents.GetComponent<RectTransform>().anchoredPosition += new Vector2(2f, 0f);

            originalSize = contents.GetComponent<RectTransform>().sizeDelta;

            contents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 100);


            NewLaw("Population", "人口", new Vector3(135,-180), PopulationWindow.openWindow, contents);
            NewLaw("Money", "钱钱", new Vector3(70,-230), EconomicWindow.openWindow, contents);
            NewLaw("noble", "贵族", new Vector3(70,-180), NobleWindow.openWindow, contents);
            NewLaw("Policy", "政策", new Vector3(135,-230), policyWindow.openWindow, contents);


            NewUI.createBgWindowButtonLeft(
                GameObject.Find($"Canvas Container Main/Canvas - Windows/windows/kingdom"),
                posY: -40,
                iconName: "iconPolicy",
                buttonName: "MunicipalWindow",
                buttonTitle: "MunicipalWindow",
                buttonDesc: "MunicipalWindow",
                call: openWindow
            );
        }

        public static void openWindow()
        {
            Windows.ShowWindow("MunicipalWindow");

            Ui();
        }

        public static void Ui()
        {
            Kingdom kingdom = currentKingdom;
        }

        private static GameObject NewbuttonGrid
        {
            get
            {
                var buttonGrid = new GameObject($"buttoGrid");
                RectTransform buttonGridRectTransform = buttonGrid.GetComponent<RectTransform>();
                if (buttonGridRectTransform == null)
                {
                    buttonGridRectTransform = buttonGrid.AddComponent<RectTransform>();
                }

                var gridLayout = buttonGrid.AddComponent<GridLayoutGroup>();
                gridLayout.cellSize = new Vector2(50, 80); // 设置每个按钮的大小
                gridLayout.spacing = new Vector2(0, 1); // 设置按钮之间的间距
                gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft; // 设置起始角落
                gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal; // 设置起始轴为水平方向
                gridLayout.childAlignment = TextAnchor.MiddleCenter;
                gridLayout.constraint = GridLayoutGroup.Constraint.Flexible; // 设置限制为自适应
                gridLayout.constraintCount = 2;
                buttonGrid.transform.SetParent(contents.transform, false);
                return buttonGrid;
            }
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
          ButtonTip.textOnClickDescription = id + "Description";
          NewButton.gameObject.SetActive(true);
          return NewButton;
        }

        private static Text Newtext
        {
            get
            {
                GameObject textobj = new GameObject("textobj");
                Text textobjComponent = textobj.AddComponent<Text>();

                var textContentSizeFitter = textobj.gameObject.AddComponent<ContentSizeFitter>();
                textContentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                textobjComponent.text = "";
                Color dateOfBirthcolor = new Color();
                ColorUtility.TryParseHtmlString("#FFFFFF", out dateOfBirthcolor);
                textobjComponent.color = dateOfBirthcolor;
                textobjComponent.fontSize = 8;
                textobjComponent.font = (Font)Resources.Load("Fonts/Roboto-Bold", typeof(Font));
                textobjComponent.fontStyle = FontStyle.Bold;
                textobjComponent.alignment = TextAnchor.MiddleCenter;
                RectTransform rectTransform = textobj.GetComponent<RectTransform>();
                if (rectTransform == null)
                {
                    rectTransform = textobj.AddComponent<RectTransform>();
                }
                rectTransform.anchoredPosition = new Vector2(0, 0);
                rectTransform.sizeDelta = new Vector2(20, 100);
                Outline outline = textobj.gameObject.AddComponent<Outline>();
                outline.effectColor = Color.black;
                outline.useGraphicAlpha = true;
                outline.effectDistance = new Vector2(0.5f, -0.5f);
                textobj.transform.SetParent(contents.transform, false);
                return textobjComponent;
            }
        }
        public static Text NewText(string id, string color, Transform parentr, Vector3 position)
        {
          GameObject MRef = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/SHAuxHelper/Background/Name");
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
          CustomTextIds.Add(id);
          return MText;
        }
    }
}
