using System;
using UnityEngine;

public class PetLifestyleTracker : MonoBehaviour
{
    [SerializeField] private PetLifestylePoints points;

    public event Action<PetLifestylePoints> PointsChanged;

    public PetLifestylePoints Points => points;

    public void AddPoints(LifestyleType type, int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        points.Add(type, amount);
        PointsChanged?.Invoke(points);
    }

    public void SetPoints(PetLifestylePoints newPoints)
    {
        points = newPoints;
        PointsChanged?.Invoke(points);
    }
}
