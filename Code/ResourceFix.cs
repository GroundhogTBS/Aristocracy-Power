using System;
using System.Collections.Generic;
using AristocracyAndPower;


namespace AristocracyAndPower
{
	public static class ResourceFix
	{
		// Token: 0x06000551 RID: 1361 RVA: 0x0005BC84 File Offset: 0x00059E84
		public static void init()
		{
			berries = AssetManager.resources.get(SR.berries);
			berries.restoreHunger = 15;
			berries.restoreHealth = 0.2f;
			berries.tradeBound = 50;
			berries.tradeCost = 5;
			berries.maximum = 5000;
			addNutritions(SR.berries,1,1,3,0,3);

			bananas = AssetManager.resources.get(SR.bananas);
			bananas.restoreHunger = 15;
			bananas.restoreHealth = 0.2f;
			bananas.tradeBound = 50;
			bananas.tradeCost = 5;
			bananas.maximum = 5000;
			addNutritions(SR.bananas,1,1,4,0,2);

			coconut = AssetManager.resources.get(SR.coconut);
			coconut.restoreHunger = 15;
			coconut.restoreHealth = 0.2f;
			coconut.tradeBound = 50;
			coconut.tradeCost = 5;
			coconut.maximum = 5000;
			addNutritions(SR.coconut,2,5,1,0,0);

			crystal_salt = AssetManager.resources.get(SR.crystal_salt);
			crystal_salt.restoreHunger = 3;
			crystal_salt.restoreHealth = 0.2f;
			crystal_salt.tradeBound = 70;
			crystal_salt.tradeCost = 5;
			crystal_salt.maximum = 5000;
			addNutritions(SR.crystal_salt,0,0,0,2,0);
			desert_berries = AssetManager.resources.get(SR.desert_berries);
			desert_berries.restoreHunger = 13;
			desert_berries.restoreHealth = 0.2f;
			desert_berries.tradeBound = 50;
			desert_berries.tradeCost = 5;
			desert_berries.maximum = 5000;
			addNutritions(SR.desert_berries,1,0,2,0,3);
			evil_beets = AssetManager.resources.get(SR.evil_beets);
			evil_beets.restoreHunger = 16;
			evil_beets.restoreHealth = 0.2f;
			evil_beets.tradeBound = 50;
			evil_beets.tradeCost = 5;
			evil_beets.maximum = 5000;
			addNutritions(SR.evil_beets,0,3,3,0,4);
			mushrooms = AssetManager.resources.get(SR.mushrooms);
			mushrooms.restoreHunger = 15;
			mushrooms.restoreHealth = 0.2f;
			mushrooms.tradeBound = 50;
			mushrooms.tradeCost = 5;
			mushrooms.maximum = 5000;
			addNutritions(SR.mushrooms,2,2,0,0,0);
			peppers = AssetManager.resources.get(SR.peppers);
			peppers.restoreHunger = 10;
			peppers.restoreHealth = 0.2f;
			peppers.tradeBound = 50;
			peppers.tradeCost = 5;
			peppers.maximum = 5000;
			addNutritions(SR.peppers,1,1,0,0,0);
			herbs = AssetManager.resources.get(SR.herbs);
			herbs.restoreHunger = 5;
			herbs.restoreHealth = 0.05f;
			herbs.tradeBound = 30;
			herbs.tradeCost = 5;
			herbs.maximum = 5000;
			addNutritions(SR.herbs,0,0,4,0,3);
			bread = AssetManager.resources.get(SR.bread);
			bread.restoreHunger = 30;
			bread.restoreHealth = 0.1f;
			bread.tradeBound = 50;
			bread.tradeCost = 10;
			bread.maximum = 5000;
			addNutritions(SR.bread,3,1,0,0,0);
			fish = AssetManager.resources.get(SR.fish);
			fish.restoreHunger = 20;
			fish.restoreHealth = 0.15f;
			fish.tradeBound = 55;
			fish.tradeCost = 5;
			fish.maximum = 5000;
			addNutritions(SR.fish,14,1,0,0,0);
			candy = AssetManager.resources.get(SR.candy);
			candy.restoreHunger = 10;
			candy.restoreHealth = 0.1f;
			candy.tradeBound = 50;
			candy.tradeCost = 3;
			candy.maximum = 5000;
			addNutritions(SR.candy,0,0,0,0,0);
			worms = AssetManager.resources.get(SR.worms);
			worms.restoreHunger = 2;
			worms.restoreHealth = -0.02f;
			worms.tradeBound = 30;
			worms.tradeCost = 1;
			worms.maximum = 5000;
			worms.give_status = Toolbox.splitStringIntoList(new string[]
			{
				"poisoned#3","slowness#2"
			});
			worms.give_chance = 0.4f;
			addNutritions(SR.worms,1,0.7,0,0,0);
			snow_cucumbers = AssetManager.resources.get(SR.snow_cucumbers);
			snow_cucumbers.restoreHunger = 5;
			snow_cucumbers.restoreHealth = 0.1f;
			snow_cucumbers.tradeBound = 50;
			snow_cucumbers.tradeCost = 5;
			snow_cucumbers.maximum = 5000;
			addNutritions(SR.worms,0.5,0,1,0,1);
			pine_cones = AssetManager.resources.get(SR.pine_cones);
			pine_cones.restoreHunger = 5;
			pine_cones.restoreHealth = 0.1f;
			pine_cones.tradeBound = 50;
			pine_cones.tradeCost = 5;
			pine_cones.maximum = 5000;
			addNutritions(SR.pine_cones,2,0,0,0,0);
			lemons = AssetManager.resources.get(SR.lemons);
			lemons.restoreHunger = 15;
			lemons.restoreHealth = 0.1f;
			lemons.tradeBound = 50;
			lemons.tradeCost = 5;
			lemons.maximum = 5000;
			addNutritions(SR.lemons,1.2,0.5,3,0,0);
			meat = AssetManager.resources.get(SR.meat);
			meat.restoreHunger = 50;
			meat.restoreHealth = 0.4f;
			meat.tradeBound = 60;
			meat.tradeCost = 12;
			meat.maximum = 5000;
			addNutritions(SR.meat,20,14,0,0,0);
			sushi = AssetManager.resources.get(SR.sushi);
			sushi.restoreHunger = 20;
			sushi.restoreHealth = 0.2f;
			sushi.tradeBound = 50;
			sushi.tradeCost = 5;
			sushi.maximum = 5000;
			addNutritions(SR.sushi,2,0.6,0,0,0);
			jam = AssetManager.resources.get(SR.jam);
			jam.restoreHunger = 15;
			jam.restoreHealth = 0.2f;
			jam.tradeBound = 50;
			jam.tradeCost = 5;
			jam.maximum = 5000;
			addNutritions(SR.jam,0,0.1,1,0,0.6);
			cider = AssetManager.resources.get(SR.cider);
			cider.restoreHunger = 15;
			cider.restoreHealth = 0.2f;
			cider.tradeBound = 50;
			cider.tradeCost = 5;
			cider.maximum = 5000;
			addNutritions(SR.cider,0,0,2,0,0);
			ale = AssetManager.resources.get(SR.ale);
			ale.restoreHunger = 15;
			ale.restoreHealth = 0.2f;
			ale.tradeBound = 50;
			ale.tradeCost = 5;
			ale.maximum = 5000;
			addNutritions(SR.ale,0,0,2,0,0);
			burger = AssetManager.resources.get(SR.burger);
			burger.restoreHunger = 15;
			burger.restoreHealth = 0.2f;
			burger.tradeBound = 50;
			burger.tradeCost = 10;
			burger.maximum = 5000;
			addNutritions(SR.cider,3,6,0,0,0);
			pie = AssetManager.resources.get(SR.pie);
			pie.restoreHunger = 15;
			pie.restoreHealth = 0.2f;
			pie.tradeBound = 50;
			pie.tradeCost = 10;
			pie.maximum = 5000;
			addNutritions(SR.pie,1,3,0,0,0);
			tea = AssetManager.resources.get(SR.tea);
			tea.restoreHunger = 5;
			tea.restoreHealth = 0.35f;
			tea.tradeBound = 80;
			tea.tradeCost = 10;
			tea.maximum = 5000;
			tea.give_status = Toolbox.splitStringIntoList(new string[]
			{
				"enchanted","powerup"
			});
			tea.give_chance = 0.1f;
			addNutritions(SR.tea,0,0,1,0,6);
			honey = AssetManager.resources.get(SR.honey);
			honey.restoreHunger = 15;
			honey.restoreHealth = 0.2f;
			honey.tradeBound = 50;
			honey.tradeCost = 10;
			honey.maximum = 5000;
			addNutritions(SR.honey,0,3,1,0,0);
			wheat = AssetManager.resources.get(SR.wheat);
			wheat.restoreHunger = 5;
			wheat.restoreHealth = 0.2f;
			wheat.tradeBound = 50;
			wheat.tradeCost = 10;
			wheat.maximum = 5000;
			addNutritions(SR.wheat,0,1,2,0,4);

			var gold=AssetManager.resources.get(SR.gold);
			gold.maximum = 1000000;
			var wood=AssetManager.resources.get(SR.wood);
			wood.maximum = 5000;
			wood.tradeCost = 10;
			wood.supplyBoundTake=20;
			var stone=AssetManager.resources.get(SR.stone);
			stone.maximum = 5000;
			stone.tradeCost = 15;
			stone.supplyBoundTake=20;
			var silver=AssetManager.resources.get(SR.silver);
			silver.maximum = 5000;
			silver.tradeCost = 25;
			silver.supplyBoundTake=10;
			var mythril=AssetManager.resources.get(SR.mythril);
			mythril.maximum = 5000;
			mythril.tradeCost = 20;
			mythril.supplyBoundTake=10;
			var adamantine=AssetManager.resources.get(SR.adamantine);
			adamantine.maximum = 5000;
			adamantine.tradeCost = 30;
			adamantine.supplyBoundTake=10;
			var common_metals=AssetManager.resources.get(SR.common_metals);
			common_metals.maximum = 5000;
			common_metals.tradeCost = 15;
			common_metals.supplyBoundTake=10;
			var bones=AssetManager.resources.get(SR.bones);
			bones.maximum = 5000;
			bones.tradeCost = 5;
			bones.supplyBoundTake=10;
			var leather=AssetManager.resources.get(SR.leather);
			leather.maximum = 5000;
			leather.tradeCost = 5;
			leather.supplyBoundTake=10;
			var gems=AssetManager.resources.get(SR.gems);
			gems.maximum = 5000;
			gems.tradeCost = 30;
			gems.supplyBoundTake=5;

			var arrow=AssetManager.resources.add(new ResourceAsset
			{
				id = "tree_text_arrow",
				path_icon = "rescources/iconResArrow",
				maximum = 5000,
				supplyBoundGive = 500,
				supplyBoundTake = 50,
				supplyGive = 100,
				tradeBound = 50,
				tradeCost = 10,
				produce_min=10,
				ingredients = new string[]
				{
					SR.wood,
				},
				type = ResType.Strategic
			});
			var cack=AssetManager.resources.add(new ResourceAsset
			{
				id = "tree_text_cack",
				path_icon = "rescources/cack",
				maximum = 5000,
				supplyBoundGive=4000,
				tradeCost = 1,
				type = ResType.Ingredient
			});
			var tool=AssetManager.resources.add(new ResourceAsset
			{
				id = "tree_text_tool",
				path_icon = "rescources/tools",
				maximum = 5000,
				supplyBoundGive = 500,
				supplyBoundTake = 50,
				supplyGive = 100,
				tradeBound = 50,
				tradeCost = 15,
				produce_min=10,
				ingredients = new string[]
				{
					SR.wood,SR.stone
				},
				type = ResType.Strategic
			});
			checkNutritions();


	}

