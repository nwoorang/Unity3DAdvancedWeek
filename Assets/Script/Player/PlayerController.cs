using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool uiOn = false; // 플레이어 컨트롤 활성화 여부
    [SerializeField] private float buoyancyForce = 10f; // 부력의 세기
    [SerializeField] private float maxWaterHeight = 3f; // 플레이어가 잠긴 깊이 제한
    [Header("MoveSet")]
    public bool movable; // 이동 가능 여부
    public float moveSpeed; // 이동 속도
    public float jumpForce; // 점프 힘
    public int multyjump; // 멀티 점프 횟수 (0이면 멀티 점프 불가, 1이면 한 번 점프 후 다시 점프 가능)
    private float jumpTime = 0f;
    private int jumpLeft; // 남은 멀티 점프 횟수
    private Vector2 curMovementInput; // 현재 입력된 이동 벡터
    public LayerMask groundLayer; // 땅 레이어 마스크
    private bool isInWater = false;
    private float waterMoveSpeed;
    private float defaultMoveSpeed;// 원래 이동 속도 저장용
    private float sprintSpeed;



    [Header("Camera")]
    public Transform camera; // 카메라 트랜스폼
    public Transform cameraRig;
    public GameObject equipCam;
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    private float camCurX;
    public float lookSensitivity; // 카메라 회전 민감도
    private Vector2 mouseDelta; // 마우스 이동 벡터
    private bool cameraLock = false;
    public bool FirstPOV = true;

    public float playerStamina;
    private bool isSprinting;

    public bool invincible = false; // 플레이어가 무적 상태인지 여부
    public float invincibleTimer = 1.5f;
    private float invincibleTimeCounter = 0f; // 무적 시간 카운터

    public bool knockback = false; // 플레이어가 넉백 상태인지 여부
    public float knockbackTime = 0.1f;
    private float knockbackTimeCounter = 0f;

    private bool isGround;
    private bool wasGround;
    private float groundCheckTimer;
    private float groundCheckTime = 0.1f;

    public Rigidbody rb; // 플레이어의 리지드바디 컴포넌트
    public Animator animator;
    [SerializeField] private Animator hitAnim;
    private PlayerInput playerInput; // 플레이어 입력 컴포넌트

    private float _moveSoundDelay = 0.6f;
    private float _lastMoveSoundPlayTime = 0f;

    private bool isGameOver;
    private bool Die = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerInput = GetComponent<PlayerInput>();

    }
    private void Start()
    {
    //    Cursor.lockState = CursorLockMode.Locked; // 마우스커서 숨기기
        waterMoveSpeed = moveSpeed/2;
        defaultMoveSpeed = moveSpeed;
        sprintSpeed = moveSpeed * 2;
    }
    private void FixedUpdate()
    {
        if (jumpTime > 1f && isGround && !knockback)
        {
            rb.drag = 10f;
        }
        else
        {
            rb.drag = 0f;
        }
        if (isGameOver)
            return;

        if (movable)
        {
            Move();
            if (curMovementInput != Vector2.zero && isGround)
            {
                PlayWalkSound();
            }
        }
        else
        {
            if (!knockback)
            {
                rb.velocity = Vector2.zero;
            }

        }
        if (isInWater)
        {
            ApplyBuoyancy();
        }
    }
    private void ApplyBuoyancy()
    {
        // 플레이어 위치보다 아래에 물 높이가 있다고 가정할 경우, 부력 적용
        // 또는, 플레이어가 아래로 가라앉을 때만 부력 적용
        if (rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
        }
    }
    private void Update()
    {
        if (isGameOver)
        {
            rb.velocity = Vector3.zero; // 게임 오버 시 플레이어 속도 초기화
            return;
        }
        if (Die)
            return;

        if (PlayerMediator.Instance.status.health.current <= 0)
        {
            Die = true;
            Invoke("GameOver",1f);
        }


        jumpTime += Time.deltaTime;
        if (isSprinting)
        {
            if (PlayerMediator.Instance.status.stamina.current <= 0)
            {
                moveSpeed = defaultMoveSpeed;
                isSprinting = false;
                animator.SetBool("IsRunning", false);
            }
        }
        groundCheckTimer += Time.deltaTime;

        if (groundCheckTimer >= groundCheckTime)
        {
            groundCheckTimer = 0f;
            wasGround = isGround; //이전 프레임의 상태 저장
            isGround = IsGrounded(); // 현재 프레임의 상태 업데이트

            if (isGround)
            {
                jumpLeft = multyjump; // 착지 시 멀티 점프 횟수 초기화
            }

            if (!wasGround && isGround) //공중에서 착지한 경우 판별
            {
                animator.SetTrigger("Land"); // 착지 트리거 실행
            }

            if (isGround && animator.GetBool("IsJump"))
            {
                animator.SetBool("IsJump", false);
            }
            animator.SetBool("IsGround", isGround);
        }
        if (invincible)
            invincibleTimeCounter += Time.deltaTime; // 무적일때 시간증가
        if (invincibleTimeCounter >= invincibleTimer)
        {
            invincible = false; // 무적 상태 해제
            invincibleTimeCounter = 0f; // 시간 카운터 초기화
        }

        if (knockback)
        {
            knockbackTimeCounter += Time.deltaTime; // 넉백일때 시간증가
        }
        if (knockbackTimeCounter > knockbackTime)
        {
            knockback = false; // 넉백 상태 해제
            movable = true; // 이동 가능 상태로 변경
            knockbackTimeCounter = 0f; // 시간 카운터 초기화
            rb.velocity = Vector3.zero; // 넉백 후 속도 초기화
        }
        _lastMoveSoundPlayTime += Time.deltaTime;

        if(UIManager.Instance.inventoryUI.activeSelf)
        {
    //        Cursor.lockState = CursorLockMode.None; // 빌드 UI와 인벤토리 UI가 활성화 상태일 때 커서 잠금 해제
            cameraLock = true;
            uiOn = true;
        }
        

        if (!UIManager.Instance.inventoryUI.activeSelf)
        {
   //         Cursor.lockState = CursorLockMode.Locked; // 빌드 UI와 인벤토리 UI가 비활성화 상태일 때 커서 잠금
            cameraLock = false;
            uiOn = false;
        }

    }

    
    private void LateUpdate()
    {
        if (isGameOver)
        {
            return;
        }

        if (!cameraLock)
        {
          //  CameraLook();
        }
    }



    public void OnMove(InputAction.CallbackContext context) //인풋액션을 이용해서 벡터 가져오기
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();        //y축이 ws , x축이 ad
            animator.SetBool("IsMove", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
            animator.SetBool("IsMove", false);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && jumpLeft > 0 && PlayerMediator.Instance.status.stamina.current > 20)
        {
            jumpLeft--; // 점프할 때마다 멀티 점프 횟수 감소
            rb.drag = 0f;
            jumpTime = 0f;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 점프 힘을 위로 추가
            PlayerMediator.Instance.status.stamina.Substract(20); // 점프 시 스태미너 감소
            animator.SetBool("IsJump", true);    //점프상태 진입
            animator.SetTrigger("Jump");        //점프 트리거
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            moveSpeed = sprintSpeed; // 스프린트 시 이동 속도 증가
            isSprinting = true; // 스프린트 상태 설정
            animator.SetBool("IsRunning", true);
            _moveSoundDelay = 0.3f;


        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (isSprinting)
                moveSpeed = defaultMoveSpeed; // 스프린트 였으면 이동 속도 원래대로
            isSprinting = false; // 스프린트 상태 해제
            animator.SetBool("IsRunning", false);
            _moveSoundDelay = 0.6f;
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            
            InventoryToggle();
        }
    }

    public void InventoryToggle()
    {

        // 인벤토리 열기/닫기
        UIManager.Instance.playerUI.inventoryUI.ToggleInventory();

  //      if (Cursor.lockState == CursorLockMode.Locked) Cursor.lockState = CursorLockMode.None; // 인벤토리 열 때 커서 잠금 해제
        if (AssessCameraLockState() == false) cameraLock = true;
    }


    public void POV(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            TogglePOV();
        }
    }

    void CameraLook()
    {
        camCurX += mouseDelta.y * lookSensitivity;
        camCurX = Mathf.Clamp(camCurX, minXLook, maxXLook);

        if (FirstPOV)
            camera.localEulerAngles = new Vector3(-camCurX, 0, 0);  // 위 아래 카메라 반전 가능
        else
            cameraRig.localEulerAngles = new Vector3(-camCurX, 0, 0); //카메라를 담은 리그를 돌려서 3인칭으로

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    void Move()
    {
        float yVelocity = rb.velocity.y;
        if (!knockback)
        {
            rb.velocity = Vector3.zero;
        }
        rb.velocity = new Vector3(0, yVelocity, 0);
        Vector3 direction = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        direction *= moveSpeed;

        rb.velocity = new Vector3(direction.x, yVelocity, direction.z); // 리지드바디의 속도 설정
    }

    public void ToggleCursor()
    {
     //   bool isLocked = Cursor.lockState == CursorLockMode.Locked;
     //   Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
    //    cameraLock = isLocked;
    }

    public bool AssessCameraLockState()
    {
        return cameraLock; // 현재 카메라 잠금 상태 반환
    }

    public void ToggleCameraLock()
    {
        cameraLock = !cameraLock; // 카메라 잠금 상태 토글
    }

    public void CameraLockStateToFalse()
    {
        cameraLock = false; // 카메라 잠금 상태를 false로 설정
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };
        for (int i = 0; i < rays.Length; i++)
        {

            if (Physics.Raycast(rays[i], 0.7f, groundLayer))
            {

                return true;
            }
        }
        return false;
    }

    public bool IsSprinting()
    {
        if (isSprinting && rb.velocity.magnitude > 1f)   //쉬프트 누르고 이동할때
        {
            return true;
        }
        return false;
    }

    void TogglePOV()
    {
        Vector3 pos = camera.localPosition;
        FirstPOV = !FirstPOV;
        int playerLayerMask = LayerMask.GetMask("Player");  //레이어마스크는 비트마스크
        int equipLayerMask = LayerMask.GetMask("Equip");
        if (!FirstPOV)      //3인칭
        {
            pos.z = -5f;
            pos.y -= 1f;
            camera.localRotation = Quaternion.Euler(0f, 0f, 0f);
            camera.localPosition = pos;
            PlayerMediator.Instance.camera.cullingMask |= playerLayerMask | equipLayerMask;  // 플레이어 다시 보이게
            equipCam.SetActive(false); // 장비 카메라 끔

        }
        else    //1인칭
        {
            pos.z = 0f;
            pos.y += 1f;
            cameraRig.localRotation = Quaternion.Euler(0f, 0f, 0f);
            camera.localPosition = pos;
            PlayerMediator.Instance.camera.cullingMask &= ~(playerLayerMask | equipLayerMask); // 플레이어 안 보이게
            equipCam.SetActive(true); // 장비 카메라 켬
        }
    }

    public void Damaged(Transform transform, float damage)
    {
        if (invincible)
            return; // 이미 무적 상태라면 데미지 무시
        animator.SetTrigger("Damaged"); // 데미지 애니메이션 트리거
        hitAnim.SetTrigger("Hit"); // 맞았을 때 애니메이션 트리거
        Vector3 direction = (this.transform.position - transform.position); // 몬스터 → 플레이어
        direction.y = 0f; // Y축 제거로 수평 방향만 유지
        direction = direction.normalized; // 방향 정규화
        direction *= 10f;
        direction += Vector3.up * 2f; // 위로 살짝 기울임 (위 방향 조절 가능)
        invincible = true; // 무적 상태 설정
        invincibleTimeCounter = 0f; // 무적 시간 카운터 초기화
        knockback = true; // 넉백 상태 설정
        movable = false; // 이동 불가 설정
        rb.AddForce(direction, ForceMode.Impulse); // 힘 적용
        PlayerMediator.Instance.status.health.Substract(damage); // 플레이어 체력 감소
        
    }

    public void ToggleUIOn()
    {
        uiOn = !uiOn; // UI 활성화 상태 토글
    }

    public void PlayWalkSound()
    {
        if (_lastMoveSoundPlayTime > _moveSoundDelay)
        {
         StringBuilder sb = new StringBuilder();
         
          Ray ray = new Ray(transform.position, Vector3.down);
          if (Physics.Raycast(ray,out RaycastHit hit,3f, groundLayer))
          {     sb.Append("Footsteps");
                if (hit.collider.CompareTag("Rock"))
                {
                    sb.Append("Rock");
                }
                else if (hit.collider.CompareTag("Wood"))
                {
                    sb.Append("Wood");
                }
                else
                {
                    sb.Append("Grass");
                }
          }
 
          sb.Append(Random.Range(1, 5).ToString());

          _lastMoveSoundPlayTime = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;


            moveSpeed = waterMoveSpeed; // 물속에서는 이동 속도 절반
            // 물 속 효과를 위한 이펙트, 후처리 등도 추가 가능
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;

            moveSpeed = defaultMoveSpeed; // 이동 속도 원복
        }
    }
    
    

}
