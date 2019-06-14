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
	public class acegiak_Zombable : IPart
	{


        // public BodyPart Part;
        public Body Body = new Body();

        public string storedTile;


		public acegiak_Zombable()
		{
			base.Name = "acegiak_Zombable";

            // Part = new BodyPart("Body", Body);
			//DisplayName = "acegiak_Zombable";
		}

		public override void Register(GameObject Object)
		{
            Object.RegisterPartEvent(this, "BeforeDie");
            Object.RegisterPartEvent(this, "Dismember");

			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{

			if (E.ID == "BeforeDie")
			{
                Zombablify();
			}
			return base.FireEvent(E);


        }

        public void Zombablify(){
            IPart.AddPlayerMessage("Attempt zombify");
            Corpse CorpsePart = ParentObject.GetPart<Corpse>();
            if(CorpsePart == null){
                IPart.AddPlayerMessage("No Corpse!");
                return;
            }

            Body part = ParentObject.GetPart<Body>();
            if(part == null){
                IPart.AddPlayerMessage("No Body!");
                return;
            }

            GameObject gameObject = null;

            if (CorpsePart.CorpseObject != null)
            {
                gameObject = CorpsePart.CorpseObject;
            }
            else if (ParentObject.pPhysics.LastDamagedByType == "Fire")
            {
                if (CorpsePart.BurntCorpseChance > 0 && (string.IsNullOrEmpty(CorpsePart.BurntCorpseRequiresBodyPart) || (part != null && part.GetFirstPart(CorpsePart.BurntCorpseRequiresBodyPart) != null)) && (CorpsePart.BurntCorpseChance >= 100 || Stat.Random(1, 100) <= CorpsePart.BurntCorpseChance))
                {
                    gameObject = GameObject.create(CorpsePart.BurntCorpseBlueprint);
                }
            }
            else if (ParentObject.pPhysics.LastDamagedByType == "Vaporized")
            {
                if (CorpsePart.VaporizedCorpseChance > 0 && (string.IsNullOrEmpty(CorpsePart.VaporizedCorpseRequiresBodyPart) || (part != null && part.GetFirstPart(CorpsePart.VaporizedCorpseRequiresBodyPart) != null)) && (CorpsePart.VaporizedCorpseChance >= 100 || Stat.Random(1, 100) <= CorpsePart.VaporizedCorpseChance))
                {
                    gameObject = GameObject.create(CorpsePart.VaporizedCorpseBlueprint);
                }
            }
            else if (CorpsePart.CorpseChance > 0 && (string.IsNullOrEmpty(CorpsePart.CorpseRequiresBodyPart) || (part != null && part.GetFirstPart(CorpsePart.CorpseRequiresBodyPart) != null)) && (CorpsePart.CorpseChance >= 100 || Stat.Random(1, 100) <= CorpsePart.CorpseChance))
            {
                gameObject = GameObject.create(CorpsePart.CorpseBlueprint);
            }
            if(part != null && gameObject != null){
                acegiak_Zombable zombieparts = new acegiak_Zombable();
                zombieparts.Body = (Body)Body.DeepCopy(ParentObject);
                zombieparts.storedTile = ParentObject.pRender.Tile;
                gameObject.AddPart(zombieparts);
                CorpsePart.CorpseObject = gameObject;
                IPart.AddPlayerMessage("Zombified!");
            }
            IPart.AddPlayerMessage("Done!");



        }
    }
}