	public static Dictionary<string, nutrition> nutritions = new Dictionary<string, nutrition>();
	public static void addNutritions(string id,double protein,double fat,double vitamin,double calcium,double dietary_fiber)
	{
		nutrition nutrition=new nutrition(){fat=fat,protein=protein,vitamin=vitamin,calcium=calcium,dietary_fiber=dietary_fiber};
		nutritions.Add(id,nutrition);
	}
	public static void checkNutritions()
	{
		foreach(var food in AssetManager.resources.list)
		{
			if(!nutritions.ContainsKey(food.id)&&food.type == ResType.Food)
			{
				nutrition nutrition=new nutrition(){fat=1,protein=1,vitamin=1,calcium=1,dietary_fiber=1};
				nutritions.Add(food.id,nutrition);
			}
		}

	}
	public static ResourceAsset herbs;

	// Token: 0x040004BF RID: 1215
	public static ResourceAsset mushrooms;

	// Token: 0x040004C0 RID: 1216
	public static ResourceAsset desert_berries;

	// Token: 0x040004C1 RID: 1217
	public static ResourceAsset berries;

	// Token: 0x040004C2 RID: 1218
	public static ResourceAsset peppers;

	// Token: 0x040004C3 RID: 1219
	public static ResourceAsset bananas;

