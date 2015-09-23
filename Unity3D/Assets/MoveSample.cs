using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveSample : MonoBehaviour
{	
	int x_ = 258;
	public GameObject volante;
	public GameObject indicador;
	public GameObject fire;

	public GameObject barTime;
	public GameObject barTimeContainer;
	public GameObject barLife;
	public GameObject barLifeContainer;

	public GameObject Maira;

	public Canvas CanvasObject;
	public InputField nickname;

	public float lifePercent = 100f;
	public float timeLeft = 60.0f;
	private float timeLeftAux = 60.0f;
	public GameObject[] obstacles;


	private TextMesh textMesh;
	private float lifeBarSegment;
	private float timeBarSegment;

	public GameObject restore;

	private int score = 0;
	private bool playing;

	private int level = 0;

	private Vector3 escalaFinalMaira;
	private Vector3 escalaInicialMaira;

	int totalCollision = 0;

	void Start(){
		timeLeftAux = timeLeft;
		CanvasObject.enabled = false;
		playing = true;
		textMesh = (TextMesh)indicador.transform.GetComponent(typeof(TextMesh));

		escalaFinalMaira = new Vector3 (0.00208307f, 07681011f,00672876f);
		escalaInicialMaira = new Vector3 (0.0007911256f, 0.02917159f, 0.002555504f);

		lifeBarSegment = barLife.transform.localScale.x/lifePercent;
		timeBarSegment = barTime.transform.localScale.x/timeLeft;
		BeginLevel ();
		//iTween.MoveBy(gameObject, iTween.Hash("z", 200, "easeType", "linear", "loopType", "none", "delay", 0, "time", timeLeft));

		InvokeRepeating ("AddObstacle", 1.0f, 1f);
		InvokeRepeating ("UpdateTime", 1.0f, 1f);
		Debug.Log("begin");
		Debug.Log(lifeBarSegment);
		Debug.Log(timeBarSegment);
		CheckView ();
	}


	private void BeginLevel(){
		level += 1;
		Debug.Log ("incremento de nivel" + level);
		gameObject.transform.position = new Vector3 (254.01f, 1.26001f, 162.15f);
		float factorSpeed = timeLeftAux - (5 * level);
		Debug.Log ("velocidad" + factorSpeed);
		if (factorSpeed > 0) {
			iTween.MoveBy (gameObject, iTween.Hash ("z", 200, "easeType", "linear", "loopType", "none", "delay", 0, "time", factorSpeed, "onComplete", "BeginLevel"));
			Maira.transform.localScale = new Vector3(0,0,0);
			iTween.ScaleTo(Maira, iTween.Hash("time", factorSpeed*2, "scale", new Vector3(.0025f,.0025f,.0025f) * 1.5f, "looptype", iTween.LoopType.none));
		} else {
			End();
		}

	}




	IEnumerator Score_ () {
		Debug.Log ("******************");
		// Create a form object for sending high score data to the server
		WWWForm form = new WWWForm();
		// Assuming the perl script manages high scores for different games
		form.AddField( "score", score.ToString() );
		form.AddField( "nick", nickname.textComponent.text);

		// The name of the player submitting the scores
		// The score
		
		// Create a download object
		//WWW download = new WWW( "http://games.josuezarza.com/services.asmx/score", form );
		WWW download = new WWW( "http://games.josuezarza.com/services.asmx/RegisterScoreWithNick", form );


		
		// Wait until the download is done
		yield return download;
		
		if(!string.IsNullOrEmpty(download.error)) {
			Debug.Log( "Error downloading: " + download.error );

		} else {
			// show the highscores
			Debug.Log("-----" + download.text);
			score = 0;

		}
		CanvasObject.enabled = false;
		Application.LoadLevel (0);
	}


	void CheckView(){

		// set the desired aspect ratio (the values in this example are
		// hard-coded for 16:9, but you could make them into public
		// variables instead so you can set them at design time)
		float targetaspect = 16.0f / 9.0f;
		
		// determine the game window's current aspect ratio
		float windowaspect = (float)Screen.width / (float)Screen.height;
		
		// current viewport height should be scaled by this amount
		float scaleheight = windowaspect / targetaspect;
		
		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();
		
		// if scaled height is less than current height, add letterbox
		if (scaleheight < 1.0f)
		{  
			Rect rect = camera.rect;
			
			rect.width = 1.0f;
			rect.height = scaleheight;
			rect.x = 0;
			rect.y = (1.0f - scaleheight) / 2.0f;
			
			camera.rect = rect;
		}
		else // add pillarbox
		{
			float scalewidth = 1.0f / scaleheight;
			
			Rect rect = camera.rect;
			
			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;
			
			camera.rect = rect;
		}
	}

	void Update(){
		bool wheel = true;

		/*Vector3 s = barTime.transform.localScale;
		s.x -= .01;*/


		CheckView ();


		if (transform.position.x < 252.9) {
			transform.position += Vector3.right * (Time.deltaTime*5);
			wheel = false;
		}
		if (transform.position.x > 254.9){
			transform.position += Vector3.left * (Time.deltaTime*5);
			wheel = false;
		}


		if (Input.GetButton ("Fire1")) {
			if (Input.mousePosition.x < Screen.width / 2) {
				Move ("left", wheel);
			} else if (Input.mousePosition.x > Screen.width / 2) {
				Move ("right", wheel);
			}
		} else if (Input.GetKey (KeyCode.A)) {
			Move ("left", wheel);
		} else if (Input.GetKey (KeyCode.D)) {
			Move ("right", wheel);
		} else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			if (Input.GetTouch(0).position.x < Screen.width / 2) {
				Move ("left", wheel);
			} else if (Input.GetTouch(0).position.x > Screen.width / 2) {
				Move ("right", wheel);
			}
		}
	}
	void Move(string direction, bool wheel){
		float zAngle = volante.transform.eulerAngles.z;
		if (direction == "left") {
			transform.position += Vector3.left * (Time.deltaTime * 5);
			zAngle += 5f;
		} else if (direction == "right") {
			transform.position += Vector3.right * (Time.deltaTime*5);
			zAngle -= 5f;
		}
		if (wheel) {
			volante.transform.eulerAngles = new Vector3(0,0,zAngle);
		}
	}
	void AddObstacle(){
		float _xobs = 253f;

		switch (Random.Range (1, 4)) {
		case 1:
			_xobs = 253f;
			break;
		case 2:
			_xobs = 253.8f;
			break;
		case 3:
			_xobs = 254.2f;
			break;
		}

	
		Instantiate(obstacles[Random.Range (0, obstacles.Length)],  new Vector3(Random.Range (253f, 254.2f),1f,transform.position.z + 20), transform.rotation);



	}
	void UpdateTime(){
		if (playing) {
			timeLeft -= 1;
			score += 1;
			barTime.transform.localScale -= new Vector3 (timeBarSegment, 0, 0);

			if(score%30 == 0){
				Instantiate(restore,  new Vector3(transform.position.x,1.5f,transform.position.z + 20), transform.rotation);
			}

			if(lifePercent<=0){
				End();
			}
			/*if (timeLeft < 0) {
				textMesh.text = "You loose";
				barTime.transform.localScale = new Vector3 (.0F, barTimeContainer.transform.localScale.y, barTimeContainer.transform.localScale.z);
			}

			if (timeLeft < 1) {
				End ();
			}*/
		}
	}

	public void GuardarScore(){
		StartCoroutine (Score_ ());

	}

	void OnCollisionEnter (Collision col)
	{
		int damage = 0;
		if (col.gameObject.tag == "Restore") {
			if(lifePercent<96){
			lifePercent += 5;
			barLife.transform.localScale += new Vector3(lifeBarSegment*5,0,0);
			}
		} else {
			score -=1;
			damage = Random.Range(1, 3)*level;
			lifePercent -= damage;
			barLife.transform.localScale -= new Vector3(lifeBarSegment*damage,0,0);
		}

		//Instantiate(fire, transform.position += new Vector3(0,0,20), col.gameObject.transform.rotation);
		Destroy(col.gameObject);

		textMesh.text = lifePercent.ToString();
	}

	void End(){
		iTween.Stop ();
		textMesh.text = "TIME END";
		CanvasObject.enabled = true;
		playing = false;
		CancelInvoke ("AddObstacle");
	}
}

