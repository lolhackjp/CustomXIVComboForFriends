﻿using XIVSlothCombo.Data;
using XIVSlothCombo.Services;

namespace XIVSlothCombo.CustomComboNS.Functions
{
    internal abstract partial class CustomComboFunctions
    {
        /// <summary> Gets the cooldown data for an action. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> Cooldown data. </returns>
        public static CooldownData GetCooldown(uint actionID) => Service.ComboCache.GetCooldown(actionID);

        /// <summary> Gets the cooldown total remaining time. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> Total remaining time of the cooldown. </returns>
        public static float GetCooldownRemainingTime(uint actionID) => Service.ComboCache.GetCooldown(actionID).CooldownRemaining;

        /// <summary> Gets the cooldown remaining time for the next charge. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> Remaining time for the next charge of the cooldown. </returns>
        public static float GetCooldownChargeRemainingTime(uint actionID) => Service.ComboCache.GetCooldown(actionID).ChargeCooldownRemaining;

        /// <summary> Gets a value indicating whether an action is on cooldown. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> True or false. </returns>
        public static bool IsOnCooldown(uint actionID) => GetCooldown(actionID).IsCooldown;

        /// <summary> Gets a value indicating whether an action is off cooldown. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> True or false. </returns>
        public static bool IsOffCooldown(uint actionID) => !GetCooldown(actionID).IsCooldown;

        /// <summary> Check if the Cooldown was just used. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> True or false. </returns>
        public static bool JustUsed(uint actionID) => IsOnCooldown(actionID) && GetCooldownRemainingTime(actionID) > (GetCooldown(actionID).CooldownTotal - 3);

        /// <summary> Gets a value indicating whether an action has any available charges. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> True or false. </returns>
        public static bool HasCharges(uint actionID) => GetCooldown(actionID).RemainingCharges > 0;

        /// <summary> Get the current number of charges remaining for an action. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> Number of charges. </returns>
        public static ushort GetRemainingCharges(uint actionID) => GetCooldown(actionID).RemainingCharges;

        /// <summary> Get the maximum number of charges for an action. </summary>
        /// <param name="actionID"> Action ID to check. </param>
        /// <returns> Number of charges. </returns>
        public static ushort GetMaxCharges(uint actionID) => GetCooldown(actionID).MaxCharges;
    }
}
