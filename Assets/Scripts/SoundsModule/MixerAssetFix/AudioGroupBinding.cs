using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound
{
	[CreateAssetMenu()]
	public class AudioGroupBinding : ScriptableObject
	{
		private static AudioGroupBinding instance;

		public static AudioGroupBinding Instance
		{
			get
			{
				if (instance == null)
				{
					instance = Resources.Load<AudioGroupBinding>("AudioGroupBinding");
				}

				return instance;
			}
		}

		public AudioMixerGroup[] GroupReferences;

		public AudioMixerGroup ResolveMixerGroupID(int id)
		{
			if ((uint) (id - 1) >= (uint) GroupReferences.Length) return null;
			return GroupReferences[id - 1];
		}

#if UNITY_EDITOR
		public int GetOrCreateMixerGroupID(AudioMixerGroup group)
		{
			if (group == null) return 0;

			for (int index = 0; index < GroupReferences.Length; index++)
			{
				if (GroupReferences[index] == group)
				{
					return index + 1;
				}
			}

			Array.Resize(ref GroupReferences, GroupReferences.Length + 1);
			GroupReferences[GroupReferences.Length - 1] = group;
			EditorUtility.SetDirty(this);
			return GroupReferences.Length;
		}
#endif
	}
}