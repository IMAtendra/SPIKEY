using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
	private float transitionTime = 3f;
    public GameObject prefabAvatar;

	void Awake() => LoadingAnimate();

    // Update is called once per frame
    void Update() => TouchHere();

	public void LoadNextLevel() 
	{
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
	}

	IEnumerator LoadLevel(int levelIndex)
	{
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(levelIndex);
	}

    private void TouchHere()
    {
        if (Input.GetMouseButtonDown(0)) LoadNextLevel();
    }

    void LoadingAnimate()
    {
        LeanTween.delayedCall(gameObject, 14f, ()=>{
			for(int i=0; i < 10; i++){
				// Instantiate Container
				GameObject rotator = new GameObject("Circle"+i);
				rotator.transform.position = new Vector3(0,0,0);

				// Instantiate Avatar
				GameObject dude = (GameObject)GameObject.Instantiate(prefabAvatar, Vector3.zero, prefabAvatar.transform.rotation );
				dude.transform.parent = rotator.transform;
				dude.transform.localPosition = new Vector3(0f,1.5f,2.5f*i);

				// Scale, pop-in
				dude.transform.localScale = new Vector3(0f,0f,0f);
				LeanTween.scale(dude, new Vector3(.4f, .4f, .4f), 1f)
                .setDelay(i * 0.2f)
                .setEase(LeanTweenType.easeOutBack);

				// Color like the rainbow
				float period = LeanTween.tau/10*i;
				float red   = Mathf.Sin(period + LeanTween.tau*0f/3f) * 0.5f + 0.5f;
	  			float green = Mathf.Sin(period + LeanTween.tau*1f/3f) * 0.5f + 0.5f;
	  			float blue  = Mathf.Sin(period + LeanTween.tau*2f/3f) * 0.5f + 0.5f;
				Color rainbowColor = new Color(red, green, blue);
				LeanTween.color(dude, rainbowColor, 0.3f).setDelay(1.2f + i*0.4f);
				
				// Push into the wheel
				LeanTween.moveZ(dude, 0f, 0.3f)
                .setDelay(1.2f + i*0.4f)
                .setEase(LeanTweenType.easeSpring)
                .setOnComplete(
					()=>{
						LeanTween.rotateAround(rotator, Vector3.forward, -1080f, 12f);
					}
				);

				// Jump Up and back down
				LeanTween.moveLocalY(dude, 2.5f, 1.2f)
                .setDelay(5f + i * 0.2f)
                .setLoopPingPong(1)
                .setEase(LeanTweenType.easeInOutQuad);
			
				// Alpha Out, and destroy
				LeanTween.alpha(dude, 0f, 0.6f)
                .setDelay(9.2f + i*0.4f)
                .setDestroyOnComplete(true)
                .setOnComplete(
					()=>{
						Destroy( rotator ); // destroying parent as well
					}
				);	
			}

		}).setOnCompleteOnStart(true).setRepeat(-1);
    }

}
