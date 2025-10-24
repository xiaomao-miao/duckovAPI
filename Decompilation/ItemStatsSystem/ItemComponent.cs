using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x0200000C RID: 12
	public class ItemComponent : MonoBehaviour, ISelfValidator
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002FB3 File Offset: 0x000011B3
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002FBB File Offset: 0x000011BB
		public Item Master
		{
			get
			{
				return this.master;
			}
			internal set
			{
				this.master = value;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002FC4 File Offset: 0x000011C4
		private void Awake()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.OnAwake();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002FDA File Offset: 0x000011DA
		protected virtual void OnAwake()
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002FDC File Offset: 0x000011DC
		internal void Initialize()
		{
			this.initialized = true;
			if (this.master == null)
			{
				this.master = base.GetComponent<Item>();
			}
			this.OnInitialize();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003005 File Offset: 0x00001205
		internal virtual void OnInitialize()
		{
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003008 File Offset: 0x00001208
		public void Validate(SelfValidationResult result)
		{
			if (this.Master == null)
			{
				result.AddError("这个组件依赖Item，Item不可以留空。").WithFix("设置为本Game Object上的Item", delegate()
				{
					this.master = base.GetComponent<Item>();
				}, true);
				return;
			}
			if (this.Master.gameObject != base.gameObject)
			{
				result.AddError("Master需要和本组件处于同一个Game Object上。").WithFix("设置为本Game Object上的Item", delegate()
				{
					this.master = base.GetComponent<Item>();
				}, true);
			}
		}

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private Item master;

		// Token: 0x04000029 RID: 41
		private bool initialized;
	}
}
