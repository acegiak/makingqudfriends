using System;
using System.Text;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ModHandy : IModification
	{

		public acegiak_ModHandy()
		{
		}

		public acegiak_ModHandy(int Tier)
			: base(Tier)
		{
		}

		public override void Configure()
		{
			WorksOnSelf = true;
		}

		public override void ApplyModification(GameObject Object)
		{
                Body partBody = Object.GetPart<Body>();
                if(partBody !=null){
                    BodyPart body2 = partBody.GetBody();
                    BodyPart firstAttachedPart = body2.GetFirstAttachedPart("Back", 0, partBody, true);
                    if(firstAttachedPart != null){

                        body2.AddPartAt(firstAttachedPart,"Arm", 2).AddPart("Hand", 2, "MetalFist", "Hands");
                        body2.AddPartAt(firstAttachedPart,"Arm", 1).AddPart("Hand", 1, "MetalFist", "Hands");
                        body2.AddPartAt(firstAttachedPart,"Missile Weapon", 2);
                        body2.AddPartAt(firstAttachedPart,"Missile Weapon", 1);
				        body2.AddPartAt(firstAttachedPart,"Thrown Weapon");
                        body2.AddPartAt(firstAttachedPart,"Hands", 0, null, null, "Hands");
                    }else{

                        body2.AddPart("Arm", 2).AddPart("Hand", 2, "MetalFist", "Hands");
                        body2.AddPart("Arm", 1).AddPart("Hand", 1, "MetalFist", "Hands");
                        body2.AddPart("Missile Weapon", 2);
                        body2.AddPart("Missile Weapon", 1);
				        body2.AddPart("Thrown Weapon");
                        body2.AddPart("Hands", 0, null, null, "Hands");
                    }
                }
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
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "GetShortDescription")
			{
				string str = "\n&CHandy: This robot has been equiped with arms and hands for holding and manipulating things.";
				E.SetParameter("Postfix", E.GetStringParameter("Postfix") + str);
			}
			else if ((E.ID == "GetDisplayName" || E.ID == "GetShortDisplayName") && (!ParentObject.Understood() || !ParentObject.HasProperName))
			{
				E.GetParameter<StringBuilder>("Prefix").Append("handy ");
			}
			return base.FireEvent(E);
		}

		public override bool ModificationApplicable(GameObject Object)
		{
			if(Object.GetPart<acegiak_ModHandy>() != null){
				return false;
			}
			return true;
		}
	}
}
