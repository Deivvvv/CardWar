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
        stol.ClickHiro(line, slot, position);
    }

    public void Target()
    {
        stol.SelectTarget(line, slot, position);
    }
    public void CardLoad()
    {
        stol.UseCard(line, slot, position);
    }

    public void PreView()
    {
        stol.PreView(line, slot, position);
    }
}
