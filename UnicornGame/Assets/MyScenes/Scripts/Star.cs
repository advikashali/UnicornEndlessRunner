using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameManagerScript.Instance.GetStar();
			animator.SetTrigger("Collected");
			Destroy(gameObject, 1.5f);
		}
	}
}
