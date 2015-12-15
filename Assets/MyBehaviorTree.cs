using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using System;

public class MyBehaviorTree : MonoBehaviour
{
	public Transform wander1;
	public Transform wander2;
	public Transform wander3;



	public GameObject Daniel;
    public GameObject Peter;
    //for test,set some test bool to test the IBT
    bool testIBT = false;


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
        if (Input.GetKey(KeyCode.A))
            testIBT = false;
        if (Input.GetKey(KeyCode.B))
            testIBT = true;
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

    protected Node ST_IBT()
    {
        Func<bool> con = () => (testIBT == false);
        Node trigger = new DecoratorLoop(new LeafAssert(con));
        return new SequenceParallel(
            new Sequence(
                

                )
            );
    }
    protected Node ST_TestAssert()
    {
        Func<bool> con = () => (testIBT == false);
        Node trigger = new DecoratorLoop (new LeafAssert(con));
        return new DecoratorForceStatus(RunStatus.Success, new SequenceParallel(trigger,new DecoratorLoop( new LeafTrace("2"))));
    }
	protected Node BuildTreeRoot()
	{
        return
          new DecoratorLoop(
              new Sequence(
                this.ST_TestAssert(),
                new LeafTrace("1")
                )
                )
                            ;
	}
}
