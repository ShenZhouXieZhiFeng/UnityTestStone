using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPawnState
{ 
    Idle,
    WalkForward,
    WalkBackground,
    WalkLeft,
    WalkRight
}

public class Pawn : MonoBehaviour
{
    public uint mNetID;
    public EPawnState mState;
    public float MOVE_SPEED = 2;

    MeshRenderer mRender;

    private void Awake()
    {
        mRender = GetComponent<MeshRenderer>();
    }

    public void SetPawn(uint id, string name, Color color)
    {
        mNetID = id;
        gameObject.name = name;
        mRender.material.color = color;
    }

    public void WalkForward()
    {
        changeState(EPawnState.WalkForward);
    }

    public void WalkBackground()
    {
        changeState(EPawnState.WalkBackground);
    }

    public void WalkLeft()
    {
        changeState(EPawnState.WalkLeft);
    }

    public void WalkRight()
    {
        changeState(EPawnState.WalkRight);
    }

    void changeState(EPawnState newState)
    {
        mState = newState;
        switch (newState)
        {
            case EPawnState.Idle:

                break;
            case EPawnState.WalkForward:

                break;
            case EPawnState.WalkBackground:

                break;
            case EPawnState.WalkLeft:

                break;
            case EPawnState.WalkRight:

                break;
        }
    }

    public void CallTick(float deltaTime)
    {
        switch (mState)
        {
            case EPawnState.Idle:
                break;
            case EPawnState.WalkForward:
                setPawnPos(transform.localPosition + transform.forward * MOVE_SPEED * deltaTime);
                break;
            case EPawnState.WalkBackground:
                setPawnPos(transform.localPosition - transform.forward * MOVE_SPEED * deltaTime);
                break;
            case EPawnState.WalkLeft:
                setPawnPos(transform.localPosition - transform.right * MOVE_SPEED * deltaTime);
                break;
            case EPawnState.WalkRight:
                setPawnPos(transform.localPosition + transform.right * MOVE_SPEED * deltaTime);
                break;
        }
    }

    void setPawnPos(Vector3 pos)
    {
        transform.position = pos;
    }
}
