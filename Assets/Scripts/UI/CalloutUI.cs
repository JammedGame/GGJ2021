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
		public TextMeshProUGUI titleLabel;

		private void Start()
		{
			_instance = this;
		}

		public static void Show(string text)
		{
			_instance.titleLabel.text = text;
			_instance.StartCoroutine(_instance.ShowFlow());
		}

		private IEnumerator ShowFlow()
		{
			var pos1 = -9999 * Vector3.right;
			var pos2 = -50 * Vector3.right;
			var pos3 = 50 * Vector3.right;
			var pos4 = 9999 * Vector3.right;
			calloutPanel.localPosition = pos1;
			calloutPanel.gameObject.SetActive(true);

			const float panTime = 1f;
			const float waitTime = 4f;
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
		public static void Test()
		{
			Show("Test");
		}
#endif
	}
}