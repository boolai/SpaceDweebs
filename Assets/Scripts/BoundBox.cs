using UnityEngine;
using System.Collections;

namespace BoogieDownGames {


	public class BoundBox : MonoBehaviour {

		enum SetState { NONE, SLEEP, DESTROY, STOP };

		[SerializeField]
		private SetState m_state;

		void OnTriggerEnter2D(Collider2D other)
		{

			switch( m_state ) {

			case SetState.NONE:

				break;

			case SetState.DESTROY:

				DestroyObject( other.gameObject );

				break;

			case SetState.SLEEP:

				if(other.GetComponent<MoveBackground>()) {
					other.GetComponent<MoveBackground>().SetObject();
				}
				other.gameObject.SetActive(false);
				//check for rigid body

				break;

			case SetState.STOP:
				if(other.GetComponent<Rigidbody2D>()) {
					other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				}

				break;
			}
		}
		
	}
}