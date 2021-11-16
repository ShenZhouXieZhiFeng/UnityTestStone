using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBuffer
{
    private byte[] buffer = new byte[1024];
    private int dataLength;

    private int pointer;

    public byte[] Buffer => buffer;

    public int DataLength => dataLength;

    public byte[] DataBuffer
    {
        get
        {
            byte[] b = new byte[dataLength + 4];
            Array.Copy(buffer, 0, b, 4, dataLength);
            var ddd = BitConverter.GetBytes(dataLength);
            Array.Copy(ddd, 0, b, 0, 4);
            return b;
        }
    }

    public void Set(byte[] b, int count)
    {
        pointer = 0;
        dataLength = count;
        b.CopyTo(buffer, 0);
    }

    public void Set(byte[] b, int startIndex, int count)
    {
        pointer = 0;
        dataLength = count;
        Array.Copy(b, startIndex, buffer, 0, count);
    }

    public void Reset()
    {
        pointer = 0;
        dataLength = 0;
    }

    public void ResetPointer()
    {
        pointer = 0;
    }

    public int ReadInt()
    {
        int n = BitConverter.ToInt32(Buffer, pointer);
        pointer += 4;
        return n;
    }

    public void WriteInt(int n)
    {
        var bytes = BitConverter.GetBytes(n);
        for (int i = 0; i < bytes.Length; i++)
        {
            buffer[pointer + i] = bytes[i];
        }
        pointer += bytes.Length;
        dataLength = pointer;
    }

}
