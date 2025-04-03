using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

namespace NodeCanvas.Tasks.Actions {

	public class MonsterParameterAT : ActionTask {

		public BBParameter<float> monsterCurrentSpeed;
        public float monsterDefaultSpeed;
		public float monsterSlowSpeed;

        public BBParameter<bool> isAwake;
        public BBParameter<bool> hasSeenPlayer;
        public BBParameter<bool> canSeePlayer;

        public BBParameter<bool> effectAllSeeing;
        public float effectAllSeeinMaxTime;
        public float effectAllSeeinTimer;

        public BBParameter<bool> debuffSlow;
		public float debuffSlowMaxTime;
        public float debuffSlowTimer;

        public BBParameter<bool> debuffBlind;
        public float debuffBlindMaxTime;
        public float debuffBlindTimer;

        public BBParameter<GameObject> player;
        public float visionRange;
        public float visionAngle;
        public LayerMask wallLayerMask;

        private NavMeshAgent navAgent;

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

            // Seeing Player

            if (CanSeePlayer() && canSeePlayer.value == true) // Note to self, fucntions can go inside if statements??
            {
                hasSeenPlayer.value = true;
            }
            else
            {
                hasSeenPlayer.value = false;
            }

            // Debuff Speed

            if (debuffSlow.value == true)
			{
                debuffSlowTimer += Time.deltaTime;
				navAgent.speed = monsterSlowSpeed;

                if (debuffSlowTimer >= debuffSlowMaxTime)
				{
					debuffSlow.value = false;
                }
            }
            else
            {
				debuffSlowTimer = 0;
                navAgent.speed = monsterDefaultSpeed;
            }

            // Debuff Blind

            if (debuffBlind.value == true)
            {
                debuffBlindTimer += Time.deltaTime;
                hasSeenPlayer.value = false;
                canSeePlayer.value = false;

                if (debuffBlindTimer >= debuffBlindMaxTime)
                {
                    debuffBlind.value = false;
                }
            }
            else
            {
                debuffBlindTimer = 0;
                canSeePlayer.value = true;
            }
        }

        // Note to self, never realized you can write code in a if statement like this

        private bool CanSeePlayer()
        {
            if (player.value == null)
                return false;

            Vector3 origin = agent.transform.position + Vector3.up; // Where dem eyes are
            Vector3 directionToPlayer = (player.value.transform.position - origin).normalized;
            float distanceToPlayer = Vector3.Distance(origin, player.value.transform.position);

            // Show raycast in editor
            Debug.DrawRay(origin, directionToPlayer * visionRange, Color.green);

            // Player in range
            if (distanceToPlayer > visionRange)
            {
                Debug.DrawRay(origin, directionToPlayer * visionRange, Color.red);
                return false;
            }

            // Player in fov
            float angle = Vector3.Angle(agent.transform.forward, directionToPlayer);
            if (angle > visionAngle / 2)
            {
                Debug.DrawRay(origin, directionToPlayer * visionRange, Color.yellow);
                return false;
            }

            // Wall blocks
            if (Physics.Raycast(origin, directionToPlayer, out RaycastHit hit, visionRange, wallLayerMask))
            {
                float monsterToWallDistance = hit.distance;

                if (hit.collider.gameObject == player.value)
                {
                    Debug.DrawRay(origin, directionToPlayer * distanceToPlayer, Color.green);
                    return true;
                }
                else
                {
                    // Check if the wall is closer than the player from the monster
                    if (monsterToWallDistance < distanceToPlayer)
                    {
                        Debug.DrawRay(origin, directionToPlayer * monsterToWallDistance, Color.red);
                        return false;
                    }
                }
            }

            // Player detected
            Debug.DrawRay(origin, directionToPlayer * distanceToPlayer, Color.green);
            return true;
        }

        //Called when the task is disabled.
        protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}