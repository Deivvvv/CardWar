using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHiro : MonoBehaviour
{
    private int line;
    private int slot;
    private int position;
    private Stol stol;

    public void Set(int _line, int _slot, int _position, Stol _stol)
    {
        line = _line;
        slot = _slot;
        position = _position;
        stol = _stol;
    }

    public void Play()
    {
        Debug.Log("ok");
        stol.ClickHiro(line, slot, position);
    }

    public void Target()
    {
        Debug.Log("ok");
        stol.SelectTarget(line, slot, position);
    }
    public void CardLoad()
    {
        Debug.Log("ok");
        stol.UseCard(line, slot, position);
    }
}
