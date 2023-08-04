using System;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class PlayClipOnClick : MonoBehaviour
	{
		[SerializeField] private AudioClip _clip;
		private void Awake()
		{
			var button = GetComponent<Button>();
			button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			SoundManager.Instance.Play(_clip);
		}
	}
}