using System;
using System.Text;
using System.Threading.Tasks;
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

namespace AristocracyAndPower
{
    class NewUI : MonoBehaviour
    {   // thanks to Dej
        private static GameObject avatarRef= GameObject.Find(
                $"Canvas Container Main/Canvas - Windows/windows/inspect_unit/Background/Scroll View/Viewport/Content/Part 1/BackgroundAvatar"
            );        
        public static void createActorUI(Actor actor, GameObject parent, Vector3 pos)
        {
            GameObject GO = Instantiate(avatarRef);
            GO.transform.SetParent(parent.transform);
            var avatarElement = GO.GetComponent<UiUnitAvatarElement>();
            avatarElement.show_banner_clan = true;
            avatarElement.show_banner_kingdom = true;
            avatarElement.show(actor);
            RectTransform GORect = GO.GetComponent<RectTransform>();
            GORect.localPosition = pos;
            GORect.localScale = new Vector3(0.5f, 1, 1);

            Button GOButton = GO.AddComponent<Button>();
            GOButton.OnHover(new UnityAction(() => actorTooltip(actor)));
			GOButton.OnHoverOut(new UnityAction(Tooltip.hideTooltip));
            GOButton.onClick.AddListener(() => showActor(actor));
            GO.AddComponent<GraphicRaycaster>();
        }
        private static GameObject textRef;



        public static Button createBgWindowButtonLeft(
            GameObject parent,
            int posY,
            string iconName,
            string buttonName,
            string buttonTitle,
            string buttonDesc,
            UnityAction call
        )
        {
            PowerButton button = PowerButtons.CreateButton(
                buttonName,
                Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.Icons.{iconName}.png"),
                buttonTitle,
                buttonDesc,
                new Vector2(-118, posY),
                ButtonType.Click,
                parent.transform,
                call
            );

            Image buttonBG = button.gameObject.GetComponent<Image>();
            buttonBG.sprite = Mod.EmbededResources.LoadSprite(
                $"{Mod.Info.Name}.Resources.UI.backgroundTabButton.png"
            );
            Button buttonButton = button.gameObject.GetComponent<Button>();
            buttonBG.rectTransform.localScale = Vector3.one;

            return buttonButton;
        }
        public static void showcity(City city)
        {
            if (city == null)
            {
                return;
            }
            Config.selectedCity = city;
            ScrollWindow.showWindow("village");
            return;
        }
        public static void showclan(Clan clan)
        {
            if (clan == null)
            {
                return;
            }
            Config.selectedClan = clan;
            ScrollWindow.showWindow("clans");
            return;
        }
        public static void cityTooltip(City city)
        {
            if (city == null)
            {
                return;
            }
            string text = "city";

            Tooltip.show(city, text, new TooltipData
            {
                city = city,
            });
            return;
        }
        public static void clanTooltip(Clan clan)
        {
            if (clan == null)
            {
                return;
            }
            string text = "clan";

            Tooltip.show(clan, text, new TooltipData
            {
                clan = clan,
            });
            return;
        }
        private static void actorTooltip(Actor actor)
        {
            if (actor == null)
            {
                return;
            }
            string text = "actor";
            if (actor.isKing())
            {
                text = "actor_king";
            }
            else if (actor.isCityLeader())
            {
                text = "actor_leader";
            }
            Tooltip.show(actor, text, new TooltipData
            {
                actor = actor,
            });
            return;
        }
        public static void TextTooltip(string Key)
        {

            Tooltip.show(null, "normal", new TooltipData
            {
                tip_name = Key,
                tip_description = Key
            });
            return;
        }

        public static void showActor(Actor pActor)
        {
            if (pActor != null)
            {
                Config.selectedUnit = pActor;
                ScrollWindow.showWindow("inspect_unit");
            }

        }

