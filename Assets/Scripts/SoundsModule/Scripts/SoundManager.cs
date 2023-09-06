using UnityEngine;

namespace Sound
{
	public class SoundManager : MonoBehaviour
	{
		[SerializeField] private AudioSource _soundSource;
		[SerializeField] public MixerGroup _soundMixerGroup;
		[SerializeField] private AudioSource _musicSource;
		[SerializeField] public MixerGroup _musicMixerGroup;
		[SerializeField] private float _lowPitchRange = .95f;
		[SerializeField] private float _highPitchRange = 1.05f;
		[SerializeField] private bool _autoPlayMusic;
		[SerializeField] private AudioClip _defaultMusic;

		private static SoundManager _instance = null;

		public static SoundManager Instance
		{
			get
			{
				if (_instance == null)
				{
					var prefab = Resources.Load<GameObject>("SoundManger");
					// create the prefab in your scene
					var inScene = Instantiate<GameObject>(prefab);
					// try find the instance inside the prefab
					_instance = inScene.GetComponentInChildren<SoundManager>();
					// guess there isn't one, add one
					if (!_instance) _instance = inScene.AddComponent<SoundManager>();
					// mark root as DontDestroyOnLoad();
					DontDestroyOnLoad(_instance.transform.root.gameObject);
				}
				return _instance;
			}
		}

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
			}
			else if (_instance != this)
			{
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);
		}

		private void Start()
		{
			_soundMixerGroup.Resolve();
			_musicMixerGroup.Resolve();
			
			_soundSource.outputAudioMixerGroup = _soundMixerGroup.Group;
			_musicSource.outputAudioMixerGroup = _musicMixerGroup.Group;
			
			SetEffectVolume(SaveSoundManagerVolume.SoundVolume);
			SetMusicVolume(SaveSoundManagerVolume.MusicVolume);
			
			if (_autoPlayMusic)
			{
				PlayMusic(_defaultMusic, true);
			}
		}

		public void Play(AudioClip clip)
		{
			if (clip == null) return;
			
			_soundSource.PlayOneShot(clip);
		}

		public void PlayLoop(AudioClip clip)
		{
			if (clip == null) return;
			_soundSource.clip = clip;
			_soundSource.loop = true;
			_soundSource.Play();
		}

		public void StopLoop()
		{
			_soundSource.loop = false;
			_soundSource.Stop();
		}

		public void StopMusic()
		{
			_musicSource.Stop();
		}

		public void PlayMusic(AudioClip clip, bool isLoop)
		{
			if (clip == null) return;
			_musicSource.clip = clip;
			_musicSource.loop = isLoop;
			_musicSource.Play();
		}

		public void RandomSoundEffect(params AudioClip[] clips)
		{
			if (clips == null) return;

			int randomIndex = Random.Range(0, clips.Length);
			float randomPitch = Random.Range(_lowPitchRange, _highPitchRange);
			_soundSource.pitch = randomPitch;
			_soundSource.clip = clips[randomIndex];
			_soundSource.Play();
		}

		public void SetEffectVolume(float value)
		{
			_soundSource.outputAudioMixerGroup.audioMixer.SetFloat("SoundsVolume", GetVolume(value));
		}
		
		public void SetMusicVolume(float value)
		{
			_musicSource.outputAudioMixerGroup.audioMixer.SetFloat("MusicVolume", GetVolume(value));
		}

		private float GetVolume(float value)
		{
			return Mathf.Log10(value) * 20;
		}
	}
}