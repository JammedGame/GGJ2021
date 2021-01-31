using System;
using UnityEngine;

namespace Settings
{
	[CreateAssetMenu]
	public class PirateSettings : ScriptableObject
	{
		public string pirateName;
		public string title;
		public GameObject prefab;
		public PirateAvatarTransform avatarTransform;
	}

	[Serializable]
	public class PirateAvatarTransform
	{
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 scale;
	}
}