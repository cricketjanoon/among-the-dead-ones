using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnEnter : StateMachineBehaviour
{
    [SerializeField]
    private AudioCollection _audioCollection = null;

    [SerializeField]
    private int _bank = 0;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (AudioManager.instance == null || _audioCollection == null) return;

        AudioManager.instance.PlayOneShotSound(_audioCollection.audioGroup, _audioCollection[_bank], animator.transform.position, _audioCollection.volume, _audioCollection.spatialBlend, _audioCollection.priority);
    }
}