        public static Button createBGWindowButton(GameObject parent, int posY, string iconName, string buttonName, string buttonTitle,
        string buttonDesc, UnityAction call)
        {
            PowerButton button = PowerButtons.CreateButton(
                buttonName,
                Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.Icons.{iconName}.png"),
                buttonTitle,
                buttonDesc,
                new Vector2(118, posY),
                ButtonType.Click,
                parent.transform,
                call
            );

            Image buttonBG = button.gameObject.GetComponent<Image>();
            buttonBG.sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.UI.backgroundTabButton.png");
            Button buttonButton = button.gameObject.GetComponent<Button>();
            buttonBG.rectTransform.localScale = Vector3.one;

            return buttonButton;
        }

        public static PowerButton createBGWindowButtonMet(GameObject parent, Vector2 pos, string iconName, string buttonName, string buttonTitle,
        string buttonDesc, UnityAction call)
        {
            PowerButton button = PowerButtons.CreateButton(
                buttonName,
                Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.Icons.{iconName}.png"),
                buttonTitle,
                buttonDesc,
                pos,
                ButtonType.Click,
                parent.transform,
                call
            );

            Image buttonBG = button.gameObject.GetComponent<Image>();
            buttonBG.sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.UI.backgroundTabButton.png");
            Button buttonButton = button.gameObject.GetComponent<Button>();
            buttonBG.rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            return button;
        }

        public static Text addText(string textString, GameObject parent, int sizeFont, Vector3 pos, Vector2 addSize = default(Vector2))
        {
            textRef = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/village/Background/Title");
            GameObject textGo = Instantiate(textRef, parent.transform);
            textGo.SetActive(true);

            var textComp = textGo.GetComponent<Text>();
            textComp.fontSize = sizeFont;
            textComp.resizeTextMaxSize = sizeFont;
            var textRect = textGo.GetComponent<RectTransform>();
            textRect.position = new Vector3(0, 0, 0);
            textRect.localPosition = pos + new Vector3(0, -50, 0);
            textRect.sizeDelta = new Vector2(100, 100) + addSize;
            textGo.AddComponent<GraphicRaycaster>();
            textComp.text = textString;

            return textComp;
        }

        public static void loadTraitButton(string pID, Vector2 pos, GameObject parent)
        {
            WindowCreatureInfo info = GameObjects.FindEvenInactive("inspect_unit").GetComponent<WindowCreatureInfo>();
            TraitButton traitButton = Instantiate<TraitButton>(info.prefabTrait, parent.transform);
            Reflection.CallMethod(traitButton, "Awake");
            Reflection.CallMethod(traitButton, "load", pID);
            RectTransform component = traitButton.GetComponent<RectTransform>();
            component.localPosition = pos;
        }


        public static NameInput createInputOption(string objName, string title, string desc, int posY, GameObject parent, string textValue = "-1")
        {
            GameObject statHolder = new GameObject("OptionHolder");
            statHolder.transform.SetParent(parent.transform);
            Image statImage = statHolder.AddComponent<Image>();
            statImage.sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.UI.windowInnerSlicedTopRound.png");
            RectTransform statHolderRect = statHolder.GetComponent<RectTransform>();
            statHolderRect.localPosition = new Vector3(0, posY, 0);
            statHolderRect.sizeDelta = new Vector2(250, 60);

            // Text statText = addText(title, statHolder, 20, new Vector3(0, 110, 0), new Vector2(100, 0));
            // RectTransform statTextRect = statText.gameObject.GetComponent<RectTransform>();
            // statTextRect.sizeDelta = new Vector2(statTextRect.sizeDelta.x, 80);

            // Text descText = addText(desc, statHolder, 20, new Vector3(0, 60, 0), new Vector2(300, 0));
            // RectTransform descTextRect = descText.gameObject.GetComponent<RectTransform>();
            // descTextRect.sizeDelta = new Vector2(descTextRect.sizeDelta.x, 80);

            GameObject inputRef = NCMS.Utils.GameObjects.FindEvenInactive("NameInputElement");

            GameObject inputField = Instantiate(inputRef, statHolder.transform);
            NameInput nameInputComp = inputField.GetComponent<NameInput>();
            nameInputComp.setText(textValue);
            RectTransform inputRect = inputField.GetComponent<RectTransform>();
            inputRect.localPosition = new Vector3(0, -20, 0);
            inputRect.sizeDelta += new Vector2(120, 50);

            GameObject inputChild = inputField.transform.Find("InputField").gameObject;
            RectTransform inputChildRect = inputChild.GetComponent<RectTransform>();
            inputChildRect.sizeDelta = new Vector2(105, 20);
            Text inputChildText = inputChild.GetComponent<Text>();
            inputChildText.resizeTextMaxSize = 20;
            return nameInputComp;
        }

