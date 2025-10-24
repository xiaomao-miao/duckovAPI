using System;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001FC RID: 508
public class MultiInteractionMenuButton : MonoBehaviour
{
	// Token: 0x06000EDD RID: 3805 RVA: 0x0003B242 File Offset: 0x00039442
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x0003B260 File Offset: 0x00039460
	private void OnButtonClicked()
	{
		if (this.target == null)
		{
			return;
		}
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		main.Interact(this.target);
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x0003B286 File Offset: 0x00039486
	internal void Setup(InteractableBase target)
	{
		base.gameObject.SetActive(true);
		this.target = target;
		this.text.text = target.InteractName;
		this.fadeGroup.SkipHide();
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x0003B2B7 File Offset: 0x000394B7
	internal void Show()
	{
		this.fadeGroup.Show();
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x0003B2C4 File Offset: 0x000394C4
	internal void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x04000C41 RID: 3137
	[SerializeField]
	private Button button;

	// Token: 0x04000C42 RID: 3138
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04000C43 RID: 3139
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000C44 RID: 3140
	private InteractableBase target;
}
