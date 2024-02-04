using HarmonyLib;
using System;
using NeoModLoader.api;
using NeoModLoader.services;using NCMS;
using NCMS.Utils;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using ReflectionUtility;
using System.Threading;
using System.Text;
using ai;
using ai.behaviours;

namespace AristocracyAndPower
{
	class BehCheckArrow : BehaviourActionActor
	{
		public override BehResult execute(Actor pActor)
		{
			if (BehaviourActionBase<Actor>.world.worldLaws.world_law_peaceful_monsters.boolVal){return BehResult.Stop;}
			if (pActor.tileTarget != null && pActor.attackTarget == null || pActor.attackTarget != null){return BehResult.Stop;}
			if (pActor!=null&&pActor.equipment != null && !pActor.equipment.getSlot(EquipmentType.Weapon).isEmpty())
			{
				if(pActor.getWeaponAsset().projectile=="arrow")
				{
					pActor.data.get("arrow", out int arrow, 0);
					City pCity=pActor.city;
					if(arrow<=10&&pCity!=null)
					{
						pActor.ai.setTask("try_to_return_home");
						int wood=pCity.data.storage.get("wood");
						int CArrow=pCity.data.storage.get("tree_text_arrow");
						if(CArrow<=0)
						{
							if (wood <= 0)
							{
								return BehResult.Continue;
							}
							else
							{
								while(wood==0||arrow>10)
								{
									wood=pCity.data.storage.get("wood");
									pActor.data.get("arrow", out arrow, 0);
									pCity.data.storage.change("wood", -1);
									pActor.data.change("arrow", 3);
								}
							}
						}
						else
						{
							while(CArrow==0||arrow>10)
							{
								CArrow=pCity.data.storage.get("tree_text_arrow");
								pActor.data.get("arrow", out arrow, 0);
								pCity.data.storage.change("tree_text_arrow", -1);
								pActor.data.change("arrow", 1);
							}
						}

					}
				}
			}
		  	return BehResult.Continue;
		}
	}
}