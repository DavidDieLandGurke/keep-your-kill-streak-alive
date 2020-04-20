using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCharacterController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private CircleCollider2D _coll;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector2 _mousePos;
    private Vector2 _mouseDir;
    private float _mouseAngle;

    public float speed;
    public float dashSpeed;

    private bool _isDashing;

    private float _dashingTimer = 0;
    public float dashTime;

    private float _dashingCooldownTimer = 0;
    public float dashingCooldownTime;

    public TMP_Text text;
    public TMP_Text highscoreText;

    public GameObject PauseCanvas;
    public GameObject DeathCanvas;

    [HideInInspector]
    public int KillStreak;
    [HideInInspector]
    public float KillStreakCooldown;
    public float KillStreakCooldownTime;

    public Slider CooldownSlider;

    public AudioSource DashSound;
    public AudioSource Deathsound;

    public GameObject ParticleSystem;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<CircleCollider2D>();
        CooldownSlider.minValue = -KillStreakCooldownTime;
        CooldownSlider.maxValue = 0;
    }

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _mouseDir = _mousePos - new Vector2(transform.position.x, transform.position.y);

        _mouseAngle = Mathf.Atan2(_mouseDir.y, _mouseDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, _mouseAngle);

        _dashingTimer += Time.deltaTime;
        _dashingCooldownTimer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && _dashingCooldownTimer >= dashingCooldownTime)
        {
            _isDashing = true;
            _dashingCooldownTimer = 0;
            DashSound.Play();
            Instantiate(ParticleSystem, transform.position, Quaternion.Euler(-transform.eulerAngles.z + 180, 90, 0));
        }

        if(KillStreak == 1)
        {
            text.text = "KILL!";
        }
        else if(KillStreak == 2)
        {
            text.text = "DOUBLE KILL!!";
        }
        else if(KillStreak == 3)
        {
            text.text = "TRIPLE KILL!!!";
        }
        else if(KillStreak == 4)
        {
            text.text = "QUADROUPLE KILL!!!!";
        }
        else if(KillStreak > 4)
        {
            text.text = KillStreak + " KILLS!";
        }

        if(KillStreak >= 1)
        {
            CooldownSlider.gameObject.SetActive(true);
        }
        else
        {
            CooldownSlider.gameObject.SetActive(false);
        }

        KillStreakCooldown += Time.deltaTime;

        CooldownSlider.value = -KillStreakCooldown;

        if (KillStreakCooldown > KillStreakCooldownTime)
        {
            if (KillStreak >= 1)
            {
                text.text = "";
                Time.timeScale = 0;
                DeathCanvas.SetActive(true);
                Deathsound.Play();
                this.enabled = false;
            }
        }

        if(KillStreak > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", KillStreak);
        }

        highscoreText.text = "High: " + PlayerPrefs.GetInt("Highscore", 0);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            PauseCanvas.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (_isDashing)
        {
            _coll.isTrigger = true;
            _rb.velocity = _mouseDir.normalized * dashSpeed;
            _dashingTimer = 0;
            _isDashing = false;
        }
        else
        {
            if(_dashingTimer >= dashTime)
            {
                _rb.velocity = new Vector2(_horizontalInput * speed, _verticalInput * speed);
                _coll.isTrigger = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            text.text = "";
            Time.timeScale = 0;
            DeathCanvas.SetActive(true);
            Deathsound.Play();
            this.enabled = false;
        }
    }
}