        public static string checkStatInput(NameInput pInput = null, string pText = null)
        {
            string text = pText;
            if (pInput != null)
            {
                text = pInput.inputField.text;
            }
            int num = -1;
            if (!int.TryParse(text, out num))
            {
                return "0";
            }
            if (num > 9999)
            {
                return "9999";
            }
            if (num < -9999)
            {
                return "-9999";
            }
            return text;
        }

        public static Button createTextButtonWSize(string name, string title, Vector2 pos, Color color, Transform parent, UnityAction callback, Vector2 size)
        {
            Button textButton = PowerButtons.CreateTextButton(
                name,
                title,
                pos,
                color,
                parent,
                callback
            );
            if (title.Length > 7)
            {
                textButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta += new Vector2(0, 10);
            }
            textButton.gameObject.GetComponent<RectTransform>().sizeDelta += size;
            return textButton;
        }


        public static void createTab(string buttonID, string tabID, string name, string desc, int xPos)
        {
            GameObject OtherTabButton = GameObjects.FindEvenInactive("Button_Other");
            if (OtherTabButton != null)
            {
                Localization.AddOrSet(buttonID, name);
                Localization.AddOrSet($"{buttonID} Description", desc);
                Localization.AddOrSet("aw2_mod_creator", "Mod by GoatDie#4068\nTab By: Dej#0594");
                Localization.AddOrSet(tabID, name);


                GameObject newTabButton = GameObject.Instantiate(OtherTabButton);
                newTabButton.transform.SetParent(OtherTabButton.transform.parent);
                Button buttonComponent = newTabButton.GetComponent<Button>();
                TipButton tipButton = buttonComponent.gameObject.GetComponent<TipButton>();
                tipButton.textOnClick = buttonID;
                tipButton.textOnClickDescription = $"{buttonID} Description";
                tipButton.text_description_2 = "aw2_mod_creator";



                newTabButton.transform.localPosition = new Vector3(xPos, 49.62f);
                newTabButton.transform.localScale = new Vector3(1f, 1f);
                newTabButton.name = buttonID;

                Sprite spriteForTab = Sprites.LoadSprite($"{Mod.Info.Path}/icon.png");
                newTabButton.transform.Find("Icon").GetComponent<Image>().sprite = spriteForTab;


                GameObject OtherTab = GameObjects.FindEvenInactive("Tab_Other");
                foreach (Transform child in OtherTab.transform)
                {
                    child.gameObject.SetActive(false);
                }

                GameObject additionalPowersTab = GameObject.Instantiate(OtherTab);

                foreach (Transform child in additionalPowersTab.transform)
                {
                    if (child.gameObject.name == "tabBackButton" || child.gameObject.name == "-space")
                    {
                        child.gameObject.SetActive(true);
                        continue;
                    }

                    GameObject.Destroy(child.gameObject);
                }

                foreach (Transform child in OtherTab.transform)
                {
                    child.gameObject.SetActive(true);
                }


                additionalPowersTab.transform.SetParent(OtherTab.transform.parent);
                PowersTab powersTabComponent = additionalPowersTab.GetComponent<PowersTab>();
                powersTabComponent.powerButton = buttonComponent;
                powersTabComponent.powerButtons.Clear();


                additionalPowersTab.name = tabID;
                powersTabComponent.powerButton.onClick = new Button.ButtonClickedEvent();
                powersTabComponent.powerButton.onClick.AddListener(() => tabOnClick(tabID));
                Reflection.SetField<GameObject>(powersTabComponent, "parentObj", OtherTab.transform.parent.parent.gameObject);

                additionalPowersTab.SetActive(true);
                powersTabComponent.powerButton.gameObject.SetActive(true);
            }
        }

