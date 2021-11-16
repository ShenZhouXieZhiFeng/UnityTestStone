using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnCharacter : MonoBehaviour
{
    [HideInInspector]
    public Pawn mPawn;

    public uint mNetId => mPawn.mNetID;

    private void Awake()
    {
        mPawn = GetComponent<Pawn>();
    }

    public void DoOperation(OperationData operation)
    {
        switch (operation.mOperation)
        {
            case EmOperation.MOVE_FORWARD:
                mPawn.WalkForward();
                break;
            case EmOperation.MOVE_BACKGROUND:
                mPawn.WalkBackground();
                break;
            case EmOperation.MOVE_LEFT:
                mPawn.WalkLeft();
                break;
            case EmOperation.MOVE_RIGHT:
                mPawn.WalkRight();
                break;
            default:
                break;
        }
    }

    public void CallTick(float deltaTime)
    { 
        
    }
}
