using System;
using ItemStatsSystem;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class FxAction : EffectAction
{
	// Token: 0x170000FD RID: 253
	// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00015A97 File Offset: 0x00013C97
	private CharacterMainControl MainControl
	{
		get
		{
			if (this._mainControl == null)
			{
				Effect master = base.Master;
				CharacterMainControl mainControl;
				if (master == null)
				{
					mainControl = null;
				}
				else
				{
					Item item = master.Item;
					mainControl = ((item != null) ? item.GetCharacterMainControl() : null);
				}
				this._mainControl = mainControl;
			}
			return this._mainControl;
		}
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00015AD4 File Offset: 0x00013CD4
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl || !this.MainControl.characterModel)
		{
			return;
		}
		Transform transform = this.MainControl.transform;
		switch (this.socket)
		{
		case FxAction.Sockets.root:
			break;
		case FxAction.Sockets.helmat:
			transform = this.MainControl.characterModel.HelmatSocket;
			break;
		case FxAction.Sockets.armor:
			transform = this.MainControl.characterModel.ArmorSocket;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (!transform)
		{
			return;
		}
		if (!this.fxPfb)
		{
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(this.fxPfb, transform.position, quaternion.identity);
	}

	// Token: 0x040003FD RID: 1021
	public FxAction.Sockets socket = FxAction.Sockets.helmat;

	// Token: 0x040003FE RID: 1022
	public GameObject fxPfb;

	// Token: 0x040003FF RID: 1023
	private CharacterMainControl _mainControl;

	// Token: 0x0200043D RID: 1085
	public enum Sockets
	{
		// Token: 0x04001A69 RID: 6761
		root,
		// Token: 0x04001A6A RID: 6762
		helmat,
		// Token: 0x04001A6B RID: 6763
		armor
	}
}
