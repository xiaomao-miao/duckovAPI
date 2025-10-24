using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Scenes;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200009E RID: 158
public class Carriable : MonoBehaviour
{
	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000545 RID: 1349 RVA: 0x00017889 File Offset: 0x00015A89
	private Inventory inventory
	{
		get
		{
			if (this.lootbox == null)
			{
				return null;
			}
			return this.lootbox.Inventory;
		}
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x000178A6 File Offset: 0x00015AA6
	public float GetWeight()
	{
		if (this.inventory)
		{
			return this.inventory.CachedWeight + this.selfWeight;
		}
		return this.selfWeight;
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x000178D0 File Offset: 0x00015AD0
	public void Take(CA_Carry _carrier)
	{
		if (!_carrier)
		{
			return;
		}
		if (this.carrier)
		{
			this.carrier.StopAction();
		}
		this.droping = false;
		this.carrier = _carrier;
		if (this.inventory)
		{
			this.inventory.RecalculateWeight();
		}
		this.rb.transform.SetParent(this.carrier.characterController.modelRoot);
		this.rb.velocity = Vector3.zero;
		this.rb.transform.position = this.carrier.characterController.modelRoot.TransformPoint(this.carrier.carryPoint);
		this.rb.transform.localRotation = Quaternion.identity;
		this.SetRigidbodyActive(false);
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x000179A4 File Offset: 0x00015BA4
	private void SetRigidbodyActive(bool active)
	{
		if (active)
		{
			this.rb.isKinematic = false;
			this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			if (this.lootbox && this.lootbox.interactCollider)
			{
				this.lootbox.interactCollider.isTrigger = false;
				return;
			}
		}
		else
		{
			this.rb.isKinematic = true;
			this.rb.interpolation = RigidbodyInterpolation.None;
			if (this.lootbox && this.lootbox.interactCollider)
			{
				this.lootbox.interactCollider.isTrigger = true;
			}
		}
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x00017A48 File Offset: 0x00015C48
	public void Drop()
	{
		if (this.carrier.Running)
		{
			this.carrier.StopAction();
		}
		this.carrier = null;
		MultiSceneCore.MoveToActiveWithScene(this.rb.gameObject, SceneManager.GetActiveScene().buildIndex);
		this.DropTask().Forget();
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00017AA0 File Offset: 0x00015CA0
	public void OnCarriableUpdate(float deltaTime)
	{
		if (!this.carrier)
		{
			return;
		}
		Vector3 position = this.carrier.characterController.modelRoot.TransformPoint(this.carrier.carryPoint);
		if (this.carrier.characterController.RightHandSocket)
		{
			position.y = this.carrier.characterController.RightHandSocket.transform.position.y + this.carrier.carryPoint.y;
		}
		this.rb.transform.position = position;
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00017B3C File Offset: 0x00015D3C
	private UniTaskVoid DropTask()
	{
		Carriable.<DropTask>d__14 <DropTask>d__;
		<DropTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<DropTask>d__.<>4__this = this;
		<DropTask>d__.<>1__state = -1;
		<DropTask>d__.<>t__builder.Start<Carriable.<DropTask>d__14>(ref <DropTask>d__);
		return <DropTask>d__.<>t__builder.Task;
	}

	// Token: 0x040004B6 RID: 1206
	private CA_Carry carrier;

	// Token: 0x040004B7 RID: 1207
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x040004B8 RID: 1208
	[SerializeField]
	private float selfWeight;

	// Token: 0x040004B9 RID: 1209
	public InteractableLootbox lootbox;

	// Token: 0x040004BA RID: 1210
	private bool droping;

	// Token: 0x040004BB RID: 1211
	private float startDropTime = -1f;

	// Token: 0x040004BC RID: 1212
	private bool carring;
}
