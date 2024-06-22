using System;

public interface ICounter
{
    event Action<int> ChangedActive;
    event Action<int> ChangedTotal;

    string Name { get; }
}