using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000C9 RID: 201
public class ReloadHUD : MonoBehaviour
{
	// Token: 0x0600064A RID: 1610 RVA: 0x0001C4F0 File Offset: 0x0001A6F0
	private void Update()
	{
		if (this.characterMainControl == null)
		{
			this.characterMainControl = LevelManager.Instance.MainCharacter;
			if (this.characterMainControl == null)
			{
				return;
			}
			this.button.onClick.AddListener(new UnityAction(this.Reload));
		}
		this.reloadable = this.characterMainControl.GetGunReloadable();
		if (this.reloadable != this.button.interactable)
		{
			this.button.interactable = this.reloadable;
			if (this.reloadable)
			{
				UnityEvent onShowEvent = this.OnShowEvent;
				if (onShowEvent != null)
				{
					onShowEvent.Invoke();
				}
			}
			else
			{
				UnityEvent onHideEvent = this.OnHideEvent;
				if (onHideEvent != null)
				{
					onHideEvent.Invoke();
				}
			}
		}
		this.frame++;
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x0001C5B5 File Offset: 0x0001A7B5
	private void OnDestroy()
	{
		this.button.onClick.RemoveAllListeners();
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x0001C5C7 File Offset: 0x0001A7C7
	private void Reload()
	{
		if (this.characterMainControl)
		{
			this.characterMainControl.TryToReload(null);
		}
	}

	// Token: 0x04000608 RID: 1544
	private CharacterMainControl characterMainControl;

	// Token: 0x04000609 RID: 1545
	public Button button;

	// Token: 0x0400060A RID: 1546
	private bool reloadable;

	// Token: 0x0400060B RID: 1547
	public UnityEvent OnShowEvent;

	// Token: 0x0400060C RID: 1548
	public UnityEvent OnHideEvent;

	// Token: 0x0400060D RID: 1549
	private int frame;
}
