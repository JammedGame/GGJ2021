using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
	[CreateAssetMenu]
	public class DialogSettings : ScriptableObject
	{
		public List<Dialog> dialogs;
	}

	[Serializable]
	public class Dialog
	{
		public bool isEpilogue;
		public PirateSettings triggerWhenFound;
		public List<DialogLine> lines;
	}

	[Serializable]
	public class DialogLine
	{
		public PirateSettings pirate;
		public string text;
		public DialogCondition condition;
	}

	[Serializable]
	public class DialogCondition
	{
		public PirateSettings pirate;
		public DialogConditionOperator op;
	}

	public enum DialogConditionOperator
	{
		NotFound,
		Found
	}
}