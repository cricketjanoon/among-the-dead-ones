using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveKeypad : InteractiveItem
{
    [SerializeField]
    protected Transform _door = null;

    [SerializeField]
    protected AudioCollection _keypadSound = null;

    [SerializeField]
    protected AudioCollection _doorUnlockSound = null;

    [SerializeField]
    protected int _bank = 0;

    [SerializeField]
    protected float _activationDelay = 0.0f;

    private bool _isActivated = false;

    public override string GetText()
    {
        ApplicationManager appDatabase = ApplicationManager.instance;
        if (!appDatabase) return string.Empty;

        string powerState = appDatabase.GetGameState("POWER");
        string lockdownState = appDatabase.GetGameState("LOCKDOWN");
        string accessCodeState = appDatabase.GetGameState("ACCESSCODE");

        // If we have not turned on the power
        if (string.IsNullOrEmpty(powerState) || !powerState.Equals("TRUE"))
        {
            return "Keypad : No Power";
        }
        else
        // Or we have not deactivated lockdown
        if (string.IsNullOrEmpty(lockdownState) || !lockdownState.Equals("TRUE"))
        {
            return "Keypad : Under Lockdown";
        }
        else
        // or we don't have the access code yet
        if (string.IsNullOrEmpty(accessCodeState) || !accessCodeState.Equals("TRUE"))
        {
            return "Keypad : Access Code Required";
        }

        // We have everything we need
        return "Keypad: To Unlock the door.";
    }

    public override void Activate(CharacterManager characterManager)
    {
        if (_isActivated) return;
        ApplicationManager appDatabase = ApplicationManager.instance;
        if (!appDatabase) return;

        string powerState = appDatabase.GetGameState("POWER");
        string lockdownState = appDatabase.GetGameState("LOCKDOWN");
        string accessCodeState = appDatabase.GetGameState("ACCESSCODE");

        if (string.IsNullOrEmpty(powerState) || !powerState.Equals("TRUE")) return;
        if (string.IsNullOrEmpty(lockdownState) || !lockdownState.Equals("TRUE")) return;
        if (string.IsNullOrEmpty(accessCodeState) || !accessCodeState.Equals("TRUE")) return;

        // Delay the actual animation for the desired number of seconds
        StartCoroutine(DoDelayedActivation(characterManager));

        _isActivated = true;
    }

    protected IEnumerator DoDelayedActivation(CharacterManager characterManager)
    {
        if (!_door) yield break; ;

        // Play the sound
        if (_keypadSound != null)
        {
            AudioClip clip = _keypadSound[_bank];
            if (clip)
            {
                if (AudioManager.instance)
                    AudioManager.instance.PlayOneShotSound(_keypadSound.audioGroup,
                                                            clip,
                                                            _door.position,
                                                            _keypadSound.volume,
                                                            _keypadSound.spatialBlend,
                                                            _keypadSound.priority);
            }
        }

        // Wait for the desired delay
        yield return new WaitForSeconds(_activationDelay);

        // If we have a character manager
        if (characterManager != null)
        {
            // Get the animator of the elevator and activate it animation
            Animator animator = _door.GetComponent<Animator>();
            if (animator) animator.SetTrigger("DoorOpen");

            // Play the door opening sound
            if (_keypadSound != null)
            {
                AudioClip clip = _doorUnlockSound[_bank];
                if (clip)
                {
                    if (AudioManager.instance)
                        AudioManager.instance.PlayOneShotSound(_doorUnlockSound.audioGroup,
                                                                clip,
                                                                _door.position,
                                                                _doorUnlockSound.volume,
                                                                _doorUnlockSound.spatialBlend,
                                                                _doorUnlockSound.priority);
                }
            }
        }
    }
}