using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class PackedMapData : ScriptableObject, IMiniMapDataProvider
{
	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000D6E RID: 3438 RVA: 0x000376E4 File Offset: 0x000358E4
	public Sprite CombinedSprite
	{
		get
		{
			return this.combinedSprite;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000D6F RID: 3439 RVA: 0x000376EC File Offset: 0x000358EC
	public float PixelSize
	{
		get
		{
			return this.pixelSize;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000D70 RID: 3440 RVA: 0x000376F4 File Offset: 0x000358F4
	public Vector3 CombinedCenter
	{
		get
		{
			return this.combinedCenter;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000D71 RID: 3441 RVA: 0x000376FC File Offset: 0x000358FC
	public List<IMiniMapEntry> Maps
	{
		get
		{
			return this.maps.ToList<IMiniMapEntry>();
		}
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x0003770C File Offset: 0x0003590C
	internal void Setup(IMiniMapDataProvider origin)
	{
		this.combinedSprite = origin.CombinedSprite;
		this.pixelSize = origin.PixelSize;
		this.combinedCenter = origin.CombinedCenter;
		this.maps.Clear();
		foreach (IMiniMapEntry miniMapEntry in origin.Maps)
		{
			PackedMapData.Entry item = new PackedMapData.Entry(miniMapEntry.Sprite, miniMapEntry.PixelSize, miniMapEntry.Offset, miniMapEntry.SceneID, miniMapEntry.Hide, miniMapEntry.NoSignal);
			this.maps.Add(item);
		}
	}

	// Token: 0x04000B69 RID: 2921
	[SerializeField]
	private Sprite combinedSprite;

	// Token: 0x04000B6A RID: 2922
	[SerializeField]
	private float pixelSize;

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	private Vector3 combinedCenter;

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	private List<PackedMapData.Entry> maps = new List<PackedMapData.Entry>();

	// Token: 0x020004D4 RID: 1236
	[Serializable]
	public class Entry : IMiniMapEntry
	{
		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x0008DB1D File Offset: 0x0008BD1D
		public Sprite Sprite
		{
			get
			{
				return this.sprite;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600271D RID: 10013 RVA: 0x0008DB25 File Offset: 0x0008BD25
		public float PixelSize
		{
			get
			{
				return this.pixelSize;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600271E RID: 10014 RVA: 0x0008DB2D File Offset: 0x0008BD2D
		public Vector2 Offset
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600271F RID: 10015 RVA: 0x0008DB35 File Offset: 0x0008BD35
		public string SceneID
		{
			get
			{
				return this.sceneID;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06002720 RID: 10016 RVA: 0x0008DB3D File Offset: 0x0008BD3D
		public bool Hide
		{
			get
			{
				return this.hide;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06002721 RID: 10017 RVA: 0x0008DB45 File Offset: 0x0008BD45
		public bool NoSignal
		{
			get
			{
				return this.noSignal;
			}
		}

		// Token: 0x06002722 RID: 10018 RVA: 0x0008DB4D File Offset: 0x0008BD4D
		public Entry()
		{
		}

		// Token: 0x06002723 RID: 10019 RVA: 0x0008DB55 File Offset: 0x0008BD55
		public Entry(Sprite sprite, float pixelSize, Vector2 offset, string sceneID, bool hide, bool noSignal)
		{
			this.sprite = sprite;
			this.pixelSize = pixelSize;
			this.offset = offset;
			this.sceneID = sceneID;
			this.hide = hide;
			this.noSignal = noSignal;
		}

		// Token: 0x04001CE9 RID: 7401
		[SerializeField]
		private Sprite sprite;

		// Token: 0x04001CEA RID: 7402
		[SerializeField]
		private float pixelSize;

		// Token: 0x04001CEB RID: 7403
		[SerializeField]
		private Vector2 offset;

		// Token: 0x04001CEC RID: 7404
		[SerializeField]
		private string sceneID;

		// Token: 0x04001CED RID: 7405
		[SerializeField]
		private bool hide;

		// Token: 0x04001CEE RID: 7406
		[SerializeField]
		private bool noSignal;
	}
}
