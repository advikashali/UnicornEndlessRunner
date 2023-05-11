using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{

	public PropType type;
	private Prop currentProp;

	public void Spawn()
	{
		int amountObject = 0;
		switch (type)
		{
			case PropType.jump:
				amountObject = LevelManagerScript.Instance.jumps.Count;
				break;
			case PropType.slide:
				amountObject = LevelManagerScript.Instance.slides.Count;
				break;
			case PropType.longBlock:
				amountObject = LevelManagerScript.Instance.longBlocks.Count;
				break;
			case PropType.ramp:
				amountObject = LevelManagerScript.Instance.ramps.Count;
				break;

		}

		currentProp = LevelManagerScript.Instance.GetProp(type, Random.Range(0, amountObject));
		currentProp.gameObject.SetActive(true);
		currentProp.transform.SetParent(transform, false);
	}

	public void DeSpawn()
	{
		currentProp.gameObject.SetActive(false);
	}
}