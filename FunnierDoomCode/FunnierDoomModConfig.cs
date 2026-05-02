using BaseLib.Config;

namespace FunnierDoom.FunnierDoomCode;

[ConfigHoverTipsByDefault]
internal class FunnierDoomModConfig : SimpleModConfig
{
    [ConfigSection("Volume Settings")]
    
    [ConfigSlider(0, 50, 1)]
    public static double VolumeSlider { get; set; } = 25f;
}