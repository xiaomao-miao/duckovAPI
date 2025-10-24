using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[Serializable]
public class CustomFacePartUtility
{
	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06000109 RID: 265 RVA: 0x00005367 File Offset: 0x00003567
	public CustomFacePart PartInstance
	{
		get
		{
			return this.facePartInstance;
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0000536F File Offset: 0x0000356F
	public int GetCurrentPartID()
	{
		if (this.facePartInstance != null)
		{
			return this.facePartInstance.id;
		}
		return -1;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000538C File Offset: 0x0000358C
	public string GetCurrentPartName()
	{
		if (this.facePartInstance != null)
		{
			return this.facePartInstance.id.ToString();
		}
		return "Empty";
	}

	// Token: 0x0600010C RID: 268 RVA: 0x000053B4 File Offset: 0x000035B4
	public void ChangePart(CustomFacePart newInstance)
	{
		if (this.facePartInstance)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(this.facePartInstance.gameObject);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(this.facePartInstance.gameObject);
			}
		}
		this.facePartInstance = newInstance;
		newInstance.transform.SetParent(this.socket);
		newInstance.transform.localPosition = Vector3.zero;
		newInstance.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x0000542F File Offset: 0x0000362F
	public void RefreshThisPart()
	{
		if (this.facePartInstance)
		{
			this.facePartInstance.SetInfo(this.partInfo, this.parent);
		}
	}

	// Token: 0x0400007E RID: 126
	public CustomFaceInstance parent;

	// Token: 0x0400007F RID: 127
	public CustomFacePartTypes partType;

	// Token: 0x04000080 RID: 128
	public CustomFacePartInfo partInfo;

	// Token: 0x04000081 RID: 129
	public Transform socket;

	// Token: 0x04000082 RID: 130
	private CustomFacePart facePartInstance;
}
