using System;

public interface ISpawnable<T> where T : class
{
    public event Action<T> Disabled;
}