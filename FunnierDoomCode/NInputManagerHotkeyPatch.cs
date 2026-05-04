using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace FunnierDoom.FunnierDoomCode;

[HarmonyPatch(typeof(NInputManager), nameof(NInputManager._UnhandledKeyInput))]
public static class NInputManagerHotkeyPatch
{
    /**
     * Overrides MegaCrit's input manager to read button presses in real-time
     *
     * @inputEvent the input button
     */
    static void Prefix(InputEvent inputEvent)
    {
        if (inputEvent is not InputEventKey keyEvent)
            return;

        if (!keyEvent.Pressed || keyEvent.Echo)
            return;

        // The FunnyDoomSounds will be played and can be tested at will by pressing F8
        if (keyEvent.Keycode == Key.F8)
        {
            NDoomVfxPlayVfxPatch.PlayCustomSound();
        }
    }
}