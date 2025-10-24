using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class Points : MonoBehaviour
{
	// Token: 0x06000A57 RID: 2647 RVA: 0x0002D91C File Offset: 0x0002BB1C
	public void SetYtoZero()
	{
		for (int i = 0; i < this.points.Count; i++)
		{
			this.points[i] = new Vector3(this.points[i].x, 0f, this.points[i].z);
		}
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0002D977 File Offset: 0x0002BB77
	public void RemoveAllPoints()
	{
		this.points = new List<Vector3>();
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0002D984 File Offset: 0x0002BB84
	public List<Vector3> GetRandomPoints(int count)
	{
		List<Vector3> list = new List<Vector3>();
		list.AddRange(this.points);
		List<Vector3> list2 = new List<Vector3>();
		while (list2.Count < count && list.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			Vector3 item = this.PointToWorld(list[index]);
			list2.Add(item);
			list.RemoveAt(index);
		}
		return list2;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0002D9E8 File Offset: 0x0002BBE8
	public Vector3 GetRandomPoint()
	{
		int index = UnityEngine.Random.Range(0, this.points.Count);
		return this.GetPoint(index);
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0002DA10 File Offset: 0x0002BC10
	public Vector3 GetPoint(int index)
	{
		if (index >= this.points.Count)
		{
			return Vector3.zero;
		}
		Vector3 point = this.points[index];
		return this.PointToWorld(point);
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0002DA45 File Offset: 0x0002BC45
	private Vector3 PointToWorld(Vector3 point)
	{
		if (!this.worldSpace)
		{
			point = base.transform.TransformPoint(point);
		}
		return point;
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0002DA60 File Offset: 0x0002BC60
	public void SendPointsChangedMessage()
	{
		IOnPointsChanged component = base.GetComponent<IOnPointsChanged>();
		if (component != null)
		{
			component.OnPointsChanged();
		}
	}

	// Token: 0x0400090A RID: 2314
	public List<Vector3> points;

	// Token: 0x0400090B RID: 2315
	[HideInInspector]
	public int lastSelectedPointIndex = -1;

	// Token: 0x0400090C RID: 2316
	public bool worldSpace = true;

	// Token: 0x0400090D RID: 2317
	public bool syncToSelectedPoint;
}
