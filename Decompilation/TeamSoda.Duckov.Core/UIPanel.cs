using System;
using Duckov.UI.Animations;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class UIPanel : MonoBehaviour
{
	// Token: 0x06000A73 RID: 2675 RVA: 0x0002DCA9 File Offset: 0x0002BEA9
	protected virtual void OnOpen()
	{
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0002DCAB File Offset: 0x0002BEAB
	protected virtual void OnClose()
	{
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0002DCAD File Offset: 0x0002BEAD
	protected virtual void OnChildOpened(UIPanel child)
	{
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0002DCAF File Offset: 0x0002BEAF
	protected virtual void OnChildClosed(UIPanel child)
	{
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0002DCB1 File Offset: 0x0002BEB1
	internal void Open(UIPanel parent = null, bool controlFadeGroup = true)
	{
		this.parent = parent;
		this.OnOpen();
		if (controlFadeGroup)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0002DCD4 File Offset: 0x0002BED4
	public void Close()
	{
		if (this.activeChild != null)
		{
			this.activeChild.Close();
		}
		this.OnClose();
		UIPanel uipanel = this.parent;
		if (uipanel != null)
		{
			uipanel.NotifyChildClosed(this);
		}
		FadeGroup fadeGroup = this.fadeGroup;
		if (fadeGroup == null)
		{
			return;
		}
		fadeGroup.Hide();
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0002DD24 File Offset: 0x0002BF24
	public void OpenChild(UIPanel childPanel)
	{
		if (childPanel == null)
		{
			return;
		}
		if (this.activeChild != null)
		{
			this.activeChild.Close();
		}
		this.activeChild = childPanel;
		childPanel.Open(this, true);
		this.OnChildOpened(childPanel);
		if (this.hideWhenChildActive)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Hide();
		}
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0002DD82 File Offset: 0x0002BF82
	private void NotifyChildClosed(UIPanel child)
	{
		this.OnChildClosed(child);
		if (this.hideWhenChildActive)
		{
			FadeGroup fadeGroup = this.fadeGroup;
			if (fadeGroup == null)
			{
				return;
			}
			fadeGroup.Show();
		}
	}

	// Token: 0x0400091D RID: 2333
	[SerializeField]
	protected FadeGroup fadeGroup;

	// Token: 0x0400091E RID: 2334
	[SerializeField]
	private bool hideWhenChildActive;

	// Token: 0x0400091F RID: 2335
	private UIPanel parent;

	// Token: 0x04000920 RID: 2336
	private UIPanel activeChild;
}
