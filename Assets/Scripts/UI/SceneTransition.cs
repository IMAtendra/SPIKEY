using UnityEngine;

public class SceneTransition : MonoBehaviour {
    //name of the scene you want to load
    public string scene = "Main";
	public Color loadToColor = Color.white;
	
	public void GoFade()
    {
        Initiate.Fade(scene, loadToColor, 1.0f);
        Bird.isDead = false;
    }
}
