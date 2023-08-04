using UnityEngine;
using UnityEngine.UI;

namespace Sound
{
	public class ChangeVolumeSlider : MonoBehaviour
	{
		private enum VolumeType
		{
			Sounds,
			Music
		}

		[SerializeField] private VolumeType _volumeType;
		[SerializeField] private AudioClip _testVolumeClip;
		[SerializeField] private Slider _slider;

		private void OnEnable()
		{
			switch (_volumeType)
			{
				case VolumeType.Music:
					_slider.value = SaveSoundManagerVolume.MusicVolume;
					break;
				case VolumeType.Sounds:
					_slider.value = SaveSoundManagerVolume.SoundVolume;
					break;
			}

			_slider.onValueChanged.AddListener(SetVolume);
		}

		public void PlayTestVolumeClip()
		{
			if (_volumeType == VolumeType.Music) return;
			
			if (_testVolumeClip != null)
			{
				SoundManager.Instance.Play(_testVolumeClip);
			}
		}

		private void SetVolume(float value)
		{
			switch (_volumeType)
			{
				case VolumeType.Sounds:
					SaveSoundManagerVolume.SoundVolume = value;
					SoundManager.Instance.SetEffectVolume(value);
					break;
				case VolumeType.Music:
					SaveSoundManagerVolume.MusicVolume = value;
					SoundManager.Instance.SetMusicVolume(value);
					break;
			}
		}

		private void OnDisable()
		{
			_slider.onValueChanged.RemoveListener(SetVolume);
		}
	}
}