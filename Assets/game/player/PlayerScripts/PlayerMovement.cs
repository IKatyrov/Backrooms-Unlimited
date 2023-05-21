using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private MoveSettings _settings = null;
    public Vector3 _moveDirection;
    private float _speed;
    private float _stamina = 100.0f;
    bool _ToggleAudioChange = true;
    
    [SerializeField] private Text speedMeter;
    [SerializeField] private Slider _staminaSlider;
    private float _changeSpeed;
    private CharacterController _controller;
    private AudioManager audioManager;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Update()
    {
        StaminaCount();
        DefaultMovement();
        CheckSpeed();
        _controller.Move(_moveDirection * Time.deltaTime);   
        speedMeter.text=_changeSpeed.ToString();
    }
    private bool Sprinting()
    {
        if (!Input.GetKey(KeyCode.LeftShift)) return false;
        if (!_controller.isGrounded) return false;
     // if (PlayerInput.y <= 0) return false;
        return true;
    }
    private float TrueSpeed()
    {
        return new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;
    }
    private void CheckSpeed()
    {
         if (_changeSpeed != TrueSpeed())
        {
            _changeSpeed = TrueSpeed();
        }
    }
    private float DesiredSpeed()
    {
    //  if (Crouching()) return _settings.crouchSpeed;
        return Sprinting() ? _settings.runSpeed : _settings.walkSpeed;
    }

    private void SetSpeed()
    {
        _speed = Mathf.Lerp(_speed, DesiredSpeed(), _settings.acceleration * Time.deltaTime);
    }
    private void DefaultMovement()
    {
        if (_controller.isGrounded)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            
            if (input.x != 0 && input.y != 0)
            {
                input *= 1;
            }
            _moveDirection.y = -_settings.antiBump;
            
            if (Input.GetKey(KeyCode.LeftShift) == false || _stamina <= 5) 
            { 
                _moveDirection.x = input.x * _settings.walkSpeed;
                _moveDirection.z = input.y * _settings.walkSpeed;
            }
            if (Input.GetKey(KeyCode.LeftShift) == true && _stamina >= 5) 
            { 
                _moveDirection.x = input.x * _settings.runSpeed;
                _moveDirection.z = input.y * _settings.runSpeed;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            } 
            
            _moveDirection = transform.TransformDirection(_moveDirection);
        }
        else
        {
            _moveDirection.y -= _settings.gravity * Time.deltaTime;
        }
    }

    private void Jump()
    {
        _moveDirection.y += _settings.jumpForce;
        _stamina-=5f;
    }


    private void StaminaCount()
    {
        _staminaSlider.value = _stamina;
        //работает, и хуй с ним
        if (Sprinting()==true)
        {
            _stamina -= .1f;
        }
        else
        {
            if (_controller.isGrounded == true)
            {
                _stamina += .35f;
            }
        }
        if (_stamina >= 100)
        {
            _stamina = 100;
        }
        if (_stamina <= 0)
        {
            _stamina = 0;
        }
    }
 
}