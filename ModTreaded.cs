using System;
using System.Text;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ModTreaded : IModification
	{

		public acegiak_ModTreaded()
		{
		}

		public acegiak_ModTreaded(int Tier)
			: base(Tier)
		{
		}

		public override void Configure()
		{
			WorksOnSelf = true;
		}

		public override void ApplyModification(GameObject Object)
		{
			if(Object == null){
				return;
			}
                Body partBody = Object.GetPart<Body>();
                if(partBody !=null){
                    BodyPart body2 = partBody.GetBody();
					if(body2 == null){
						return;
					}
                    body2.AddPart("Tread", 2);
                    body2.AddPart("Tread", 1);
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
			Object.RegisterPartEvent(this, "GetMaxWeight");
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{
			if (E.ID == "GetShortDescription")
			{
				string str = "\n&CTreaded: This robot has been equiped with treads allowing them to carry heavy loads.";
				E.SetParameter("Postfix", E.GetStringParameter("Postfix") + str);
			}
			else if ((E.ID == "GetDisplayName" || E.ID == "GetShortDisplayName") && (!ParentObject.Understood() || !ParentObject.HasProperName))
			{
				E.GetParameter<StringBuilder>("Prefix").Append("treaded ");
			}
            if (E.ID == "GetMaxWeight")
			{
				E.AddParameter("Weight", (int)Math.Floor((double)(int)E.GetParameter("Weight") * 100));
				return true;
			}
			return base.FireEvent(E);
		}
		public override bool ModificationApplicable(GameObject Object)
		{
			if(Object.GetPart<acegiak_ModTreaded>() != null){
				return false;
			}
			return true;
		}
	}
}
