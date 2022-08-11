﻿using Dalamud.Interface.Colors;
using Dalamud.Utility;
using ImGuiNET;
using System.Linq;
using XIVSlothCombo.Attributes;
using XIVSlothCombo.Combos;
using XIVSlothCombo.Core;
using XIVSlothCombo.Data;
using XIVSlothCombo.Services;

namespace XIVSlothCombo.Window.Functions
{
    internal class Presets : ConfigWindow
    {
        internal static void DrawPreset(CustomComboPreset preset, CustomComboInfoAttribute info, ref int i)
        {
            var enabled = Service.Configuration.IsEnabled(preset);
            var secret = PluginConfiguration.IsSecret(preset);
            var conflicts = Service.Configuration.GetConflicts(preset);
            var parent = PluginConfiguration.GetParent(preset);
            var blueAttr = preset.GetAttribute<BlueInactiveAttribute>();

            ImGui.PushItemWidth(200);

            if (ImGui.Checkbox($"{info.FancyName}###{i}", ref enabled))
            {
                if (enabled)
                {
                    EnableParentPresets(preset);
                    Service.Configuration.EnabledActions.Add(preset);
                    foreach (var conflict in conflicts)
                    {
                        Service.Configuration.EnabledActions.Remove(conflict);
                    }
                }

                else
                {
                    Service.Configuration.EnabledActions.Remove(preset);
                }

                Service.Configuration.Save();
            }

            ImGui.PopItemWidth();
            ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudGrey);

            if (preset.GetAttribute<ReplaceSkillAttribute>() != null)
            {
                string skills = string.Join(", ", preset.GetAttribute<ReplaceSkillAttribute>().ActionNames);

                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.TextUnformatted($"Replaces: {skills}");
                    ImGui.EndTooltip();
                }
            }

            ImGui.Text($"#{i}: ");
            var length = ImGui.CalcTextSize($"#{i}: ");
            ImGui.SameLine();
            ImGui.PushItemWidth(length.Length());
            ImGui.TextWrapped($"{info.Description}");

            if (preset.GetAttribute<HoverInfoAttribute>() != null)
            {
                if (ImGui.IsItemHovered())
                {
                    ImGui.BeginTooltip();
                    ImGui.TextUnformatted(preset.GetAttribute<HoverInfoAttribute>().HoverText);
                    ImGui.EndTooltip();
                }
            }


            ImGui.PopStyleColor();
            ImGui.Spacing();

            UserConfigItems.Draw(preset, enabled);

            if (conflicts.Length > 0)
            {
                var conflictText = conflicts.Select(conflict =>
                {
                    var conflictInfo = conflict.GetAttribute<CustomComboInfoAttribute>();

                    return $"\n - {conflictInfo.FancyName}";


                }).Aggregate((t1, t2) => $"{t1}{t2}");

                if (conflictText.Length > 0)
                {
                    ImGui.TextColored(ImGuiColors.DalamudRed, $"Conflicts with: {conflictText}");
                    ImGui.Spacing();
                }
            }

            if (blueAttr != null)
            {
                if (blueAttr.Actions.Count > 0)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.DalamudOrange);
                    ImGui.Text($"Missing active spells: {string.Join(", ", blueAttr.Actions.Select(x => ActionWatching.GetActionName(x)))}");
                    ImGui.PopStyleColor();
                }

                else
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, ImGuiColors.HealerGreen);
                    ImGui.Text($"All required spells active!");
                    ImGui.PopStyleColor();
                }
            }

            i++;

            var hideChildren = Service.Configuration.HideChildren;
            var children = presetChildren[preset];

            if (children.Length > 0)
            {
                if (enabled || !hideChildren)
                {
                    ImGui.Indent();

                    foreach (var (childPreset, childInfo) in children)
                    {
                        if (Service.Configuration.HideConflictedCombos)
                        {
                            var conflictOriginals = Service.Configuration.GetConflicts(childPreset);    // Presets that are contained within a ConflictedAttribute
                            var conflictsSource = Service.Configuration.GetAllConflicts();              // Presets with the ConflictedAttribute

                            if (!conflictsSource.Where(x => x == childPreset || x == preset).Any() || conflictOriginals.Length == 0)
                            {
                                DrawPreset(childPreset, childInfo, ref i);
                                continue;
                            }

                            if (conflictOriginals.Any(x => Service.Configuration.IsEnabled(x)))
                            {
                                Service.Configuration.EnabledActions.Remove(childPreset);
                                Service.Configuration.Save();
                            }

                            else
                            {
                                DrawPreset(childPreset, childInfo, ref i);
                                continue;
                            }
                        }

                        else
                        {
                            DrawPreset(childPreset, childInfo, ref i);
                        }
                    }

                    ImGui.Unindent();
                }   
                else
                {
                    i += AllChildren(presetChildren[preset]);

                }
            }
        }

        internal static int AllChildren((CustomComboPreset Preset, CustomComboInfoAttribute Info)[] children)
        {
            var output = 0;

            foreach (var (Preset, Info) in children)
            {
                output++;
                output += AllChildren(presetChildren[Preset]);
            }

            return output;
        }

        /// <summary> Iterates up a preset's parent tree, enabling each of them. </summary>
        /// <param name="preset"> Combo preset to enabled. </param>
        private static void EnableParentPresets(CustomComboPreset preset)
        {
            var parentMaybe = PluginConfiguration.GetParent(preset);

            while (parentMaybe != null)
            {
                var parent = parentMaybe.Value;

                if (!Service.Configuration.EnabledActions.Contains(parent))
                {
                    Service.Configuration.EnabledActions.Add(parent);
                    foreach (var conflict in Service.Configuration.GetConflicts(parent))
                    {
                        Service.Configuration.EnabledActions.Remove(conflict);
                    }
                }

                parentMaybe = PluginConfiguration.GetParent(parent);
            }
        }
    }
}
