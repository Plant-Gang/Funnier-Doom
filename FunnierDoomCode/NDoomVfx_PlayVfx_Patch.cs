using Godot;

namespace FunnierDoom.FunnierDoomCode;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;

using HarmonyLib;
using System.Reflection;

[HarmonyPatch]
public static class NDoomVfxPlayVfxPatch
{
    /// Automatically discerns the relative mod path
    private static readonly string ModPath =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
        ?? throw new Exception("Could not find mod path.");

    /// Automatically Discerns the relative "FunnyDoomSounds" folder
    private static readonly string SfxAssetsPath =
        Path.Combine(ModPath, "FunnyDoomSounds");

    /// Automatically discerns how many SFX files are in the "FunnyDoomSounds" folder
    private static readonly int FunnySoundsAmount = Directory.GetFiles(SfxAssetsPath).Length;
    
    /**
     * Attaches the patch / prefix to the target method "PlayVFX"
     */
    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(DoomPower), "PlayVfx", new[] { typeof(Creature) });
    }

    /**
     * Calls before "PlayVfx" in the NDoomVfx class is played.
     * This should play a sound at the same time as the doom death animation is triggered.
     */
    public static void Prefix(Creature creature)
    {
        PlayCustomSound();
    }

    /**
     * Plays a random custom sound from the "FunnyDoomSounds" folder in the mod directory
     */
    public static void PlayCustomSound()
    {
        // Check if any funny sounds exist
        if (FunnySoundsAmount <= 0) return;
        
        // Start to randomly choose a sound
        Random random = new Random();
        int funnySoundIndex = random.Next(0, FunnySoundsAmount);
            
        // Assign the random sound
        string funnySfxName = Directory.GetFiles(SfxAssetsPath).ElementAt(funnySoundIndex);

        string randomFunnySfxPath = Path.Combine(SfxAssetsPath, funnySfxName);
        
        
        // Playing the sound
        AudioStreamMP3 stream = AudioStreamMP3.LoadFromFile(randomFunnySfxPath);
    
        AudioStreamPlayer player = new AudioStreamPlayer();
        player.Stream = stream;
        // -10 DB by default because files are usually very loud, also it'd be weird to have
        // a slider go from -40 to 50.
        player.VolumeDb = (float) FunnierDoomModConfig.VolumeSlider - 40;
    
        SceneTree tree = (SceneTree)Engine.GetMainLoop();
        tree.Root.AddChild(player);
    
        player.Finished += () =>
        {
            player.QueueFree();
        };
            
        player.Play();
    }
}