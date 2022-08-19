using XIVSlothCombo.CustomComboNS;

namespace XIVSlothCombo.Combos.PvP
{
    internal static class DRGPVP
    {
        public const byte ClassID = 4;
        public const byte JobID = 22;
        internal const uint
            RaidenThrust = 29486,
            FangAndClaw = 29487,
            WheelingThrust = 29488,
            ChaoticSpring = 29490,
            Geirskogui = 29491,
            Nastroid = 29492,
            HighJump = 29493,
            WyrmwinThrust = 29495,
            HorridRoat = 29496,
            HeavensThrust = 29489;


        internal class Buffs
        {
            internal const ushort
                Heavensent = 3176,
                LifeOfTheDragon = 3177,
                FirstmindFocus = 3178;
        }

        internal class DRGPvP_BurstMode : CustomCombo
        {
            protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DRGPvP_BurstMode;

            protected override uint Invoke(uint actionID, uint lastComboActionID, float comboTime, byte level)
            {
                if (actionID is RaidenThrust or FangAndClaw or WheelingThrust)
                {
                    var canWeave = CanWeave(actionID);
                    if (IsOffCooldown(HighJump) && !InMeleeRange())
                    {
                        return HighJump;
                    }
                    if (canWeave && IsOffCooldown(HighJump) && InMeleeRange() && InCombat())
                    {
                        return HighJump;
                    }
                    if (HasEffect(Buffs.Heavensent))
                    {
                        return HeavensThrust;
                    }
                    if (HasEffect(Buffs.FirstmindFocus))
                    {
                        return WyrmwinThrust;
                    }
                    if (canWeave)
                    {
                        if (IsOffCooldown(Geirskogui) && InCombat())
                        {
                            return Geirskogui;
                        }
                        if (HasEffect(Buffs.LifeOfTheDragon))
                        {
                            return Nastroid;
                        }
                    }
                    if (IsOffCooldown(ChaoticSpring) && InCombat())
                    {
                        return ChaoticSpring;
                    }


                }
                return actionID;
            }
        }
    }
}