	// Token: 0x040004C4 RID: 1220
	public static ResourceAsset crystal_salt;

	// Token: 0x040004C5 RID: 1221
	public static ResourceAsset coconut;

	// Token: 0x040004C6 RID: 1222
	public static ResourceAsset evil_beets;

	// Token: 0x040004C7 RID: 1223
	public static ResourceAsset lemons;

	// Token: 0x040004C8 RID: 1224
	public static ResourceAsset meat;

	// Token: 0x040004C9 RID: 1225
	public static ResourceAsset sushi;

	// Token: 0x040004CA RID: 1226
	public static ResourceAsset jam;

	// Token: 0x040004CB RID: 1227
	public static ResourceAsset burger;

	// Token: 0x040004CC RID: 1228
	public static ResourceAsset cider;

	// Token: 0x040004CD RID: 1229
	public static ResourceAsset ale;

	// Token: 0x040004CE RID: 1230
	public static ResourceAsset honey;

	// Token: 0x040004CF RID: 1231
	public static ResourceAsset tea;

	// Token: 0x040004D0 RID: 1232
	public static ResourceAsset pie;

	// Token: 0x040004D1 RID: 1233
	public static ResourceAsset wheat;

	// Token: 0x040004D2 RID: 1234
	public static ResourceAsset bread;

	// Token: 0x040004D3 RID: 1235
	public static ResourceAsset fish;

	// Token: 0x040004D4 RID: 1236
	public static ResourceAsset candy;

	// Token: 0x040004D5 RID: 1237
	public static ResourceAsset worms;

	// Token: 0x040004D6 RID: 1238
	public static ResourceAsset pine_cones;

	// Token: 0x040004D7 RID: 1239
	public static ResourceAsset snow_cucumbers;


	}
}
