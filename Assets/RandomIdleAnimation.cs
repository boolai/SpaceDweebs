using UnityEngine;
using System.Collections;

namespace BoogieDownGames {

	public class RandomIdleAnimation : MonoBehaviour {

		public Animator Anim;
		public float RandomCounter;
		public int RanNumb;

		// Use this for initialization
		void Start () {

			NotificationCenter.DefaultCenter.AddObserver(this, "SpawnRandom");

			if (GetComponent<Tile>() != null)
				Anim = GetComponent<Tile>().Body.GetComponent<Animator>();
			else
				Anim = this.GetComponent<Animator>();
			Randomize ();

		}

		void Randomize()
		{
			RandomCounter = 0;
			RanNumb = Random.Range (1, 7);
		}

		// Update is called once per frame
		void Update () {
		
			RandomCounter += Time.deltaTime;
			if (RandomCounter >= RanNumb)
			{
				Anim.SetTrigger("Idle");
				Randomize();
			}

		}

		public void SpawnRandom ()
		{
			//Debug.Log ("turning faster");
			Anim.SetTrigger("GenerateDweeb");
		}
	}
}