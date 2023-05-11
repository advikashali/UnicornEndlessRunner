using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{

	public bool SHOW_COLLIDER = true; //!! 

	public static LevelManagerScript Instance { set; get; }

	// Level spawning
	private const float DISTANCE_BEFORE_SPAWN = 100f; //how much distance player will see before spawning more
	private const int INITIAL_SEGMENT = 10; //how many segments before game starts 
	private const int INITIAL_TRANSITION_SEGMENTS = 2; 
	private const int MAX_SEGMENT_ON_SCREEN = 15; //how many segments until its time to despawn
	private Transform cameraContainer;
	private int amountOfActiveSegment;
	private int continuousSegment; //for spawning transition segments 
	private int currentSpawnZ;
	//private int currentlevel; //grey..
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
	private bool ismoving = false;


	private void Awake() 
	{
		Instance = this;
		cameraContainer = Camera.main.transform;
		currentSpawnZ = 0;
		//currentlevel = 0;
	}

	private void Start() 
	{
		for (int i = 0; i < INITIAL_SEGMENT; i++)
			if (i < INITIAL_TRANSITION_SEGMENTS)
				SpawnTransition();
			else
				GeneratesSegment();
		
	}

	private void Update()
	{
		if (currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPAWN)
		GeneratesSegment();
		

		if (amountOfActiveSegment >= MAX_SEGMENT_ON_SCREEN)
		{
			segments[amountOfActiveSegment - 1].DeSpawn();
			amountOfActiveSegment--;
		}
	}


	private void GeneratesSegment()
	{
		SpawnSegment();

		if (Random.Range(0f, 1f) < (continuousSegment * 0.25f))
		{
			SpawnTransition();
			continuousSegment = 0;
		}
		else
		{
			continuousSegment++;
		}
	}

	private void SpawnSegment()
	{
		List<SegmentScript> possibleSeg = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
		int id = Random.Range(0, possibleSeg.Count);

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
		List<SegmentScript> possibleTransitions = availableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
		int id = Random.Range(0, possibleTransitions.Count);

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

			if (pt == PropType.ramp)
				go = ramps[visualIndex].gameObject;
			else if (pt == PropType.longBlock)
				go = longBlocks[visualIndex].gameObject;
			else if (pt == PropType.jump)
				go = jumps[visualIndex].gameObject;
			else if (pt == PropType.slide)
				go = slides[visualIndex].gameObject;

			go = Instantiate(go);
			p = go.GetComponent<Prop>();
			props.Add(p);
		


			//switch (pt)
			//{
				//case PropType.ramp:
					//go = ramps[visualIndex].gameObject;
					//break;
				//case PropType.longBlock:
					//go = longBlocks[visualIndex].gameObject;
					//break;
				//case PropType.jump:
					//go = jumps[visualIndex].gameObject;
					//break;
				//case PropType.slide:
					//go = slides[visualIndex].gameObject;
					//break;
			
		}

		return p;
	}

}