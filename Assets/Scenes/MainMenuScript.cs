using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private void Start()
    {
		/*int x = PlayerPrefs.GetInt("level",1);

		if (x >= SceneManager.sceneCountInBuildSettings - 1)
		{
			x = Random.Range(1, SceneManager.sceneCountInBuildSettings);
		}

		SceneManager.LoadScene(x);*/
		
		
		
		
		if (!PlayerPrefs.HasKey("lastBuildIndex"))
		{
			PlayerPrefs.SetInt("lastBuildIndex", 1);
			PlayerPrefs.SetInt("levelNo", 1);
		}

		int x = PlayerPrefs.GetInt("lastBuildIndex");

		SceneManager.LoadScene(x);
		/*
		if(AudioManager.instance)
			AudioManager.instance.Play("BG");*/
    }
}
