using FFXIVClientStructs;
using XIVSlothCombo.Data;

namespace XIVSlothCombo.CustomComboNS.Functions
{
    internal abstract partial class CustomComboFunctions
    {

        public unsafe void UseItem(uint itemId)
        {
            FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Instance()->UseAction(FFXIVClientStructs.FFXIV.Client.Game.ActionType.Item, itemId, 0xE0000000, 65535, 0, 0, null);
        }

        // Testing auto skills
        // Current issue that it only executes while a new target is selected.
        //
        public unsafe void UseActionId(uint actionID)
        {
            FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Instance()->UseAction(FFXIVClientStructs.FFXIV.Client.Game.ActionType.Spell, actionID, 0xE000_0000, 0, 0, 0, null);
        }


    }

}
