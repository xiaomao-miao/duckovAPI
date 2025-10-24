using System;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x02000087 RID: 135
public class SpawnPaperBoxAction : EffectAction
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00015E07 File Offset: 0x00014007
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

	// Token: 0x060004D2 RID: 1234 RVA: 0x00015E44 File Offset: 0x00014044
	protected override void OnTriggered(bool positive)
	{
		if (!this.MainControl || !this.MainControl.characterModel)
		{
			return;
		}
		Transform transform = this.MainControl.transform;
		switch (this.socket)
		{
		case SpawnPaperBoxAction.Sockets.root:
			break;
		case SpawnPaperBoxAction.Sockets.helmat:
			transform = this.MainControl.characterModel.HelmatSocket;
			break;
		case SpawnPaperBoxAction.Sockets.armor:
			transform = this.MainControl.characterModel.ArmorSocket;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (!transform)
		{
			return;
		}
		if (!this.paperBoxPrefab)
		{
			return;
		}
		this.instance = UnityEngine.Object.Instantiate<PaperBox>(this.paperBoxPrefab, transform);
		this.instance.character = this.MainControl;
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00015EFE File Offset: 0x000140FE
	private void OnDestroy()
	{
		if (this.instance)
		{
			UnityEngine.Object.Destroy(this.instance.gameObject);
		}
	}

	// Token: 0x0400040D RID: 1037
	public SpawnPaperBoxAction.Sockets socket = SpawnPaperBoxAction.Sockets.helmat;

	// Token: 0x0400040E RID: 1038
	public PaperBox paperBoxPrefab;

	// Token: 0x0400040F RID: 1039
	private PaperBox instance;

	// Token: 0x04000410 RID: 1040
	private CharacterMainControl _mainControl;

	// Token: 0x0200043E RID: 1086
	public enum Sockets
	{
		// Token: 0x04001A6D RID: 6765
		root,
		// Token: 0x04001A6E RID: 6766
		helmat,
		// Token: 0x04001A6F RID: 6767
		armor
	}
}
