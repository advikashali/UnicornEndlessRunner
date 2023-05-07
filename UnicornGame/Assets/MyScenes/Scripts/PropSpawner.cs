using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{

	public PropType type;
	private Prop currentProp;

	public void Spawn()
	{
		//currentProp = LevelManager.Instance.GetPiece(type, 0);
		currentProp.gameObject.SetActive(true);
		currentProp.transform.SetParent(transform, false);
	}

	public void DeSpawn()
	{
		currentProp.gameObject.SetActive(false);
	}
}