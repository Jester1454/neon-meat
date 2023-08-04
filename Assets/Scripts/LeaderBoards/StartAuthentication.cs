using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace LeaderBoards
{
	public class StartAuthentication : MonoBehaviour
	{
		[SerializeField] private GameObject _internetNotAvailable;
		public static bool IsAuthorized;
		
		async void Awake()
		{
			_internetNotAvailable.SetActive(false);

			if (IsAuthorized) return;
			
			await UnityServices.InitializeAsync();
			await SignInAnonymously();
		}

		async Task SignInAnonymously()
		{
			AuthenticationService.Instance.SignedIn += () =>
			{
				Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
				IsAuthorized = true;
			};
			AuthenticationService.Instance.SignInFailed += s =>
			{
				// Take some action here...
				Debug.LogError(s);
				_internetNotAvailable.SetActive(true);
			};
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}
	}
}