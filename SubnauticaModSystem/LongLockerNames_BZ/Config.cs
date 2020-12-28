//Special Thanks to Metious for this

using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace LongLockerNames_BZ
{
	[Menu("Long Locker Names and Color Picker (Game Restart required, Warning Text can get cutting off !)", LoadOn = MenuAttribute.LoadEvents.MenuOpened | MenuAttribute.LoadEvents.MenuRegistered)]
	public class Config : ConfigFile
	{
		public Config() : base("config") { }
		[Slider("Small Locker Text Limit", 1, 500, DefaultValue = 60)]
		public int SmallLockerTextLimit = 60;
		[Slider("Sign Text Limit", 1, 500, DefaultValue = 100)]
		public int SignTextLimit = 100;
		[Toggle("Color Picker On Lockers")]
		public bool ColorPickerOnLockers = true;
		[Toggle("Extra Colors On Lockers")]
		public bool ExtraColorsOnLockers = true;
		[Toggle("Color Picker On Signs")]
		public bool ColorPickerOnSigns = true;
		[Toggle("Extra Colors On Signs")]
		public bool ExtraColorsOnSigns = true;
	}
}