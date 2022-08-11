using FFXIVClientStructs;

namespace XIVSlothCombo.CustomComboNS.Functions
{
    internal abstract partial class CustomComboFunctions
    {

        public unsafe void UseItem(uint itemId)
        {
            FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Instance()->UseAction(FFXIVClientStructs.FFXIV.Client.Game.ActionType.Item, itemId, 0xE0000000, 65535, 0, 0, null);
        }
        public unsafe void UseActionId(uint actionId)
        {
            FFXIVClientStructs.FFXIV.Client.Game.ActionManager.Instance()->UseAction(FFXIVClientStructs.FFXIV.Client.Game.ActionType.Ability, actionId, 0xE0000000, 65535, 0, 0, null);
        }
    }
    
}
