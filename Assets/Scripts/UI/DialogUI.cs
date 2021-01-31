using System.Collections;
using Settings;
using TMPro;
using UnityEngine;

namespace UI
{
	public class DialogUI : MonoBehaviour
	{
		private static DialogUI _instance;

		public RectTransform mainPanel;
		public RectTransform avatar;
		public TextMeshProUGUI titleLabel;
		public TextMeshProUGUI textLabel;
		public RectTransform footer;

		private bool anyKey;
		private GameObject currentAvatar;

		private void Start()
		{
			_instance = this;
			mainPanel.gameObject.SetActive(false);
			foreach (Transform child in avatar) Destroy(child.gameObject);
		}

		private void Update()
		{
			anyKey = Input.anyKey;
		}

		public static IEnumerator Show(PirateSettings pirate, string text, bool finalLine = false)
		{
			return _instance.ShowFlow(pirate, text, finalLine);
		}

		private IEnumerator ShowFlow(PirateSettings pirate, string text, bool finalLine = false)
		{
			mainPanel.gameObject.SetActive(true);
			if (pirate != null && pirate.prefab != currentAvatar)
			{
				if (currentAvatar != null) Destroy(currentAvatar);
				if (pirate.prefab != null)
				{
					currentAvatar = Instantiate(pirate.prefab, avatar);
					currentAvatar.transform.localPosition = pirate.avatarTransform.position;
					currentAvatar.transform.localRotation = Quaternion.Euler(pirate.avatarTransform.rotation);
					if (pirate.avatarTransform.scale != Vector3.zero)
						currentAvatar.transform.localScale = pirate.avatarTransform.scale;
				}
			}
			else
			{
				Destroy(currentAvatar);
			}

			titleLabel.text = pirate != null ? pirate.title : "";
			textLabel.text = text;
			footer.gameObject.SetActive(false);
			if (finalLine)
			{
				while (true) yield return null;
			}

			yield return new WaitForSeconds(0.5f);

			footer.gameObject.SetActive(true);
			yield return new WaitUntil(() => anyKey);
		}

		public static void Hide()
		{
			_instance.HideImpl();
		}

		private void HideImpl()
		{
			mainPanel.gameObject.SetActive(false);
		}
	}
}