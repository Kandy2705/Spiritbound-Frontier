using System;
using UnityEngine;

public class PetEvolutionController : MonoBehaviour
{
    [SerializeField] private PetForm currentForm = PetForm.Base;
    [SerializeField] private PetLifestyleTracker lifestyleTracker;
    [SerializeField] private int pointsThreshold = 100;
    [SerializeField] private int daysBetweenEvolution = 7;

    public event Action<PetForm> FormEvolved;

    public PetForm CurrentForm => currentForm;

    public void Initialize(PetLifestyleTracker tracker)
    {
        lifestyleTracker = tracker;
    }

    public bool CanEvolve(int dayCount)
    {
        if (lifestyleTracker == null)
        {
            return false;
        }

        bool meetsPointGate = lifestyleTracker.Points.Total >= pointsThreshold;
        bool meetsDayGate = dayCount > 0 && dayCount % daysBetweenEvolution == 0;
        return meetsPointGate || meetsDayGate;
    }

    public void TryEvolve(int dayCount)
    {
        if (!CanEvolve(dayCount))
        {
            return;
        }

        PetForm nextForm = PetEvolutionRules.DetermineNextForm(currentForm, lifestyleTracker.Points);
        if (nextForm == currentForm)
        {
            return;
        }

        currentForm = nextForm;
        FormEvolved?.Invoke(currentForm);
    }

    public void ForceEvolve()
    {
        if (lifestyleTracker == null)
        {
            return;
        }

        PetForm nextForm = PetEvolutionRules.DetermineNextForm(currentForm, lifestyleTracker.Points);
        if (nextForm == currentForm)
        {
            return;
        }

        currentForm = nextForm;
        FormEvolved?.Invoke(currentForm);
    }
}
