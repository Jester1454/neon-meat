using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Sound.Editor
{
	[CustomPropertyDrawer(typeof(MixerGroup))]
	public class MixerGroupPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			property.NextVisible(true);
			Assert.AreEqual("groupID", property.name);
			int value = property.intValue;
			var binding = AudioGroupBinding.Instance;
			var oldGroup = binding.ResolveMixerGroupID(value);
			var newGroup = (AudioMixerGroup) EditorGUI.ObjectField(position, "Output", oldGroup, typeof(AudioMixerGroup), false);
			if (newGroup != oldGroup)
			{
				property.intValue = binding.GetOrCreateMixerGroupID(newGroup);
			}
		}
	}
}