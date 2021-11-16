using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnManager : MonoBehaviour
{
    public GameObject pawnPrefab;

    public Dictionary<uint, Pawn> pawnDict = new Dictionary<uint, Pawn>();

    public Dictionary<uint, PawnCharacter> pawnCharDict = new Dictionary<uint, PawnCharacter>();

    public Dictionary<uint, PawnControllerBase> pawnControllerDict = new Dictionary<uint, PawnControllerBase>();

    uint pawnIdIndex = 1;

    public void CallGameInit()
    {
        // 创建本地玩家
        createPawn<LocalPawnController>("LocalPlayer", Color.green);
    }

    void createPawn<T>(string name, Color color) where T : PawnControllerBase, new()
    {
        GameObject localPawnObj = GameObject.Instantiate(pawnPrefab);
        // pawn
        Pawn localPawn = localPawnObj.GetComponent<Pawn>();
        localPawn.SetPawn(getNewPawnId(), name, color);
        // pawnCharcater
        PawnCharacter localPawnCharacter = localPawnObj.GetComponent<PawnCharacter>();
        addPawnCharacter(localPawnCharacter);
        // pawnController
        PawnControllerBase localPawnCtrl = new LocalPawnController();
        localPawnCtrl.SetPawnController(localPawn);
        localPawnCtrl.CallInit();
        addPawn(localPawn);
        addPawnController(localPawnCtrl);
    }

    public void CallGameStart()
    {
        
    }

    public void CallGameTick(float deltaTime)
    {
        foreach (var ctrl in pawnControllerDict.Values)
        {
            ctrl.CallTick(deltaTime);
        }
        foreach (var pawnChar in pawnCharDict.Values)
        {
            pawnChar.CallTick(deltaTime);
        }
        foreach (var pawn in pawnDict.Values)
        {
            pawn.CallTick(deltaTime);
        }
    }

    void addPawn(Pawn pawn)
    {
        pawnDict.Add(pawn.mNetID, pawn);
    }

    void addPawnCharacter(PawnCharacter pawnChar)
    {
        pawnCharDict.Add(pawnChar.mNetId, pawnChar);
    }

    public PawnCharacter getPawnCharacter(uint netId)
    {
        PawnCharacter pChar = null;
        if (pawnCharDict.TryGetValue(netId, out pChar))
        {
            return pChar;
        }
        return pChar;
    }

    void addPawnController(PawnControllerBase pawnController)
    {
        pawnControllerDict.Add(pawnController.mNetID, pawnController);
    }

    uint getNewPawnId()
    {
        return pawnIdIndex++;
    }
}
