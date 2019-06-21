using System;
using System.Text;

namespace XRL.World.Parts
{
	[Serializable]
	public class acegiak_ModPlated : IModification
	{

		public acegiak_ModPlated()
		{
		}

		public acegiak_ModPlated(int Tier)
			: base(Tier)
		{
		}

		public override void Configure()
		{
			WorksOnSelf = true;
		}

		public override void ApplyModification(GameObject Object)
		{
                Object.Statistics["Toughness"].BaseValue += 4;
                Object.Statistics["AV"].BaseValue += 3;
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
				string str = "\n&Kp&ylate&Kd&C: This robot has been covered in heavy plating, increasing its toughness and armor value.";
				E.SetParameter("Postfix", E.GetStringParameter("Postfix") + str);
			}
			else if ((E.ID == "GetDisplayName" || E.ID == "GetShortDisplayName") && (!ParentObject.Understood() || !ParentObject.HasProperName))
			{
				E.GetParameter<StringBuilder>("Prefix").Append("&Kp&ylate&Kd&C ");
			}
			return base.FireEvent(E);
		}
	}
}
