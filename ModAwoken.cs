using ConsoleLib.Console;
using Qud.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using XRL.Core;
using XRL.Language;
using XRL.Messages;
using XRL.Names;
using XRL.Rules;
using XRL.UI;
using XRL.World.Capabilities;
using XRL.World.Parts.Effects;
using XRL.World.Parts.Mutation;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ModAwoken : IModification
	{

        public bool boot = false;
		public acegiak_ModAwoken()
		{
		}

		public acegiak_ModAwoken(int Tier)
			: base(Tier)
		{
		}

		public override void Configure()
		{
			WorksOnSelf = true;
		}

		public override bool ModificationApplicable(GameObject Object)
		{
			if(Object.GetPart<acegiak_Zombable>() == null
            || Object.GetPart<acegiak_Zombable>().Body == null
            || Object.GetPart<acegiak_Zombable>().Body._Body == null){
                return false;
            }

			return true;
		}
		public override void ApplyModification(GameObject Object)
		{


            Object.Statistics["Energy"] = new Statistic("Energy", -100000, 100000, 0, Object);
            Object.Statistics["Speed"] = new Statistic("Speed", 1, 100000, 100, Object);
            Object.Statistics["MoveSpeed"] = new Statistic("MoveSpeed", -200, 200, 100, Object);
            Object.Statistics["Hitpoints"] = new Statistic("Hitpoints", 0, 64000, 16, Object);
            Object.Statistics["AV"] = new Statistic("AV", 0, 100, 0, Object);
            Object.Statistics["DV"] = new Statistic("DV", -100, 100, 0, Object);


            Object.Statistics["Agility"] = new Statistic("Agility", 1, 9000, 16, Object);
            Object.Statistics["Strength"] = new Statistic("Strength", 1, 9000, 16, Object);
            Object.Statistics["Toughness"] = new Statistic("Toughness", 1, 9000, 16, Object);
            Object.Statistics["Wisdom"] = new Statistic("Wisdom", 1, 9000, 16, Object);
            Object.Statistics["Ego"] = new Statistic("Ego", 1, 9000, 16, Object);
            Object.Statistics["Intelligence"] = new Statistic("Intelligence", 1, 9000, 16, Object);

            Object.Statistics["SP"] = new Statistic("SP", 0, 2147483647, 0, Object);
            Object.Statistics["MP"] = new Statistic("MP", 0, 2147483647, 0, Object);
            Object.Statistics["AP"] = new Statistic("AP", 0, 2147483647, 0, Object);
            Object.Statistics["MA"] = new Statistic("MA", -100, 2147483647, 0, Object);

            Object.Statistics["Level"] = new Statistic("Level", 1, 10000, 1, Object);

            Object.Statistics["XP"] = new Statistic("XP", 0, 2147483647, 0, Object);
            Object.Statistics["XPValue"] = new Statistic("XPValue", 0, 2147483647, 0, Object);
            Object.Statistics["HeatResistance"] = new Statistic("HeatResistance", -100, 100, 0, Object);
            Object.Statistics["ColdResistance"] = new Statistic("ColdResistance", -100, 100, 0, Object);
            Object.Statistics["ElectricalResistance"] = new Statistic("ElectricalResistance", -100, 100, 0, Object);
            Object.Statistics["AcidResistance"] = new Statistic("AcidResistance", -100, 100, 0, Object);

            acegiak_Zombable zomb = Object.GetPart<acegiak_Zombable>();
            Body newBody = new Body();
            newBody._Body = acegiak_Zombable.BodyPartCopy(zomb.Body._Body,Object,newBody);
            Object.AddPart(newBody);

            Brain newBrain = new Brain();
            Object.AddPart(newBrain);
            Object.AddPart(new ConversationScript("acegiak_Zomber"));
            Object.AddPart(new Inventory());
            Object.AddPart(new Leveler());
            Object.AddPart(new Experience());
            Object.AddPart(new Mutations());
            Object.AddPart(new Skills());
            Object.AddPart(new ActivatedAbilities());
            Object.AddPart(new RandomLoot());



            Object.GetPart<Render>().Tile = zomb.storedTile;
            Object.GetPart<Render>().TileColor = "y";
            Object.GetPart<Render>().DetailColor = "r";
		}

		public override bool SameAs(IPart p)
		{
			return base.SameAs(p);
		}

		public override bool AllowStaticRegistration()
		{
			return true;
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "GetDisplayName");
			Object.RegisterPartEvent(this, "GetShortDescription");
			Object.RegisterPartEvent(this, "GetShortDisplayName");

			Object.RegisterPartEvent(this, "Dropped");
            Object.RegisterPartEvent(this, "EnteredCell");
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{

              if (E.ID == "Dropped")
            {
                boot = true;
            }
  
            if (E.ID == "EnteredCell" && boot)
            {
                    boot = false;
                    if(ParentObject.GetPart<Brain>() == null){
                        return false;
                    }

                    ParentObject.GetPart<Brain>().PerformReequip();

                    ParentObject.GetPart<Brain>().BecomeCompanionOf(ParentObject.ThePlayer);
                    ParentObject.GetPart<Brain>().IsLedBy(ParentObject.ThePlayer);
                    ParentObject.GetPart<Brain>().SetFeeling(ParentObject.ThePlayer,100);
					ParentObject.GetPart<Brain>().Goals.Clear();
                    ParentObject.GetPart<Brain>().Calm = false;
                    ParentObject.GetPart<Brain>().Hibernating = false;
                    ParentObject.GetPart<Brain>().FactionMembership.Clear();

                    ParentObject.AddPart(new Combat());

                    XRLCore.Core.Game.ActionManager.AddActiveObject(ParentObject);

            }
			if (E.ID == "GetShortDescription")
			{
				string str = "\n&CAwoken: This corpse has been reanimated with cybernetic technologies.";
				E.SetParameter("Postfix", E.GetStringParameter("Postfix") + str);
			}
			else if ((E.ID == "GetDisplayName" || E.ID == "GetShortDisplayName") && (!ParentObject.Understood() || !ParentObject.HasProperName))
			{
				E.GetParameter<StringBuilder>("Prefix").Append("awoken ");
			}
			return base.FireEvent(E);
		}
	}
}
