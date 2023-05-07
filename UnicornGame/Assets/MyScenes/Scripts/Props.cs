using UnityEngine;

public enum PropType
{
	none = -1,
	ramp = 0,
	longBlock = 1,
	jump = 2,
	slide = 3,
}

public class Prop : MonoBehaviour
{
	public PropType type;
	public int visualIndex;
}
