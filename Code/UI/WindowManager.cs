using System;
using System.Collections;
using System.Collections.Generic;
using NeoModLoader.api;
using NeoModLoader.services;using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;

namespace AristocracyAndPower.UI
{
    class WindowManager
    {
        public static Dictionary<string, GameObject> windowContents = new Dictionary<string, GameObject>();
        public static Dictionary<string, ScrollWindow> createdWindows = new Dictionary<string, ScrollWindow>();


        public static void init()
        {
            newWindow("MunicipalWindow", "市政窗口");
            MunicipalWindow.init();
            newWindow("PopulationWindow", "人口");
            PopulationWindow.init();
            newWindow("policyWindow", "政策");
            policyWindow.init();
            newWindow("EconomicWindow", "经济");
            EconomicWindow.init();
            newWindow("NobleWindow", "贵族");
            NobleWindow.init();
            newWindow("LegionWindow", "军团");
            LegionWindow.init();
        }

        private static void newWindow(string id, string title)
        {
            ScrollWindow window;
            GameObject content;
            window = Windows.CreateNewWindow(id, title);
            createdWindows.Add(id, window);

            GameObject scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View");
            scrollView.gameObject.SetActive(true);

            content = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{window.name}/Background/Scroll View/Viewport/Content");
            if (content != null)
            {
                windowContents.Add(id, content);
            }
        }

        public static void updateScrollRect(GameObject content, int count, int size)
        {
            var scrollRect = content.GetComponent<RectTransform>();
            scrollRect.sizeDelta = new Vector2(0, count * size);
        }
    }
}