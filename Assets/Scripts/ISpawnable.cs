using System;
using UnityEngine;

public interface ISpawnable<T> where T : class
{
    public event Action<T> Disabled;
}