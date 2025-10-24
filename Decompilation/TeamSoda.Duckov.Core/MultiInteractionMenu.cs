using System;
using System.Collections.ObjectModel;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class MultiInteractionMenu : MonoBehaviour
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x0003B04E File Offset: 0x0003924E
	// (set) Token: 0x06000ED3 RID: 3795 RVA: 0x0003B055 File Offset: 0x00039255
	public static MultiInteractionMenu Instance { get; private set; }

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x0003B060 File Offset: 0x00039260
	private PrefabPool<MultiInteractionMenuButton> ButtonPool
	{
		get
		{
			if (this._buttonPool == null)
			{
				this._buttonPool = new PrefabPool<MultiInteractionMenuButton>(this.buttonTemplate, this.buttonTemplate.transform.parent, null, null, null, true, 10, 10000, null);
				this.buttonTemplate.gameObject.SetActive(false);
			}
			return this._buttonPool;
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0003B0B9 File Offset: 0x000392B9
	public MultiInteraction Target
	{
		get
		{
			return this.target;
		}
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x0003B0C1 File Offset: 0x000392C1
	private void Awake()
	{
		if (MultiInteractionMenu.Instance == null)
		{
			MultiInteractionMenu.Instance = this;
		}
		this.buttonTemplate.gameObject.SetActive(false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x0003B0F4 File Offset: 0x000392F4
	private void Setup(MultiInteraction target)
	{
		this.target = target;
		ReadOnlyCollection<InteractableBase> interactables = target.Interactables;
		this.ButtonPool.ReleaseAll();
		foreach (InteractableBase x in interactables)
		{
			if (!(x == null))
			{
				MultiInteractionMenuButton multiInteractionMenuButton = this.ButtonPool.Get(null);
				multiInteractionMenuButton.Setup(x);
				multiInteractionMenuButton.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x0003B174 File Offset: 0x00039374
	private int CreateNewToken()
	{
		this.currentTaskToken = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		return this.currentTaskToken;
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x0003B191 File Offset: 0x00039391
	private bool TokenChanged(int token)
	{
		return token != this.currentTaskToken;
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x0003B1A0 File Offset: 0x000393A0
	public UniTask SetupAndShow(MultiInteraction target)
	{
		MultiInteractionMenu.<SetupAndShow>d__17 <SetupAndShow>d__;
		<SetupAndShow>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<SetupAndShow>d__.<>4__this = this;
		<SetupAndShow>d__.target = target;
		<SetupAndShow>d__.<>1__state = -1;
		<SetupAndShow>d__.<>t__builder.Start<MultiInteractionMenu.<SetupAndShow>d__17>(ref <SetupAndShow>d__);
		return <SetupAndShow>d__.<>t__builder.Task;
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x0003B1EC File Offset: 0x000393EC
	public UniTask Hide()
	{
		MultiInteractionMenu.<Hide>d__18 <Hide>d__;
		<Hide>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<Hide>d__.<>4__this = this;
		<Hide>d__.<>1__state = -1;
		<Hide>d__.<>t__builder.Start<MultiInteractionMenu.<Hide>d__18>(ref <Hide>d__);
		return <Hide>d__.<>t__builder.Task;
	}

	// Token: 0x04000C3C RID: 3132
	[SerializeField]
	private MultiInteractionMenuButton buttonTemplate;

	// Token: 0x04000C3D RID: 3133
	[SerializeField]
	private float delayEachButton = 0.25f;

	// Token: 0x04000C3E RID: 3134
	private PrefabPool<MultiInteractionMenuButton> _buttonPool;

	// Token: 0x04000C3F RID: 3135
	private MultiInteraction target;

	// Token: 0x04000C40 RID: 3136
	private int currentTaskToken;
}
