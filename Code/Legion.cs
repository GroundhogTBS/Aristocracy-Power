using AristocracyAndPower;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// using UnityEngine.Random;
using System;
namespace AristocracyAndPower;


public class Legion : UnitGroup
{

    public Actor heir;
    // public Actor leader;
    public string name;
    public int military;
    public int dead;
    public int year;

    public void clearHeirData()
    {
        this.heir = null;
    }
    public bool hasHeir()
    {
        return this.heir != null;
    }
    public void Dispose()
    {
        this.heir = null;
        // this.leader=null;
        this.name=null;
        this.military=0;
        this.dead=0;
        this.year=0;
        base.Dispose();


    }

    public Legion(City pCity) : base(pCity)
    {
        // base.UnitGroup(pCity);
        this.city=pCity;
        if(pCity!=null)
        {
            this.name=CreatName(pCity.data.race,pCity);
        }
    }


    public void SetHeir(Actor pActor)
	{
        this.clearHeirData();
		this.heir= pActor;
		
	}
    public Actor FindHeir()
    {
        Debug.Log("军团开始找继承人"); 
        List<Actor> candidates = new List<Actor>();

        // 假设继承人来自将军家族
        if(this.groupLeader!=null)
        {
            Clan royalClan = this.groupLeader.getClan();
            if (royalClan != null)
            {
                foreach (var member in royalClan.units.Values)
                {
                    // 添加适合的候选人
                    if (IsSuitableForHeir(member))
                    {
                        candidates.Add(member);
                    }
                }
            }
            else{candidates=this.units.getSimpleList();}
        }
        else{candidates=this.units.getSimpleList();}

        // 根据某些标准排序候选人
        candidates.Sort((a, b) => CompareCandidates(a, b));

        // 返回最合适的候选人
        
        Actor heir = candidates.FirstOrDefault();
        if (heir != null)
        {
            Debug.Log("军团找到了合适的继承人: " + heir.data.name); 
            return heir;
        }
        else
        {
            Debug.Log("军团没找到合适的继承人");
            return null;
        }
    }
    private bool IsSuitableForHeir(Actor member)
    {
        // 检查成员是否活着，年龄是否符合，并且不是当前的国王
        return member.isAlive() && !member.isKing()&&!member==this.groupLeader;
    }


    private int CompareCandidates(Actor a, Actor b)
    {
        // 定义比较候选人的逻辑，例如根据年龄、领导能力等
        a.data.get("military exploit", out int num,0);
        b.data.get("military exploit", out int num2,0);
        return num.CompareTo(num2); // 示例逻辑
    }

    public string CreatName(string race,City pCity)
    {
        System.Random rand = new System.Random();
        string[] Names = { "狮魂", "龙威", "狼群", "风暴", "猎鹰","锦绣", "明月", "碧水", "金秋", "翠峰" };
        string[] orc = 
        {
            "狂暴",
            "血蹄",
            "暗影",
            "雷鸣",
            "战斧",
            "裂骨",
            "铁牙",
            "噬魂",
            "蛮鬃",
            "血蹄"
        };

        string[] human = 
        {
            "圣剑",
            "荣耀",
            "不朽",
            "王室",
            "黄金",
            "铁甲",
            "日耳曼",
            "狮心",
            "法兰克",
            "梅里奥尼"
        };

        string[] elf = 
        {
            "星辰",
            "长弓",
            "翡翠",
            "魔枪",
            "雪峰",
            "碧玉",
            "幽林",
            "羽翼",
            "水玫瑰",
            "灵音"
        };

        string[] darwf = 
        {
            "铁山",
            "深洞",
            "黄金",
            "钢铸",
            "石锤",
            "炉火",
            "蛮斧",
            "鹰眼",
            "炸药",
            "冰雪"
        };

        string[] Rome = 
        {
            "帝国",
            "鹰翼",
            "荆棘",
            "狂暴",
            "血斧",
            "地狱",
            "罗马",
            "神盾",
            "铁血",
            "不败"
        };

        string[] Arab = 
        {
            "沙漠风暴",
            "炎炉",
            "喷火",
            "征服",
            "勇士",
            "大帝",
            "黄金",
            "沙漠之刃",
            "炽炎",
            "隐匿"
        };

        string[] Xia = 
        {
            "龙鳞",
            "雷电",
            "虎威",
            "凤舞",
            "神龙",
            "龙吟",
            "玄武",
            "紫金",
            "惊蛰",
            "九天"
        };
        if(race=="orc"){Names=orc;}
        else if(race=="elf"){Names=elf;}
        else if(race=="darwf"){Names=darwf;}
        else if(race=="human"){Names=human;}
        else if(race=="Rome"){Names=Rome;}
        else if(race=="Arab"){Names=Arab;}
        else if(race=="Xia"){Names=Xia;}
        string randomRaceName = Names[rand.Next(Names.Length)];
        return $"{randomRaceName}{pCity.data.name}军团";
    }   
}