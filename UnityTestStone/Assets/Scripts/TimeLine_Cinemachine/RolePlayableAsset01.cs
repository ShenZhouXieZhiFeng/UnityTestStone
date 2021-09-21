using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class RolePlayableAsset01 : PlayableAsset
{
    public ExposedReference<Text> talkText;
    public string talkStr;

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        PlayableRole01 rolePlayable = new PlayableRole01();
        rolePlayable.txtDialog = talkText.Resolve(graph.GetResolver());
        rolePlayable.dialogStr = talkStr;
        return ScriptPlayable<PlayableRole01>.Create(graph, rolePlayable);
    }
}
