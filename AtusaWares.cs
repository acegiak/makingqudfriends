using XRL.Rules;
using Qud.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XRL.Core;
using XRL.UI;
using XRL.World.Parts.Skill;
using XRL.World.Tinkering;

namespace XRL.World.Encounters.EncounterObjectBuilders
{
	public class acegiak_AtusaWares : BaseMerchantWares
	{
		public override void Stock(GameObject GO, string Context = null)
		{
			int i = 0;
			for (int num = Stat.Random(5, 7); i < num; i++)
			{
				GO.TakeObject(EncounterFactory.Factory.RollOneFromTable("Scrap 1", Context), true, 0);
			}
			int j = 0;
			for (int num2 = Stat.Random(5, 7); j < num2; j++)
			{
				GO.TakeObject(EncounterFactory.Factory.RollOneFromTable("Scrap 2", Context), true, 0);
			}
			int k = 0;
			for (int num3 = Stat.Random(5, 7); k < num3; k++)
			{
				GO.TakeObject(EncounterFactory.Factory.RollOneFromTable("Scrap 3", Context), true, 0);
			}
			int l = 0;
			for (int num4 = Stat.Random(5, 7); l < num4; l++)
			{
				GO.TakeObject(EncounterFactory.Factory.RollOneFromTable("Scrap 4", Context), true, 0);
			}
			int m = 0;
			for (int num5 = Stat.Random(1, 2); m < num5; m++)
			{
				GO.TakeObject(GameObject.create("DataDisk", Context), true, 0);
			}
			GO.TakeObject(TinkerData.createDataDisk("Autovalet"), true, 0);
			GO.TakeObject(TinkerData.createDataDisk("mod:acegiak_ModTreaded"), true, 0);
			GO.TakeObject(TinkerData.createDataDisk("mod:acegiak_ModHandy"), true, 0);
			int n = 0;
			for (int num6 = Stat.Random(3, 5); n < num6; n++)
			{
				GO.TakeObject(GameObject.create("Waterskin", Context), true, 0);
			}
		}
	}
}
