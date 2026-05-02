using System.Reflection;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace FunnierDoom.FunnierDoomCode;

[HarmonyPatch(typeof(NInputManager), nameof(NInputManager._UnhandledKeyInput))]
public static class NInputManagerHotkeyPatch
{
    private static readonly string ModPath =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        ?? throw new Exception("Could not find mod path.");

    private static readonly string SoundDirectoryPath =
        Path.Combine(ModPath, "FunnyDoomSounds");
    
    static void Prefix(InputEvent inputEvent)
    {
        if (inputEvent is not InputEventKey keyEvent)
            return;

        if (!keyEvent.Pressed || keyEvent.Echo)
            return;

        GD.Print($"[FunnierDoom] Key pressed: {keyEvent.Keycode}");

        if (keyEvent.Keycode == Key.F8)
        {
            GD.Print("[FunnierDoom] F8 pressed!");
            
            string soundPath = Path.Combine(SoundDirectoryPath, "Prowler Sound.mp3");

            if (File.Exists(soundPath))
            {
                GD.Print("File exists!");
                GD.Print(soundPath);
                
                NDoomVfxPlayVfxPatch.PlayCustomSound();
                
                //PlayCustomSound();
                
                //SfxCmd.Play("event:/sfx/characters/necrobinder/necrobinder_doom_kill");
                //SfxCmd.Play(soundPath);
            }
            else if (!File.Exists(soundPath))
            {
                GD.Print("File doesn't exist!");
                GD.Print(soundPath);
            }
        }
    }
    
    // public static void PlayCustomSound()
    // {
    //     string modPath = Path.GetDirectoryName(
    //         Assembly.GetExecutingAssembly().Location
    //     ) ?? throw new Exception("Could not find mod path.");
    //
    //     string soundPath = Path.Combine(
    //         modPath,
    //         "FunnyDoomSounds",
    //         "Prowler Sound.mp3"
    //     );
    //
    //     GD.Print("[FunnierDoom] Sound path: " + soundPath);
    //     GD.Print("[FunnierDoom] File exists: " + File.Exists(soundPath));
    //
    //     if (!File.Exists(soundPath))
    //         return;
    //
    //     AudioStreamMP3 stream = AudioStreamMP3.LoadFromFile(soundPath);
    //
    //     AudioStreamPlayer player = new AudioStreamPlayer();
    //     player.Stream = stream;
    //     player.VolumeDb = 0;
    //
    //     SceneTree tree = (SceneTree)Engine.GetMainLoop();
    //     tree.Root.AddChild(player);
    //
    //     player.Finished += () =>
    //     {
    //         player.QueueFree();
    //     };
    //
    //     player.Play();
    // }
    
}