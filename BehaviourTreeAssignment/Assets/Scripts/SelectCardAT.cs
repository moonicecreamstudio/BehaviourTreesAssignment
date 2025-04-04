using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class SelectCardAT : ActionTask {
        public int priorityNumber;
        public CardManager cardManager;
        public List<Cards> chosenCards;
		public float waitTimer;
		public float waitTimerMax;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {

			waitTimer = 0;
            for (int i = 0; i < cardManager.ghostHandPile.Count; i++)
            {
                if (cardManager.ghostHandPile[i].priorityType == priorityNumber)
                {
                    //Cards tempPile = cardManager.ghostHandPile[i];
                    //int chosenCard = Random.Range(0, i + 1);
                    cardManager.UpdateGhostCardSelection(i);
                }
            }

        }

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
            waitTimer += Time.deltaTime;
			if (waitTimer >= waitTimerMax)
			{
                EndAction(true);
            }

        }

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}