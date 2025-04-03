using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;


namespace NodeCanvas.Tasks.Actions {

	public class NavigateAT : ActionTask {

        public BBParameter<Vector3> targetPosition;
        public float sampleFrequency;
        public float sampleRadius;

        private NavMeshAgent navAgent;
        private float timeSinceLastSample = 0f;
        private Vector3 lastTarget = Vector3.zero;

        public BBParameter<Vector3> currentPosition;
        public GameObject monster;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit() {
            navAgent = agent.GetComponent<NavMeshAgent>();

            return null;
        }

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {

		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
            timeSinceLastSample += Time.deltaTime;
            if (timeSinceLastSample > sampleFrequency)
            {
                timeSinceLastSample = 0f;
                if (lastTarget != targetPosition.value)
                {
                    NavMeshHit navMeshHit;
                    bool wasValidPointDetected = NavMesh.SamplePosition(targetPosition.value, out navMeshHit, sampleRadius, NavMesh.AllAreas);

                    if (wasValidPointDetected)
                    {
                        navAgent.SetDestination(navMeshHit.position);
                    }
                    lastTarget = targetPosition.value;
                }
            }

            currentPosition.value = monster.transform.position;
        }

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}