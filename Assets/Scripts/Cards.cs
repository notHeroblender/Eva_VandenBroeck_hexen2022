using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Cards
{
    Shoot, Swipe, Push, Teleport
}

public interface ICard
{
    Cards Type { get; }
}