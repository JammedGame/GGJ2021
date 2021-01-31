using UnityEngine;

namespace Settings
{
	[CreateAssetMenu]
	public class PirateSettings : ScriptableObject
	{
		public string pirateName;
		public string title;
		public GameObject prefab;
	}
}