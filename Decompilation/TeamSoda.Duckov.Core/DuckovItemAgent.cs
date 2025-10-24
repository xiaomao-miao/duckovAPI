using System;
using System.Collections.Generic;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E3 RID: 227
public class DuckovItemAgent : ItemAgent
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000725 RID: 1829 RVA: 0x000200AF File Offset: 0x0001E2AF
	public CharacterMainControl Holder
	{
		get
		{
			return this.holder;
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000726 RID: 1830 RVA: 0x000200B8 File Offset: 0x0001E2B8
	private Dictionary<string, Transform> SocketsDic
	{
		get
		{
			if (this._socketsDic == null)
			{
				this._socketsDic = new Dictionary<string, Transform>();
				foreach (Transform transform in this.socketsList)
				{
					this._socketsDic.Add(transform.name, transform);
				}
			}
			return this._socketsDic;
		}
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x00020130 File Offset: 0x0001E330
	public Transform GetSocket(string socketName, bool createNew)
	{
		Transform transform;
		bool flag = this.SocketsDic.TryGetValue(socketName, out transform);
		if (flag && transform == null)
		{
			this.SocketsDic.Remove(socketName);
		}
		if (!flag && createNew)
		{
			transform = new GameObject(socketName).transform;
			transform.SetParent(base.transform);
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			this.SocketsDic.Add(socketName, transform);
		}
		return transform;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x000201A7 File Offset: 0x0001E3A7
	public void SetHolder(CharacterMainControl _holder)
	{
		this.holder = _holder;
		if (this.setActiveIfMainCharacter)
		{
			this.setActiveIfMainCharacter.SetActive(_holder.IsMainCharacter);
		}
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x000201CE File Offset: 0x0001E3CE
	public CharacterMainControl GetHolder()
	{
		return this.holder;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x000201D6 File Offset: 0x0001E3D6
	protected override void OnInitialize()
	{
		base.OnInitialize();
		this.InitInterfaces();
		UnityEvent onInitializdEvent = this.OnInitializdEvent;
		if (onInitializdEvent == null)
		{
			return;
		}
		onInitializdEvent.Invoke();
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x000201F4 File Offset: 0x0001E3F4
	private void InitInterfaces()
	{
		this.usableInterface = (this as IAgentUsable);
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x0600072C RID: 1836 RVA: 0x00020202 File Offset: 0x0001E402
	public IAgentUsable UsableInterface
	{
		get
		{
			return this.usableInterface;
		}
	}

	// Token: 0x040006D0 RID: 1744
	public HandheldSocketTypes handheldSocket = HandheldSocketTypes.normalHandheld;

	// Token: 0x040006D1 RID: 1745
	public HandheldAnimationType handAnimationType = HandheldAnimationType.normal;

	// Token: 0x040006D2 RID: 1746
	private CharacterMainControl holder;

	// Token: 0x040006D3 RID: 1747
	public UnityEvent OnInitializdEvent;

	// Token: 0x040006D4 RID: 1748
	[SerializeField]
	private List<Transform> socketsList = new List<Transform>();

	// Token: 0x040006D5 RID: 1749
	public GameObject setActiveIfMainCharacter;

	// Token: 0x040006D6 RID: 1750
	private Dictionary<string, Transform> _socketsDic;

	// Token: 0x040006D7 RID: 1751
	private IAgentUsable usableInterface;
}
