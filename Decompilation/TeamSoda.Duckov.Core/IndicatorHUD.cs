using System;
using UnityEngine;

// Token: 0x020000BD RID: 189
public class IndicatorHUD : MonoBehaviour
{
	// Token: 0x06000617 RID: 1559 RVA: 0x0001B540 File Offset: 0x00019740
	private void Start()
	{
		if ((LevelManager.Instance == null || LevelManager.Instance.IsBaseLevel) && this.mapIndicator)
		{
			this.mapIndicator.SetActive(false);
		}
		this.toggleParent.SetActive(this.startActive);
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0001B590 File Offset: 0x00019790
	private void Awake()
	{
		UIInputManager.OnToggleIndicatorHUD += this.Toggle;
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0001B5A3 File Offset: 0x000197A3
	private void OnDestroy()
	{
		UIInputManager.OnToggleIndicatorHUD -= this.Toggle;
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0001B5B6 File Offset: 0x000197B6
	private void Toggle(UIInputEventData data)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.toggleParent.SetActive(!this.toggleParent.activeInHierarchy);
	}

	// Token: 0x040005B3 RID: 1459
	public GameObject mapIndicator;

	// Token: 0x040005B4 RID: 1460
	public GameObject toggleParent;

	// Token: 0x040005B5 RID: 1461
	public bool startActive;
}
