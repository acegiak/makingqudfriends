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
	public class acegiak_PocketFriend : IPart
	{

		public acegiak_PocketFriend()
		{
			base.Name = "acegiak_PocketFriend";
			//DisplayName = "acegiak_PocketFriend";
		}

		public override void Register(GameObject Object)
		{
			Object.RegisterPartEvent(this, "GetInventoryActions");
			Object.RegisterPartEvent(this, "PowerSwitchActivate");
			Object.RegisterPartEvent(this, "CommandTakeObject");
            
			base.Register(Object);
		}

		public override bool FireEvent(Event E)
		{

            if (E.ID == "EnterCell")
			{
				if (StartActive && !IsActive())
				{
					ActivateForceEmitter();
				}
				ParentObject.UnregisterPartEvent(this, "EnterCell");
			}

            if (E.ID == "PowerSwitchActivate")
            {
  
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
                    XRLCore.Core.Game.ActionManager.AddActiveObject(ParentObject);

            }


			if (E.ID == "GetInventoryActions")
			{
                if(ParentObject.GetPart<Physics>() == null){
                    return false;
                }

				EventParameterGetInventoryActions eventParameterGetInventoryActions = E.GetParameter("Actions") as EventParameterGetInventoryActions;

                if (ParentObject.GetPart<Physics>().Equipped != null)
                {
                    if (!HasPropertyOrTag("NoRemoveOptionInInventory"))
                    {
                        eventParameterGetInventoryActions.AddAction("Remove", 'r',  true, "&Wr&yemove", "InvCommandUnequipObject", 10);
                    }
                }
                else
                {
                    bool flag2 = true;
                    if (ParentObject.GetPart<Physics>().InInventory == null || !ParentObject.GetPart<Physics>().InInventory.IsPlayer())
                    {
                        if (ParentObject.IsTakeable())
                        {
                            eventParameterGetInventoryActions.AddAction("Get", 'g',  true, "&Wg&yet", "CommandTakeObject", 30);
                        }
                        else
                        {
                            flag2 = false;
                        }
                    }
                    else if (!ParentObject.HasTagOrProperty("CannotDrop"))
                    {
                        eventParameterGetInventoryActions.AddAction("Drop", 'd',  true, "&Wd&yrop", "CommandDropObject");
                    }
                    if (flag2 && !ParentObject.HasTagOrProperty("CannotEquip"))
                    {
                        eventParameterGetInventoryActions.AddAction("AutoEquip", 'e',  true, "&We&yquip (auto)", "CommandAutoEquipObject", 10);
                        eventParameterGetInventoryActions.AddAction("DoEquip", 'E',  true, "&WE&yquip (manual)", "CommandEquipObject");
                    }
                }
				
				if (ParentObject.GetPart<Physics>().IsAflame())
				{
					eventParameterGetInventoryActions.AddAction("Firefight", 'f',  false, "&Wf&yight fire", "CommandFightFire", 40);
				}
			}
            
			return base.FireEvent(E);
		}
	}
}
