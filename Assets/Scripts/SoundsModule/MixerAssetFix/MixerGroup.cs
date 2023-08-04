using System;
using UnityEngine.Audio;

namespace Sound
{
	[Serializable]
	public struct MixerGroup
	{
		public int groupID;

		public AudioMixerGroup Group { get; private set; }

		/// This must be called in Awake().
		public void Resolve()
		{
			Group = AudioGroupBinding.Instance.ResolveMixerGroupID(groupID);
		}

		public static implicit operator AudioMixerGroup(MixerGroup mixer)
		{
			return mixer.Group;
		}
	}
}