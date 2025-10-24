using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F6 RID: 502
public class SplitDialogue : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0003A912 File Offset: 0x00038B12
	public static SplitDialogue Instance
	{
		get
		{
			if (GameplayUIManager.Instance == null)
			{
				return null;
			}
			return GameplayUIManager.Instance.SplitDialogue;
		}
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x0003A92D File Offset: 0x00038B2D
	private void OnEnable()
	{
		View.OnActiveViewChanged += this.OnActiveViewChanged;
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0003A940 File Offset: 0x00038B40
	private void OnDisable()
	{
		View.OnActiveViewChanged -= this.OnActiveViewChanged;
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x0003A953 File Offset: 0x00038B53
	private void OnActiveViewChanged()
	{
		this.Hide();
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0003A95B File Offset: 0x00038B5B
	private void Awake()
	{
		this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
		this.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x0003A995 File Offset: 0x00038B95
	private void OnSliderValueChanged(float value)
	{
		this.RefreshCountText();
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x0003A9A0 File Offset: 0x00038BA0
	private void RefreshCountText()
	{
		this.countText.text = this.slider.value.ToString("0");
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x0003A9D0 File Offset: 0x00038BD0
	private void OnConfirmButtonClicked()
	{
		if (this.status != SplitDialogue.Status.Normal)
		{
			return;
		}
		this.Confirm().Forget();
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0003A9E8 File Offset: 0x00038BE8
	private void Setup(Item target, Inventory destination = null, int destinationIndex = -1)
	{
		this.target = target;
		this.destination = destination;
		this.destinationIndex = destinationIndex;
		this.slider.minValue = 1f;
		this.slider.maxValue = (float)target.StackCount;
		this.slider.value = (float)(target.StackCount - 1) / 2f;
		this.RefreshCountText();
		this.SwitchStatus(SplitDialogue.Status.Normal);
		this.cachedInInventory = target.InInventory;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x0003AA5F File Offset: 0x00038C5F
	public void Cancel()
	{
		if (this.status != SplitDialogue.Status.Normal)
		{
			return;
		}
		this.SwitchStatus(SplitDialogue.Status.Canceled);
		this.Hide();
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0003AA78 File Offset: 0x00038C78
	private UniTask Confirm()
	{
		SplitDialogue.<Confirm>d__22 <Confirm>d__;
		<Confirm>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Confirm>d__.<>4__this = this;
		<Confirm>d__.<>1__state = -1;
		<Confirm>d__.<>t__builder.Start<SplitDialogue.<Confirm>d__22>(ref <Confirm>d__);
		return <Confirm>d__.<>t__builder.Task;
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0003AABB File Offset: 0x00038CBB
	private void Hide()
	{
		this.fadeGroup.Hide();
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x0003AAC8 File Offset: 0x00038CC8
	private UniTask DoSplit(int value)
	{
		SplitDialogue.<DoSplit>d__24 <DoSplit>d__;
		<DoSplit>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DoSplit>d__.<>4__this = this;
		<DoSplit>d__.value = value;
		<DoSplit>d__.<>1__state = -1;
		<DoSplit>d__.<>t__builder.Start<SplitDialogue.<DoSplit>d__24>(ref <DoSplit>d__);
		return <DoSplit>d__.<>t__builder.Task;
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x0003AB14 File Offset: 0x00038D14
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject == base.gameObject)
		{
			this.Cancel();
		}
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x0003AB44 File Offset: 0x00038D44
	private void SwitchStatus(SplitDialogue.Status status)
	{
		this.status = status;
		this.normalIndicator.SetActive(status == SplitDialogue.Status.Normal);
		this.busyIndicator.SetActive(status == SplitDialogue.Status.Busy);
		this.completeIndicator.SetActive(status == SplitDialogue.Status.Complete);
		switch (status)
		{
		default:
			return;
		}
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0003AB9F File Offset: 0x00038D9F
	public static void SetupAndShow(Item item)
	{
		if (SplitDialogue.Instance == null)
		{
			return;
		}
		SplitDialogue.Instance.Setup(item, null, -1);
		SplitDialogue.Instance.fadeGroup.Show();
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x0003ABCB File Offset: 0x00038DCB
	public static void SetupAndShow(Item item, Inventory destinationInventory, int destinationIndex)
	{
		if (SplitDialogue.Instance == null)
		{
			return;
		}
		SplitDialogue.Instance.Setup(item, destinationInventory, destinationIndex);
		SplitDialogue.Instance.fadeGroup.Show();
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0003AC00 File Offset: 0x00038E00
	[CompilerGenerated]
	private void <DoSplit>g__Send|24_0(Item item)
	{
		item.Detach();
		if (this.destination != null && this.destination.Capacity > this.destinationIndex && this.destination.GetItemAt(this.destinationIndex) == null)
		{
			this.destination.AddAt(item, this.destinationIndex);
			return;
		}
		ItemUtilities.SendToPlayerCharacterInventory(item, true);
	}

	// Token: 0x04000C1B RID: 3099
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000C1C RID: 3100
	[SerializeField]
	private Button confirmButton;

	// Token: 0x04000C1D RID: 3101
	[SerializeField]
	private TextMeshProUGUI countText;

	// Token: 0x04000C1E RID: 3102
	[SerializeField]
	private GameObject normalIndicator;

	// Token: 0x04000C1F RID: 3103
	[SerializeField]
	private GameObject busyIndicator;

	// Token: 0x04000C20 RID: 3104
	[SerializeField]
	private GameObject completeIndicator;

	// Token: 0x04000C21 RID: 3105
	[SerializeField]
	private Slider slider;

	// Token: 0x04000C22 RID: 3106
	private Item target;

	// Token: 0x04000C23 RID: 3107
	private Inventory destination;

	// Token: 0x04000C24 RID: 3108
	private int destinationIndex;

	// Token: 0x04000C25 RID: 3109
	private Inventory cachedInInventory;

	// Token: 0x04000C26 RID: 3110
	private SplitDialogue.Status status;

	// Token: 0x020004DE RID: 1246
	private enum Status
	{
		// Token: 0x04001D12 RID: 7442
		Idle,
		// Token: 0x04001D13 RID: 7443
		Normal,
		// Token: 0x04001D14 RID: 7444
		Busy,
		// Token: 0x04001D15 RID: 7445
		Complete,
		// Token: 0x04001D16 RID: 7446
		Canceled
	}
}
