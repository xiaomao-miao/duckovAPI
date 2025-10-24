using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fishing.UI
{
	// Token: 0x02000219 RID: 537
	public class ConfirmPanel : MonoBehaviour
	{
		// Token: 0x06001002 RID: 4098 RVA: 0x0003E824 File Offset: 0x0003CA24
		private void Awake()
		{
			this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
			this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
			this.itemDisplay.onPointerClick += this.OnItemDisplayClick;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0003E880 File Offset: 0x0003CA80
		private void OnItemDisplayClick(ItemDisplay display, PointerEventData data)
		{
			data.Use();
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0003E888 File Offset: 0x0003CA88
		private void OnContinueButtonClicked()
		{
			this.confirmed = true;
			this.continueFishing = true;
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0003E898 File Offset: 0x0003CA98
		private void OnQuitButtonClicked()
		{
			this.confirmed = true;
			this.continueFishing = false;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0003E8A8 File Offset: 0x0003CAA8
		internal UniTask DoConfirmDialogue(Item catchedItem, Action<bool> confirmCallback)
		{
			ConfirmPanel.<DoConfirmDialogue>d__13 <DoConfirmDialogue>d__;
			<DoConfirmDialogue>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoConfirmDialogue>d__.<>4__this = this;
			<DoConfirmDialogue>d__.catchedItem = catchedItem;
			<DoConfirmDialogue>d__.confirmCallback = confirmCallback;
			<DoConfirmDialogue>d__.<>1__state = -1;
			<DoConfirmDialogue>d__.<>t__builder.Start<ConfirmPanel.<DoConfirmDialogue>d__13>(ref <DoConfirmDialogue>d__);
			return <DoConfirmDialogue>d__.<>t__builder.Task;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0003E8FC File Offset: 0x0003CAFC
		private void Setup(Item item)
		{
			if (item == null)
			{
				this.titleText.text = this.failedTextKey.ToPlainText();
				this.itemDisplay.gameObject.SetActive(false);
				return;
			}
			this.titleText.text = this.succeedTextKey.ToPlainText();
			this.itemDisplay.Setup(item);
			this.itemDisplay.gameObject.SetActive(true);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0003E96D File Offset: 0x0003CB6D
		internal void NotifyStop()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x04000CD1 RID: 3281
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000CD2 RID: 3282
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x04000CD3 RID: 3283
		[SerializeField]
		[LocalizationKey("Default")]
		private string succeedTextKey = "Fishing_Succeed";

		// Token: 0x04000CD4 RID: 3284
		[SerializeField]
		[LocalizationKey("Default")]
		private string failedTextKey = "Fishing_Failed";

		// Token: 0x04000CD5 RID: 3285
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04000CD6 RID: 3286
		[SerializeField]
		private Button continueButton;

		// Token: 0x04000CD7 RID: 3287
		[SerializeField]
		private Button quitButton;

		// Token: 0x04000CD8 RID: 3288
		private bool confirmed;

		// Token: 0x04000CD9 RID: 3289
		private bool continueFishing;
	}
}
