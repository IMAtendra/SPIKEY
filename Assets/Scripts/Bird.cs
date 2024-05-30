using UnityEngine;

public class Bird : MonoBehaviour
{
    #region Components
    private GameController controller;
    private SoundManager music;
    private Rigidbody2D rb2d;
    private SpriteRenderer birdSPR;
    #endregion
    public static bool isDead = false;
    public static bool isAlive = true;
    public static bool isFacingRight = false;
    public float jumpForce = 3f;
    public float forwardForce = 2f;
    public float transitionTime = 0.15f;

    void Awake() => LeanTween.reset();

    void Start() => Initialize();

    private void Initialize()
    {
        // Get References to the Component
        controller = FindObjectOfType<GameController>();
        music = FindObjectOfType<SoundManager>();
        rb2d = GetComponent<Rigidbody2D>();
        birdSPR = GetComponent<SpriteRenderer>();

        controller.currentScore = 0;
        isDead = false;
        GameController.isPlaying = true;
        print("Player Dead "+ isDead);

        // To Add Startup Jump for Once
        rb2d.velocity = Vector2.up * jumpForce * 2f;
    }

    private void Update()
    {
        if (!isDead && GameController.isPlaying)
        {
            if (controller.WasTouchedOrClicked())
            {
                Jump();
                music.Play("Flap");
            }
            SetupBoundary();
            MoveBirdOnXAxis();
        }
        
        if (GameController.isPlaying == false) PingPong();
    }

    // To Manage Gravity
    public bool FallenGravity(bool value) => rb2d.simulated = value;

    #region Jump Method
    private void Jump()
    {
        if (!isDead) rb2d.velocity = Vector2.up * jumpForce;
    }
    #endregion

    #region LoopAnimation Method
    private void PingPong()
    {
        if (isDead)
        {
            rb2d.simulated = false;
            float x = transform.position.x * 0;
            float y = Mathf.PingPong(Time.time, 1f);
            float z = transform.position.z * 0;

            transform.position = new Vector3(x: (float)x,
                                             y: (float)y,
                                             z: (float)z);
        }
    }
    #endregion

    #region MoveBirdOnXAxis Method
    void MoveBirdOnXAxis()
    {
        if (isFacingRight && birdSPR && !isDead)
        {
            transform.Translate(Vector2.left * (forwardForce * Time.deltaTime));
            birdSPR.flipX = true;
        }
        else
        {
            transform.Translate(Vector2.right * (forwardForce * Time.deltaTime));
            birdSPR.flipX = false;
        }
    }
    #endregion

    #region SetupBoundary Method
    private void SetupBoundary()
    {
        var playerPos = transform.position;
        var sx = Screen.width;
        var wrld = Camera.main.ScreenToWorldPoint(new Vector3(sx, 0.0f, 0.0f));
        var half_sz = gameObject.GetComponent<Renderer>().bounds.size.x / 2;
        var dist = (wrld.x - half_sz);
        // if(playerPos.x > (wrld.x - half_sz)) isFacingRight = !isFacingRight;
        // if(playerPos.x < -(wrld.x - half_sz)) isFacingRight = !isFacingRight;

        // Whenever Player hits Left & Right Side the Edge of the Screen 
        if (playerPos.x > dist || playerPos.x < -dist)
        {
            isFacingRight = !isFacingRight;
            music.Play("Scored");
            controller.UpdateScore(1);
        }
    }
    #endregion

    #region UI Methods
    public void GamePauseEnabled()
    {
        GameController.isPlaying = false;
        rb2d.simulated = GameController.isPlaying;

        var ui = GameObject.Find("Paused Panel");
        LeanTween.scale(ui, Vector3.one, 0.15f)
                 .setDelay(.25f)
                 .setEase(LeanTweenType.easeSpring);
    }

    public void GamePauseDisabled()
    {
        GameController.isPlaying = true;
        rb2d.simulated = GameController.isPlaying;

        var ui = GameObject.Find("Paused Panel");
        LeanTween.scale(ui, Vector3.zero, 0.15f)
                 .setEase(LeanTweenType.easeSpring);
    }

    public void DisplayGameOverUI()
    {
        LeanTween.cancel(gameObject);
        var btn = GameObject.Find("PauseButton");
        var ui = GameObject.Find("Gameover Panel");

        LeanTween.scale(btn, Vector3.zero, transitionTime)
                 .setEase(LeanTweenType.easeOutElastic);

        LeanTween.scale(ui, Vector3.one, transitionTime)
                 .setDelay(.25f)
                 .setEase(LeanTweenType.easeInOutBounce);
    }

    public void CloseGameOverUI()
    {
        GameController.isPlaying = true;
        rb2d.simulated = GameController.isPlaying;

        LeanTween.cancel(gameObject);
        var ui = GameObject.Find("Gameover Panel");

        LeanTween.scale(ui, Vector3.zero, transitionTime)
                 .setEase(LeanTweenType.easeSpring);
    }
    #endregion

    #region To Add Trigger for Player
    private void OnTriggerEnter2D(Collider2D triggered)
    {
        if (triggered.CompareTag("Spikes"))
        {
            // What to do when hit Trigger
            music.Play("Die");
            // Bird Gravity Simulated is false
            FallenGravity(false);
            isDead = true;
            DisplayGameOverUI();
        }
    }
    #endregion

}