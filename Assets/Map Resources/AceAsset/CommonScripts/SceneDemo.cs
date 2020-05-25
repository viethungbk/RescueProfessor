//------------------------------------------------------------
//
//	Example Code - by AceAsset
//
//  email : AceAsset@gmail.com
//
//------------------------------------------------------------

using UnityEngine;
using System.Collections;


public class SceneDemo : MonoBehaviour
{
	[System.Serializable]
	public class CharacterList
	{
		public string		m_name;
		public GameObject	m_prefab;
	};



	//--------------------
	//Static
	//--------------------
	static int				m_currentScene = 0;
	static int				m_currentCharacter = 0;
	static Vector2			m_scrollPos = Vector2.zero;
	static float			m_cameraFOV = 0.0f;


	//--------------------
	//public 
	//--------------------
	public string[]			m_sceneList;
	public CharacterList[]	m_prefabList;

	public Texture2D		m_fadeImage;


	//--------------------
	//Private 
	//--------------------

	CharControl				m_currentCharControl = null;
	GUILayoutOption			size = GUILayout.Height(30);
	string					m_characterHelp;

	//fade
	AnimationCurve			m_fadeCurve = new AnimationCurve( new Keyframe(0.0f,1.0f) , new Keyframe(1.5f,0.0f) );
	float					m_fadeTime = 0.0f;
	float					m_fadeAlpha = 0.0f;


	// Use this for initialization
	void Start()
	{
		Respawn();
	}


	// Update is called once per frame
	void Update()
	{

		//Input Key
		if( Input.GetKeyDown(KeyCode.R) == true )
		{
			Application.LoadLevel(Application.loadedLevel);
		}


		//Fade Update
		m_fadeTime += Time.smoothDeltaTime;
		m_fadeAlpha = m_fadeCurve.Evaluate(m_fadeTime);
	}


	void OnGUI()
	{
		DrawFadetexture(m_fadeAlpha);

		GUILayout.Space(10);

		if( GUILayout.Button("Reset Scene : R", GUILayout.Height(30)) == true )
		{
			Application.LoadLevel(Application.loadedLevel);
		}

		GUILayout.Space(10);


		// Scene select
		GUIChangeScene();

		// Character select
		GUIChangeCharacter();


		m_scrollPos = GUILayout.BeginScrollView(m_scrollPos);

		//Animation Buttons
		GUIAnimationButtons();

		GUILayout.EndScrollView();

		// help box
		GUIHelpText();
	}


	void OnChangeCharControl(CharControl character)
	{
		m_currentCharControl = character;

		m_characterHelp = m_currentCharControl.GetHelpText();
	}



