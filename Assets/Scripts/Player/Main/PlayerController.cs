using UnityEngine;
using PSEMO.Core.StateMachine;
using PSEMO.Events;
using PSEMO.Core.Persistence;

namespace PSEMO.Player
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(Collider))]
    public class PlayerController : MonoBehaviour, IStateMachineUser, IPersistable
    {
        public PlayerSO data;

        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public Collider col;
        [HideInInspector] public Animator animator;

        private PlayerInputHandler inputHandler;
        private PlayerSurfaceDetector surfaceDetector;
        private PlayerStateMachineController stateController;

        [HideInInspector] public Vector3 respawnPos;

        //Input
        [HideInInspector] public float MovementInput;
        [HideInInspector] public bool JumpInput;
        [HideInInspector] public bool DashInput;

        //Move
        [HideInInspector] public Vector3 initialScale;
        [HideInInspector] public Vector3 facingDirection = Vector3.forward;

        //Jump
        [HideInInspector] public bool isGrounded = true;
        [HideInInspector] public float coyoteTimeCounter = 0;
        [HideInInspector] public float jumpBufferCounter = 0;
        [HideInInspector] public int jumpsLeft = 0;
        [HideInInspector] public bool hasJumped = false;

        //Dash
        [HideInInspector] public bool canDash = true;

        //Able To
        [HideInInspector] public bool ableToInteract;
        [HideInInspector] public bool ableToMove;
        [HideInInspector] public bool ableToJump;
        [HideInInspector] public bool ableToDash;
        [HideInInspector] public int maxJumpCount;

        //Anim variables
        private static readonly int RunSpeedHash = Animator.StringToHash("RunSpeed");

        void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();

            surfaceDetector = new PlayerSurfaceDetector(col, data);
            inputHandler = new PlayerInputHandler(this);
            stateController = new PlayerStateMachineController(this, animator);

            ableToInteract = data.ableToInteract;
            ableToMove = data.ableToMove;
            ableToJump = data.ableToJump;
            ableToDash = data.ableToDash;
            maxJumpCount = data.maxJumpCount;
        }

        void Start()
        {
            CameraEvents.InvokeCameraTargetAdded(transform, data.camWeight);

            respawnPos = transform.position;
            jumpsLeft = maxJumpCount;
            initialScale = transform.localScale;
        }

        void OnEnable()
        {
            inputHandler.OnEnable();
            PlayerEvents.OnPlayerDeath += Die;
            PlayerEvents.OnCheckPointReached += SetRespawnPos;
        }

        void OnDisable()
        {
            inputHandler.OnDisable();
            PlayerEvents.OnPlayerDeath -= Die;
            PlayerEvents.OnCheckPointReached -= SetRespawnPos;
        }

        void OnDestroy()
        {
            inputHandler.OnDestroy();
            CameraEvents.InvokeCameraTargetRemoved(transform);
        }

        void Update()
        {
            isGrounded = surfaceDetector.IsOnGround(col.bounds.center);
            UpdateTimers();

            stateController.Update();
        }

        void FixedUpdate()
        {
            stateController.FixedUpdate();
        }

        private void UpdateTimers()
        {
            if (isGrounded)
            {
                coyoteTimeCounter = data.coyoteJump;
                if (rb.linearVelocity.y <= 0.1f)
                {
                    hasJumped = false;
                    jumpsLeft = maxJumpCount;
                    canDash = true;
                }
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;

                if (coyoteTimeCounter <= 0f && !hasJumped && jumpsLeft == maxJumpCount)
                {
                    jumpsLeft--;
                }
            }

            if (jumpBufferCounter > 0f)
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if (JumpInput)
            {
                jumpBufferCounter = data.jumpBuffer;
            }
        }

        public bool IsFacingWall() => surfaceDetector.IsFacingWall(col.bounds.center, facingDirection);

        public virtual void Run()
        {
            float targetSpeed = MovementInput * data.speed;
            
            rb.linearVelocity = new(MovementInput * data.speed, rb.linearVelocity.y, 0f);

            if (MovementInput * MovementInput > 0.01f)
            {
                facingDirection = new Vector3(MovementInput, 0f, 0f).normalized;
                transform.forward = facingDirection;
            }
            
            //animator.SetFloat(RunSpeedHash, targetSpeed / data.speed);
        }

        private void Die()
        {
            Respawn();
        }

        public void Respawn()
        {
            transform.position = respawnPos;
            rb.linearVelocity = Vector3.zero;

            MovementInput = 0f;
            DashInput = false;
            JumpInput = false;
        
            stateController.SetState(new IdleState(this, animator)); 
        }

        private void SetRespawnPos(Vector3 pos) => respawnPos = pos;
        public void SetMaxJumpCount(int newCount) => maxJumpCount = newCount;

        public void EnableAbility(AbilityType type)
        {
            switch (type)
            {
                case AbilityType.Interact:
                    ableToInteract = true;
                    break;
                case AbilityType.Move:
                    ableToMove = true;
                    break;
                case AbilityType.Jump:
                    ableToJump = true;
                    break;
                case AbilityType.Dash:
                    ableToDash = true;
                    break;
            }
        }

        //====== PERSISTENCE ======
        public void LoadData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData)) return;

            PlayerSaveData saveData = JsonUtility.FromJson<PlayerSaveData>(jsonData);
            
            transform.position = saveData.playerPosition;
            respawnPos = saveData.playerRespawnPosition;
            ableToInteract = saveData.ableToInteract;
            ableToMove = saveData.ableToMove;
            ableToJump = saveData.ableToJump;
            ableToDash = saveData.ableToDash;
        }

        public string SaveData()
        {
            PlayerSaveData data = new()
            {
                playerPosition = transform.position,
                playerRespawnPosition = respawnPos,
                ableToInteract = ableToInteract,
                ableToMove = ableToMove,
                ableToJump = ableToJump,
                ableToDash = ableToDash
            };
            return JsonUtility.ToJson(data);
        }
        //=========================
    }
}