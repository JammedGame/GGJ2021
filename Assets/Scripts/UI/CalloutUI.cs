using System.Collections;
using TMPro;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UI
{
	public class CalloutUI : MonoBehaviour
	{
		private static CalloutUI _instance;

		public RectTransform calloutPanel;
		public TextMeshProUGUI nameLabel;
		public TextMeshProUGUI titleLabel;

		private void Start()
		{
			_instance = this;
		}

		public static IEnumerator Show(string pirateName, string pirateTitle)
		{
			return _instance.ShowFlow(pirateName, pirateTitle);
		}

		private IEnumerator ShowFlow(string pirateName, string pirateTitle)
		{
			nameLabel.text = pirateName;
			titleLabel.text = pirateTitle;

			var pos1 = -9999 * Vector3.right;
			var pos2 = -25 * Vector3.right;
			var pos3 = 25 * Vector3.right;
			var pos4 = 9999 * Vector3.right;
			calloutPanel.localPosition = pos1;
			calloutPanel.gameObject.SetActive(true);

			const float panTime = 1f;
			const float waitTime = 5f;
			var progress = 0f;
			while (progress < 1f)
			{
				progress += Time.deltaTime / panTime;
				calloutPanel.localPosition = Vector3.Lerp(pos1, pos2, progress);
				yield return null;
			}

			progress = 0f;
			while (progress < 1f)
			{
				progress += Time.deltaTime / waitTime;
				calloutPanel.localPosition = Vector3.Lerp(pos2, pos3, progress);
				yield return null;
			}

			progress = 0f;
			while (progress < 1f)
			{
				progress += Time.deltaTime / panTime;
				calloutPanel.localPosition = Vector3.Lerp(pos3, pos4, progress);
				yield return null;
			}

			calloutPanel.gameObject.SetActive(false);
		}

#if UNITY_EDITOR
		[MenuItem("Test/Callout")]
		private static void Test()
		{
			_instance.StartCoroutine(Show("Test", "Test"));
		}
#endif
	}
}