        public static void tabOnClick(string tabID)
        {
            GameObject AdditionalTab = GameObjects.FindEvenInactive(tabID);
            PowersTab AdditionalPowersTab = AdditionalTab.GetComponent<PowersTab>();

            AdditionalPowersTab.showTab(AdditionalPowersTab.powerButton);
        }

        public static GameObject createSubWindow(GameObject parent, Vector3 pos, Vector2 size, Vector2 infoSize)
        {
            GameObject parentScrollHolder = new GameObject("scrollHolder");
            parentScrollHolder.transform.SetParent(parent.transform);
            Image scrollImg = parentScrollHolder.AddComponent<Image>();
            scrollImg.sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.UI.windowInnerSliced.png");
            RectTransform scrollHolderRect = parentScrollHolder.GetComponent<RectTransform>();
            scrollHolderRect.localPosition = pos;
            scrollHolderRect.sizeDelta = size;

            GameObject infoHolder = new GameObject("titleInfoHolder");
            infoHolder.transform.SetParent(parentScrollHolder.transform);
            infoHolder.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
            RectTransform infoRect = infoHolder.GetComponent<RectTransform>();
            infoRect.sizeDelta = infoSize;
            ScrollRect scroll = parentScrollHolder.AddComponent<ScrollRect>();
            scroll.scrollSensitivity = 13f;
            scroll.viewport = parentScrollHolder.GetComponent<RectTransform>();
            scroll.content = infoHolder.GetComponent<RectTransform>();
            parentScrollHolder.AddComponent<Mask>();
            infoRect.localPosition = new Vector3(25, 0, 0);

            return infoHolder;
        }
        public static GameObject createSubWindowfull(GameObject parent, Vector3 pos, Vector2 size, Vector2 infoSize)
        {
            GameObject parentScrollHolder = new GameObject("scrollHolder");
            parentScrollHolder.transform.SetParent(parent.transform);
            Image scrollImg = parentScrollHolder.AddComponent<Image>();
            scrollImg.rectTransform.localScale = Vector3.one;
            scrollImg.sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.UI.windowInnerSliced.png");
            RectTransform scrollHolderRect = parentScrollHolder.GetComponent<RectTransform>();
            scrollHolderRect.localPosition = pos;
            scrollHolderRect.sizeDelta = size;

            GameObject infoHolder = new GameObject("titleInfoHolder");
            infoHolder.transform.SetParent(parentScrollHolder.transform);
            infoHolder.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
            RectTransform infoRect = infoHolder.GetComponent<RectTransform>();
            infoRect.sizeDelta = infoSize;
            ScrollRect scroll = parentScrollHolder.AddComponent<ScrollRect>();
            scroll.scrollSensitivity = 13f;
            scroll.viewport = parentScrollHolder.GetComponent<RectTransform>();
            scroll.content = infoHolder.GetComponent<RectTransform>();
            parentScrollHolder.AddComponent<Mask>();
            infoRect.localPosition = new Vector3(25, 0, 0);

            return infoHolder;
        }
        public static GameObject createProgressBar(GameObject parent, Vector3 pos)
        {
            GameObject researchBar = GameObjects.FindEvenInactive("HealthBar");
            GameObject progressBar = Instantiate(researchBar, parent.transform);
            progressBar.name = "ProgressBar";
            progressBar.SetActive(true);

            RectTransform progressRect = progressBar.GetComponent<RectTransform>();
            progressRect.localPosition = pos;

            StatBar statBar = progressBar.GetComponent<StatBar>();
            statBar.CallMethod("restartBar");

            TipButton tipButton = progressBar.GetComponent<TipButton>();
            tipButton.textOnClick = "Progress Bar";

            GameObject icon = progressBar.transform.Find("Icon").gameObject;
            icon.SetActive(false);

            return progressBar;
        }

