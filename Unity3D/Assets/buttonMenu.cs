using UnityEngine;
using System.Collections;

public class buttonMenu : MonoBehaviour {

	public enum Accion { Jugar, Tutorial, Creditos, Ninguna };



	public GameObject listaJugadores;

	public Accion accion = Accion.Ninguna ;
	public GameObject camara;
	private TextMesh lista_ ;
	public void Home(){
		iTween.MoveTo (camara, iTween.Hash ("x", 0, "easeType", "linear", "loopType", "none", "delay", 0, "time", .3f));
	}
	public void Credits(){
		iTween.MoveTo (camara, iTween.Hash ("x", -8.6f, "easeType", "linear", "loopType", "none", "delay", 0, "time", .3f));
	}
	public void Tutorial(){
		iTween.MoveTo (camara, iTween.Hash ("x", 9.05f, "easeType", "linear", "loopType", "none", "delay", 0, "time", .3f));

	}
	public void Play(){
		Application.LoadLevel(1);
	}

	void Start (){
		lista_  = (TextMesh)listaJugadores.transform.GetComponent(typeof(TextMesh));
		StartCoroutine(GetScores ());
	}


	IEnumerator GetScores () {

		Debug.Log ("******************");
		// Create a form object for sending high score data to the server
		WWWForm form = new WWWForm();
		// Assuming the perl script manages high scores for different games
		form.AddField( "game", "C2D6A91B-AEB9-4C04-8C62-CAC9CB97363E");
		
		// The name of the player submitting the scores
		// The score
		
		// Create a download object
		//WWW download = new WWW( "http://games.josuezarza.com/services.asmx/score", form );
		WWW download = new WWW( "http://games.josuezarza.com/services.asmx/GameScore", form );
		
		
		
		// Wait until the download is done
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			Debug.Log( "Error downloading: " + download.error );
			
		} else {
			// show the highscores
			string xml = download.text;
			Debug.Log(xml);
			string aux = "http://i-xupp.org";
			Debug.Log(aux);
			int posIni = xml.IndexOf(aux) + aux.Length;
			Debug.Log(posIni);
			int posFin = xml.IndexOf("</string>");
			Debug.Log(posFin);
			string resultado = xml.Substring(posIni + 3 , posFin - (posIni+3));
			Debug.Log(resultado);
			lista_.text = resultado.Replace("*", "\n");
		}
	}

	void None () {

		if (Input.GetMouseButton (0)) {


			switch(accion){
			case Accion.Creditos:
				iTween.MoveTo (camara, iTween.Hash ("x", 9.05f, "easeType", "linear", "loopType", "none", "delay", 0, "time", 1f));
				break;
			case Accion.Jugar:

				//Application.LoadLevel(1);
			case Accion.Tutorial:
				iTween.MoveTo (camara, iTween.Hash ("x", -8.6f, "easeType", "linear", "loopType", "none", "delay", 0, "time", 1f));
				break;
			default:
				iTween.MoveTo (camara, iTween.Hash ("x", 0, "easeType", "linear", "loopType", "none", "delay", 0, "time", 1f));
				break;
			}
		}
	
	}
}
