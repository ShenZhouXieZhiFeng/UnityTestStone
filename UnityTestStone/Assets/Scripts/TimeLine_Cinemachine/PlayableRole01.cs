using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

// A behaviour that is attached to a playable
public class PlayableRole01 : PlayableBehaviour
{

    [Header("Dialog")]
    public ExposedReference<Text> dialog;
    public Text txtDialog;
    [Multiline(3)]
    public string dialogStr;


    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {

    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // Called when the state of the playable is set to Play
    //public override void OnBehaviourPlay(Playable playable, FrameData info)
    //{
    //    txtDialog.gameObject.SetActive(true);
    //    txtDialog.text = dialogStr;
    //}

    //// Called when the state of the playable is set to Paused
    //public override void OnBehaviourPause(Playable playable, FrameData info)
    //{
    //    if (txtDialog)
    //    {
    //        txtDialog.gameObject.SetActive(false);
    //    }
    //}

    // Called each frame while the state is set to Play
    //public override void PrepareFrame(Playable playable, FrameData info)
    //{
        
    //}
}
