using System;
using Dalamud.Game;
using Dalamud.Logging;

namespace XIVSlothCombo.Core
{
    /// <summary> Plugin address resolver. </summary>
    internal class PluginAddressResolver : BaseAddressResolver
    {
        /// <summary> Gets the address of the member ComboTimer. </summary>
        public IntPtr ComboTimer { get; private set; }

        /// <summary> Gets the address of the member LastComboMove. </summary>
        public IntPtr LastComboMove => ComboTimer + 0x4;

        /// <summary> Gets the address of fpGetAdjustedActionId. </summary>
        public IntPtr GetAdjustedActionId { get; private set; }

        /// <summary> Gets the address of fpIsIconReplacable. </summary>
        public IntPtr IsActionIdReplaceable { get; private set; }

        /// <inheritdoc/>
        protected override void Setup64Bit(SigScanner scanner)
        {
            ComboTimer = scanner.GetStaticAddressFromSig("F3 0F 11 05 ?? ?? ?? ?? F3 0F 10 45 ?? E8");

            GetAdjustedActionId = scanner.ScanText("E8 ?? ?? ?? ?? 8B F8 3B DF");  // Client::Game::ActionManager.GetAdjustedActionId

            IsActionIdReplaceable = scanner.ScanText("81 F9 ?? ?? ?? ?? 7F 35");

            PluginLog.Verbose("===== X I V S L O T H C O M B O =====");
            PluginLog.Verbose($"{nameof(GetAdjustedActionId)}   0x{GetAdjustedActionId:X}");
            PluginLog.Verbose($"{nameof(IsActionIdReplaceable)} 0x{IsActionIdReplaceable:X}");
            PluginLog.Verbose($"{nameof(ComboTimer)}            0x{ComboTimer:X}");
            PluginLog.Verbose($"{nameof(LastComboMove)}         0x{LastComboMove:X}");
        }
    }
}
