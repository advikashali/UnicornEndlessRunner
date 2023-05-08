using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{

	public static LevelManagerScript Instance { set; get; }

	private const bool SHOW_COLLIDER = true;

	// Level spawning
	private const float DISTANCE_BEFORE_SPAWN = 100f;
	private const int INITIAL_SEGMENT = 10;
	private const int MAX_SEGMENT_ON_SCREEN = 15;
	private Transform cameraContainer;
	private int amountOfActiveSegment;
	private int continuousSegments;
	private int currentSpawnZ;
	private int currentLevel;
	private int y1, y2, y3;

	// List of props
	public List<Prop> ramps = new List<Prop>();
	public List<Prop> longBlocks = new List<Prop>();
	public List<Prop> jumps = new List<Prop>();
	public List<Prop> slides = new List<Prop>();
	[HideInInspector]
	public List<Prop> props = new List<Prop>(); // All props in the pool

	// List of segments
	public List<SegmentScript> availableSegments = new List<SegmentScript>();
	public List<SegmentScript> availableTransitions = new List<SegmentScript>();
	[HideInInspector]
	public List<SegmentScript> segments = new List<SegmentScript>();

	// Gameplay
	private bool isMoving = false;


	private void Awake() // mentioned 
	{
		cameraContainer = Camera.main.transform;
		currentSpawnZ = 0;
		currentLevel = 0;
	}

	private void Start()
	{
		for (int i = 0; i < INITIAL_SEGMENT; i++)
		{
			GenerateSegment();
		}
	}

	private void GenerateSegment()
	{
		SpawnSegment();

		if (Random.Range(0f, 1f) < (continuousSegments * 0.25f))
		{
			SpawnTransition();
			continuousSegments = 0;
		}
		else
		{
			continuousSegments++;
		}
	}

	private void Update()
	{
		if (currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
		{
			GenerateSegment();
		}

		if (amountOfActiveSegment >= MAX_SEGMENT_ON_SCREEN)
		{
			segments[amountOfActiveSegment - 1].DeSpawn();
			amountOfActiveSegment--;
		}
	}


	private void SpawnSegment()
	{
		List<SegmentScript> possibleSegments = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
		int id = Random.Range(0, possibleSegments.Count);

		SegmentScript s = GetSegment(id, false);
		y1 = s.endY1;
		y2 = s.endY2;
		y3 = s.endY3;

		s.transform.SetParent(transform);
		s.transform.localPosition = Vector3.forward * currentSpawnZ;

		currentSpawnZ += s.lenght;
		amountOfActiveSegment++;
		s.Spawn();
	}


	private void SpawnTransition()
	{
		List<SegmentScript> possibleTransition = availableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
		int id = Random.Range(0, possibleTransition.Count);

		SegmentScript s = GetSegment(id, true);
		y1 = s.endY1;
		y2 = s.endY2;
		y3 = s.endY3;

		s.transform.SetParent(transform);
		s.transform.localPosition = Vector3.forward * currentSpawnZ;

		currentSpawnZ += s.lenght;
		amountOfActiveSegment++;
		s.Spawn();
	}

	public SegmentScript GetSegment(int id, bool transition)
	{
		SegmentScript s = null;
		s = segments.Find(x => x.SegID == id && x.transition == transition && !x.gameObject.activeSelf);

		if (s == null)
		{
			GameObject go = Instantiate((transition) ? availableTransitions[id].gameObject : availableSegments[id].gameObject) as GameObject;
			s = go.GetComponent<SegmentScript>();
			s.SegID = id;
			s.transition = transition;

			segments.Insert(0, s);
		}
		else
		{
			segments.Remove(s);
			segments.Insert(0, s);
		}

		return s;

	}

	public Prop GetProp(PropType pt, int visualIndex)
	{
		Prop p = props.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

		if (p == null)
		{
			GameObject go = null;

			switch (pt)
			{
				case PropType.ramp:
					go = ramps[visualIndex].gameObject;
					break;
				case PropType.longBlock:
					go = longBlocks[visualIndex].gameObject;
					break;
				case PropType.jump:
					go = jumps[visualIndex].gameObject;
					break;
				case PropType.slide:
					go = slides[visualIndex].gameObject;
					break;
			}
			go = Instantiate(go);
			p = go.GetComponent<Prop>();
			props.Add(p);
		}

		return p;
	}

}