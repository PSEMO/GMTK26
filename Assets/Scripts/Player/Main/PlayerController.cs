using UnityEngine;
using PSEMO.Core.StateMachine;
using PSEMO.Events;
using PSEMO.Core.Persistence;

namespace PSEMO.Player
{
    public class PlayerController : MonoBehaviour, IStateMachineUser, IPersistable
    {
        public PlayerSO data;

        private PlayerInputHandler inputHandler;

        [HideInInspector] public Vector3 respawnPos;

        //Able To
        [HideInInspector] public bool ableToInteract;
        [HideInInspector] public bool ableToMove;
        [HideInInspector] public bool ableToJump;
        [HideInInspector] public bool ableToDash;

        void Awake()
        {
            inputHandler = new PlayerInputHandler(this);

            ableToInteract = data.ableToInteract;
            ableToMove = data.ableToMove;
            ableToJump = data.ableToJump;
            ableToDash = data.ableToDash;
        }

        void Start()
        {
            CameraEvents.InvokeCameraTargetAdded(transform, data.camWeight);

            respawnPos = transform.position;
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

        private void Die()
        {
            Respawn();
        }

        public void Respawn()
        {
            transform.position = respawnPos;
        }

        private void SetRespawnPos(Vector3 pos) => respawnPos = pos;

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