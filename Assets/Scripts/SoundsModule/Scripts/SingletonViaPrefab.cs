using UnityEngine;

namespace Sound
{
	public class SingletonViaPrefab : MonoBehaviour
	{
		// This is really the only blurb of code you need to implement a Unity singleton
		private static SingletonViaPrefab _Instance;
		public static SingletonViaPrefab Instance
		{
			get
			{
				if (!_Instance)
				{
					// NOTE: read docs to see directory requirements for Resources.Load!
					var prefab = Resources.Load<GameObject>("PathToYourSingletonViaPrefab");
					// create the prefab in your scene
					var inScene = Instantiate<GameObject>(prefab);
					// try find the instance inside the prefab
					_Instance = inScene.GetComponentInChildren<SingletonViaPrefab>();
					// guess there isn't one, add one
					if (!_Instance) _Instance = inScene.AddComponent<SingletonViaPrefab>();
					// mark root as DontDestroyOnLoad();
					DontDestroyOnLoad(_Instance.transform.root.gameObject);
				}
				return _Instance;
			}
		}
	}
}