        public static GameObject createCultureBanner(GameObject parent, Culture culture, Vector3 pos)
        {
            GameObject cultureHolder = new GameObject("cultureHolder");
            cultureHolder.transform.SetParent(parent.transform);
            Image cultureHolderImg = cultureHolder.AddComponent<Image>();
            cultureHolderImg.sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.Icons.culture_background.png");

            GameObject partIcon = new GameObject("partIcon");
            partIcon.transform.SetParent(cultureHolder.transform);
            Image partIconImg = partIcon.AddComponent<Image>();
            partIcon.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            GameObject partIconDecoration = new GameObject("partIconDecoration");
            partIconDecoration.transform.SetParent(cultureHolder.transform);
            Image partIconDecorationImg = partIconDecoration.AddComponent<Image>();
            partIconDecoration.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

            cultureHolder.AddComponent<Button>();
            cultureHolder.AddComponent<TipButton>().type = "culture";
            BannerLoaderCulture loader = cultureHolder.AddComponent<BannerLoaderCulture>();
            loader.partIcon = partIconImg;
            loader.partIconDecoration = partIconDecorationImg;
            loader.load(culture);
            cultureHolder.GetComponent<RectTransform>().localPosition = pos;

            return cultureHolder;
        }

        public static GameObject createKingdomBanner(GameObject parent, Kingdom kingdom, Vector3 pos)
        {
            GameObject kingdomHolder = new GameObject("kingdomHolder");
            kingdomHolder.transform.SetParent(parent.transform);
            Image kingdomHolderImg = kingdomHolder.AddComponent<Image>();
            kingdomHolderImg.sprite = Mod.EmbededResources.LoadSprite($"{Mod.Info.Name}.Resources.Icons.kingdombanner.png");

            GameObject backgroundGO = new GameObject("background");
            backgroundGO.transform.SetParent(kingdomHolder.transform);
            Image backgroundImage = backgroundGO.AddComponent<Image>();
            backgroundGO.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

            GameObject partIcon = new GameObject("partIcon");
            partIcon.transform.SetParent(kingdomHolder.transform);
            Image partIconImg = partIcon.AddComponent<Image>();
            partIcon.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

            //调整子对象在父对象的子对象列表中的顺序
            partIcon.transform.SetAsLastSibling();

            kingdomHolder.AddComponent<Button>();
            kingdomHolder.AddComponent<TipButton>().type = "kingdom";
            BannerLoader loader = kingdomHolder.AddComponent<BannerLoader>();
            loader.partIcon = partIconImg;
            loader.partBackround = backgroundImage;
            loader.load(kingdom);
            kingdomHolder.GetComponent<RectTransform>().localPosition = pos;

            return kingdomHolder;
        }

        public static GameObject createClanBanner(GameObject parent, Clan clan, Vector3 pos)
        {
            GameObject clanHolder = new GameObject("clanHolder");
            clanHolder.transform.SetParent(parent.transform, true);

            GameObject backgroundGO = new GameObject("Frame");
            backgroundGO.transform.SetParent(clanHolder.transform);
            Image backgroundImage = backgroundGO.AddComponent<Image>();
            backgroundGO.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

            GameObject partIcon = new GameObject("partIcon");
            partIcon.transform.SetParent(clanHolder.transform);
            Image partIconImg = partIcon.AddComponent<Image>();
            partIcon.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

            //调整子对象在父对象的子对象列表中的顺序
            partIcon.transform.SetAsLastSibling();

            Button button = clanHolder.AddComponent<Button>();

            BannerLoaderClans loader = clanHolder.AddComponent<BannerLoaderClans>();
            loader.partIcon = partIconImg;
            loader.partBackround = backgroundImage;

            loader.load(clan);
            clanHolder.AddComponent<RectTransform>().localPosition = pos;

            button.OnHover(new UnityAction(() => NewUI.clanTooltip(clan)));
            button.OnHoverOut(new UnityAction(Tooltip.hideTooltip));
            button.onClick.AddListener(() => NewUI.showclan(clan));
            return clanHolder;
        }



    }
}