using System;
using System.Collections.Generic;


namespace AristocracyAndPower
{
	public class ActorFix
	{
		// Token: 0x06000551 RID: 1361 RVA: 0x0005BC84 File Offset: 0x00059E84
		public static void init()
		{
			var orc=AssetManager.actor_library.get(SA.unit_orc);
			orc.maxHunger=140;
			var elf=AssetManager.actor_library.get(SA.unit_elf);
			elf.maxHunger=120;
			var dwarf=AssetManager.actor_library.get(SA.unit_dwarf);
			dwarf.maxHunger=80;
			// var orc=AssetManager.actor_library.get("orc");
			// orc.maxHunger=140;



	}



	}
}
