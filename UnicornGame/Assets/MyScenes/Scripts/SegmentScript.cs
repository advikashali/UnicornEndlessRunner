using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentScript : MonoBehaviour
{

	public int SegID { set; get; }
	public bool transition;

	public int lenght;
	public int beginY1, beginY2, beginY3;
	public int endY1, endY2, endY3;

	public PropSpawner[] props;

	private void Awake()
	{
		props = gameObject.GetComponentsInChildren<PropSpawner>();
	}

	public void Spawn()
	{
		gameObject.SetActive(true);

		for (int i = 0; i < props.Length; i++)
        {
			props[i].Spawn();
        }
	}

	public void DeSpawn()
	{
		gameObject.SetActive(false);

		for (int i = 0; i < props.Length; i++)
		{
			props[i].DeSpawn();
		}
	}

}
