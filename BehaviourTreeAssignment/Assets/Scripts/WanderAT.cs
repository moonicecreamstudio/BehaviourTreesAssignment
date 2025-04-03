using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class WanderAT : ActionTask {

        public BBParameter<Vector3> targetPosition;
        public BBParameter<Vector3> currentPosition;
        public Vector3 savedTargetPosition;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit() {
            savedTargetPosition = GetRandomNavMeshPoint(Vector3.zero, 20);
            return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
            //EndAction(true);
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate() {

            var distanceFromMonsterToRandomPoint = Vector3.Distance(currentPosition.value, targetPosition.value);

            if (distanceFromMonsterToRandomPoint < 0.1f)
            {
                savedTargetPosition = GetRandomNavMeshPoint(Vector3.zero, 25);
            }

            targetPosition.value = savedTargetPosition;
        }

        public Vector3 GetRandomNavMeshPoint(Vector3 origin, float radius)
        {
            var maxAttempts = 20;
            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * radius;
                randomDirection += origin;

                if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return Vector3.zero; // Return zero if no valid point is found
        }

        //Called when the task is disabled.
        protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}