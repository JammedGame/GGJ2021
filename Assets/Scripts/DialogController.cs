using System.Collections;
using System.Collections.Generic;
using Settings;
using UI;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class DialogController : MonoBehaviour
{
	private static DialogController _instance;

	public DialogSettings settings;

	private readonly List<PirateSettings> piratesToFind = new List<PirateSettings>();
	private readonly List<Dialog> remainingDialogs = new List<Dialog>();
	private Dialog dialogInProgress;

	private void Start()
	{
		_instance = this;
		piratesToFind.AddRange(settings.piratesToFind);
		remainingDialogs.AddRange(settings.dialogs);
		TryStartNextDialog();
	}

	public static void MarkPirateFound(PirateSettings pirate)
	{
		_instance.StartCoroutine(_instance.FindPirateFlow(pirate));
	}

	private bool IsPirateFound(PirateSettings pirate)
	{
		return !piratesToFind.Contains(pirate);
	}

	private IEnumerator FindPirateFlow(PirateSettings pirate)
	{
		piratesToFind.Remove(pirate);
		yield return CalloutUI.Show(pirate.pirateName, pirate.title);

		while (true)
		{
			var found = TryStartNextDialog();
			if (found == null) yield break;

			yield return found;
		}
	}

#if UNITY_EDITOR
	[MenuItem("Test/TryStartNextDialog")]
#endif
	private static Coroutine TryStartNextDialog()
	{
		if (_instance.dialogInProgress != null)
		{
			Debug.LogError("Dialog already in progress");
			return null;
		}

		var candidate = _instance.remainingDialogs.Find(d =>
			(d.triggerWhenFound == null || _instance.IsPirateFound(d.triggerWhenFound)) &&
			(!d.isEpilogue || _instance.piratesToFind.Count == 0));
		if (candidate == null)
		{
			Debug.Log("No candidates remaining");
			return null;
		}

		return _instance.StartCoroutine(_instance.DialogFlow(candidate));
	}

	private IEnumerator DialogFlow(Dialog dialog)
	{
		remainingDialogs.Remove(dialog);
		dialogInProgress = dialog;
		yield return new WaitForSeconds(0.5f);

		foreach (var line in dialog.lines)
		{
			var condition = line.condition;
			var conditionMet = condition.pirate == null ||
				condition.op == DialogConditionOperator.Found && IsPirateFound(condition.pirate) ||
				condition.op == DialogConditionOperator.NotFound && !IsPirateFound(condition.pirate);
			if (conditionMet) yield return DialogUI.Show(line.pirate, line.text);
		}

		DialogUI.Hide();
		dialogInProgress = null;
	}

#if UNITY_EDITOR
	[MenuItem("Test/FindAllPirates")]
	private static void FindAllPirates()
	{
		_instance.piratesToFind.Clear();
	}
#endif
}