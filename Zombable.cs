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
		[NonSerialized]
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
            //IPart.AddPlayerMessage("Attempt zombify");
            Corpse CorpsePart = ParentObject.GetPart<Corpse>();
            if(CorpsePart == null){
                //IPart.AddPlayerMessage("No Corpse!");
                return;
            }

            Body part = ParentObject.GetPart<Body>();
            if(part == null){
                //IPart.AddPlayerMessage("No Body!");
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
                    gameObject = GameObject.createUnmodded(CorpsePart.BurntCorpseBlueprint);
                }
            }
            else if (ParentObject.pPhysics.LastDamagedByType == "Vaporized")
            {
                if (CorpsePart.VaporizedCorpseChance > 0 && (string.IsNullOrEmpty(CorpsePart.VaporizedCorpseRequiresBodyPart) || (part != null && part.GetFirstPart(CorpsePart.VaporizedCorpseRequiresBodyPart) != null)) && (CorpsePart.VaporizedCorpseChance >= 100 || Stat.Random(1, 100) <= CorpsePart.VaporizedCorpseChance))
                {
                    gameObject = GameObject.createUnmodded(CorpsePart.VaporizedCorpseBlueprint);
                }
            }
            else if (CorpsePart.CorpseChance > 0 && (string.IsNullOrEmpty(CorpsePart.CorpseRequiresBodyPart) || (part != null && part.GetFirstPart(CorpsePart.CorpseRequiresBodyPart) != null)) && (CorpsePart.CorpseChance >= 100 || Stat.Random(1, 100) <= CorpsePart.CorpseChance))
            {
                gameObject = GameObject.createUnmodded(CorpsePart.CorpseBlueprint);
            }
            if(gameObject == null){
                CorpsePart.CorpseChance = 0;
                CorpsePart.VaporizedCorpseChance = 0;
                CorpsePart.BurntCorpseChance = 0;
            }
            if(part != null && gameObject != null){
                acegiak_Zombable zombieparts = new acegiak_Zombable();
                zombieparts.Body = new Body();
                ParentObject.DeepCopyInventoryObjectMap = new Dictionary<GameObject, GameObject>();
                zombieparts.Body._Body = BodyPartCopy(part._Body,ParentObject,zombieparts.Body);
                if(zombieparts.Body._Body == null){
					return;
				}
                zombieparts.storedTile = ParentObject.pRender.Tile;
                gameObject.AddPart(zombieparts);
                CorpsePart.CorpseObject = gameObject;
                //IPart.AddPlayerMessage("Zombified!");
            }
            //IPart.AddPlayerMessage("Done!");

        }
    public static BodyPart BodyPartCopy(BodyPart origin,GameObject Parent, Body NewBody)
		{
			if(origin == null || NewBody == null || Parent == null){
				return null;
			}
			BodyPart bodyPart = new BodyPart(NewBody);
			bodyPart.Type = origin.Type;
			bodyPart.VariantType = origin.VariantType;
			bodyPart.Description = origin.Description;
			bodyPart.Name = origin.Name;
			bodyPart.SupportsDependent = origin.SupportsDependent;
			bodyPart.DependsOn = origin.DependsOn;
			bodyPart.RequiresType = origin.RequiresType;
			bodyPart.PreventRegenerationBecause = origin.PreventRegenerationBecause;
			bodyPart.Category = origin.Category;
			bodyPart.Laterality = origin.Laterality;
			bodyPart.RequiresLaterality = origin.RequiresLaterality;
			bodyPart.Mobility = origin.Mobility;
			bodyPart.Primary = origin.Primary;
			bodyPart.Native = origin.Native;
			bodyPart.Integral = origin.Integral;
			bodyPart.Mortal = origin.Mortal;
			bodyPart.Abstract = origin.Abstract;
			bodyPart.Extrinsic = origin.Extrinsic;
			bodyPart.Plural = origin.Plural;
			bodyPart.Position = origin.Position;
			bodyPart._ID = origin._ID;
			bodyPart.ParentBody = NewBody;
			// if (DefaultBehavior != null)
			// {
			// 	GameObject gameObject = DefaultBehavior.DeepCopy();
			// 	Parent.DeepCopyInventoryObjectMap.Add(DefaultBehavior, gameObject);
			// 	gameObject.pPhysics._Equipped = Parent;
			// 	bodyPart.DefaultBehavior = gameObject;
			// }
			// if (Equipped != null && !Parent.DeepCopyInventoryObjectMap.ContainsKey(Equipped))
			// {
			// 	GameObject gameObject2 = Equipped.DeepCopy();
			// 	Parent.DeepCopyInventoryObjectMap.Add(Equipped, gameObject2);
			// 	Body.DeepCopyEquipMap.Add(gameObject2, bodyPart);
			// }
			if(origin.Parts == null){
				return bodyPart;
			}
			if(bodyPart.Parts == null){
				bodyPart.Parts  = new List<BodyPart>(13);
			}
			foreach (BodyPart part in origin.Parts)
			{
				if (!part.Extrinsic)
				{
					BodyPart copy = BodyPartCopy(part,Parent, NewBody);
					if(copy != null){
						bodyPart.AddPart(copy);
					}
				}
			}
			return bodyPart;
		}


		public override void SaveData(SerializationWriter Writer)
		{
			if(Body._Body == null){
				Writer.Write(0);
			}else{
				Writer.Write(1);
				Body.SaveData(Writer);
			}
			
			base.SaveData(Writer);
		}

		public override void LoadData(SerializationReader Reader)
		{
			Body Body = new Body();
			int num = Reader.ReadInt32();
			if (num > 0)
			{
				Body.LoadData(Reader);
			}
			base.LoadData(Reader);
		}


    }
}
