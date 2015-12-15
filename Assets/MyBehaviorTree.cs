using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class MyBehaviorTree : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;



	public GameObject Daniel;
    public GameObject Peter;



	private BehaviorAgent behaviorAgent;
	// Use this for initialization
	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	protected Node ST_ApproachAndWait(Transform target,GameObject participant)
	{
		Val<Vector3> position = Val.V (() => target.position);
		return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
	}

    protected Node ST_Wander(GameObject participant)
    {
        return new Sequence(
             this.ST_ApproachAndWait(wander1, participant),
                         this.ST_ApproachAndWait(wander2, participant),
                         this.ST_ApproachAndWait(wander3, participant)
             );
    }


	protected Node BuildTreeRoot()
	{
        return
            new DecoratorLoop(
                new SequenceParallel( 
                    new LeafTrace("1"),
                    new LeafTrace("2")
                             )
                            );
	}
}