	void FadeIn(float time)
	{
		m_fadeTime = 0.0f;
		m_fadeCurve = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(time, 0.0f));
	}


	void DrawFadetexture(float alpha)
	{
		if( alpha <= 0.0f )
			return;

		Rect r = new Rect(0, 0, Screen.width, Screen.height);
		Color backup = GUI.color;
		Color color = Color.white;
		color.a = alpha;
		GUI.color = color;
		GUI.DrawTexture(r, m_fadeImage, ScaleMode.StretchToFill, true);
		GUI.color = backup;
	}

	
	private void GUIHelpText()
	{
		int sx = 150;
		int sy = Screen.height;

		string text="";

		text += "[" + m_prefabList[m_currentCharacter].m_name + "]\n\n";

		text += "Reset : R\n";
		text += "Move : ←→↑↓\n";
		text += "Run : Shift + ←→↑↓\n\n";

		if( m_currentCharControl != null )
		{
			text += m_characterHelp;
		}

		GUILayout.BeginArea(new Rect(Screen.width - sx - 10, 10, sx, sy));

		GUILayout.BeginVertical();

		GUILayout.Box(text, GUI.skin.textField);

		GUIChangeFOV();

		GUILayout.EndVertical();

		GUILayout.EndArea();
	}

	private void GUIChangeScene()
	{
		GUIStyle	labelStyle = new GUIStyle(GUI.skin.box);
		labelStyle.alignment = TextAnchor.MiddleCenter;

		if( m_sceneList.Length > 1 )
		{
			GUILayout.BeginHorizontal();

			// "<" button
			if( GUILayout.Button("<", size) == true )
			{
				if( m_currentScene >= 1 )
				{
					m_currentScene--;
					m_currentScene  = Mathf.Clamp(m_currentScene, 0, m_sceneList.Length - 1);
					Application.LoadLevel(m_sceneList[m_currentScene]);
				}
			}


			// current character
			string text = m_sceneList[m_currentScene]+ "\n";
			text += "(" + ( m_currentScene + 1 ).ToString() + "/" + m_sceneList.Length + ")";

			GUILayout.Label(text, labelStyle, GUILayout.ExpandHeight(true));

			// ">" button
			if( GUILayout.Button(">", size) == true )
			{
				if( m_currentScene <= m_sceneList.Length - 2 )
				{
					m_currentScene ++;
					m_currentScene = Mathf.Clamp(m_currentScene, 0, m_sceneList.Length - 1);
					Application.LoadLevel(m_sceneList[m_currentScene]);
				}
			}

			GUILayout.EndHorizontal();
		}
	}


	private void GUIChangeCharacter()
	{
		GUIStyle	labelStyle = new GUIStyle(GUI.skin.box);
		labelStyle.alignment = TextAnchor.MiddleCenter;

		if( m_prefabList.Length > 1 )
		{
			GUILayout.BeginHorizontal();

			// "<" button
			if( GUILayout.Button("<", size) == true )
			{
				if( m_currentCharacter >= 1 )
				{
					m_currentCharacter--;
					m_currentCharacter = Mathf.Clamp(m_currentCharacter, 0, m_prefabList.Length - 1);
					Application.LoadLevel(Application.loadedLevel);
					return;
				}
			}


			// current character
			string text = m_prefabList[m_currentCharacter].m_name + "\n";
			text += "(" + ( m_currentCharacter + 1 ).ToString() + "/" + m_prefabList.Length + ")";

			GUILayout.Label(text, labelStyle, GUILayout.ExpandHeight(true));

			// ">" button
			if( GUILayout.Button(">", size) == true )
			{
				if( m_currentCharacter <= m_prefabList.Length - 2 )
				{
					m_currentCharacter++;
					m_currentCharacter = Mathf.Clamp(m_currentCharacter, 0, m_prefabList.Length - 1);

					Application.LoadLevel(Application.loadedLevel);
					return;
				}
			}

			GUILayout.EndHorizontal();
		}
	}


	private void GUIChangeFOV()
	{
		GUILayout.Label("FOV : " + Mathf.FloorToInt(Camera.main.fieldOfView).ToString());

		if( m_cameraFOV == 0.0f ) m_cameraFOV = Camera.main.fieldOfView;
		m_cameraFOV = GUILayout.HorizontalSlider(m_cameraFOV, 10.0f, 60.0f);
		Camera.main.fieldOfView = m_cameraFOV;
	}


	private void GUIAnimationButtons()
	{
		if( m_currentCharControl == null )	return;

		AnimationList	aniList = m_currentCharControl.GetComponent<AnimationList>();

		if( aniList == null )	return;

		foreach( string ani in aniList.m_animationList )
		{
			if( GUILayout.Button(ani, size) == true )
			{
				Spwan[] spwans = GameObject.FindObjectsOfType<Spwan>();

				if( spwans.Length == 0 )
				{
					AnimationList[] als= GameObject.FindObjectsOfType<AnimationList>();

					foreach( AnimationList al in als)
					{
						al.PlayAnimation(ani);
					}
				}
				else
				{
					foreach( Spwan spwan in spwans )
					{
						spwan.AnimationTest(ani);
					}
				}
			}
		}
	}
	
	private void Respawn()
	{
		FadeIn(1.5f);

		Spwan[] spwans = GameObject.FindObjectsOfType<Spwan>();

		foreach( Spwan spwan in spwans )
		{
			spwan.SpwanCharacter(m_prefabList[m_currentCharacter].m_prefab);
		}

	}
}
