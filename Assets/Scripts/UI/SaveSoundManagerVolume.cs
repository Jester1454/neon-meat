using UnityEngine;

namespace Sound
{
	public static class SaveSoundManagerVolume
	{
		private static string SoundsVolumeKey = "sounds_volume";
		private static string MusicVolumeKey = "music_volume";

		public static float SoundVolume
		{
			get => PlayerPrefs.GetFloat(SoundsVolumeKey, 0.5f);
			set => PlayerPrefs.SetFloat(SoundsVolumeKey, value);
		}	
		
		public static float MusicVolume
		{
			get => PlayerPrefs.GetFloat(MusicVolumeKey, 0.3f);
			set => PlayerPrefs.SetFloat(MusicVolumeKey, value);
		}
	}
}