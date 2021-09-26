using UnityEngine;
using System.Collections;

public class AIDamageTrigger : MonoBehaviour
{
    // Inspector Variables

    [SerializeField]
    private string _parameter = "";

    [SerializeField]
    private int _bloodParticlesBurstAmount = 10;

    [SerializeField]
    private float _damageAmount = 0.1f;

    [SerializeField]
    private bool _doDamageSound = true;

    [SerializeField]
    private bool _doPainSound = true;

    // Private Variables

    private AIStateMachine _stateMachine = null;
    private Animator _animator = null;
    private int _parameterHash = -1;
    private GameSceneManager _gameSceneManager = null;
    private bool _firstContact = false;

    // ------------------------------------------------------------
    // Name	:	Start
    // Desc	:	Called on object start-up to initialize the script.
    // ------------------------------------------------------------
    private void Start()
    {
        // Cache state machine and animator references
        _stateMachine = transform.root.GetComponentInChildren<AIStateMachine>();

        if (_stateMachine != null)
            _animator = _stateMachine.animator;

        // Generate parameter hash for more efficient parameter lookups from the animator
        _parameterHash = Animator.StringToHash(_parameter);

        _gameSceneManager = GameSceneManager.instance;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!_animator)
            return;

        if (col.gameObject.CompareTag("Player") && _animator.GetFloat(_parameterHash) > 0.9f)
            _firstContact = true;
    }

    // -------------------------------------------------------------
    // Name	:	OnTriggerStay
    // Desc	:	Called by Unity each fixed update that THIS trigger
    //			is in contact with another.
    // -------------------------------------------------------------
    private void OnTriggerStay(Collider col)
    {
        // If we don't have an animator return
        if (!_animator)
            return;

        // If this is the player object and our parameter is set for damage
        if (col.gameObject.CompareTag("Player") && _animator.GetFloat(_parameterHash) > 0.9f)
        {
            if (GameSceneManager.instance && GameSceneManager.instance.bloodParticles)
            {
                ParticleSystem system = GameSceneManager.instance.bloodParticles;

                // Temporary Code
                system.transform.position = transform.position;
                system.transform.rotation = Camera.main.transform.rotation;

                var settings = system.main;
                settings.simulationSpace = ParticleSystemSimulationSpace.World;
                system.Emit(_bloodParticlesBurstAmount);
            }

            if (_gameSceneManager != null)
            {
                PlayerInfo info = _gameSceneManager.GetPlayerInfo(col.GetInstanceID());
                if (info != null && info.characterManager != null)
                {
                    info.characterManager.TakeDamage(_damageAmount, _doDamageSound && _firstContact, _doPainSound);
                }
            }

            _firstContact = false;
        }
    }
}