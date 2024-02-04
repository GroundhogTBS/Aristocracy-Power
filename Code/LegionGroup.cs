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

namespace AristocracyAndPower
{       
    public class LegionGroup : MonoBehaviour
    {
        public LegionGroup(UnitGroup unit_group)
        {
            this.unitGroup = unit_group;
        }
        public string id="";
        public int dead=0;
        public int kills=0;
        public int military_exploit=0;
        public string name;
        public double created_time;
        public UnitGroup unitGroup;

        public List<Actor> captive = new List<Actor>();
    }
}