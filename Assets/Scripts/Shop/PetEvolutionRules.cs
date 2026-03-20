public static class PetEvolutionRules
{
    public const float PureThreshold = 0.5f;
    public const float HybridThreshold = 0.35f;

    public static PetForm DetermineNextForm(PetForm currentForm, PetLifestylePoints points)
    {
        if (points.Total == 0)
        {
            return currentForm;
        }

        float nature = points.NatureRatio;
        float battle = points.BattleRatio;
        float build = points.BuildRatio;

        bool natureHybrid = nature >= HybridThreshold;
        bool battleHybrid = battle >= HybridThreshold;
        bool buildHybrid = build >= HybridThreshold;

        if (natureHybrid && battleHybrid)
        {
            return PetForm.Druid;
        }

        if (battleHybrid && buildHybrid)
        {
            return PetForm.Guardian;
        }

        if (natureHybrid && buildHybrid)
        {
            return PetForm.Architect;
        }

        if (nature >= PureThreshold)
        {
            return currentForm == PetForm.Nature ? PetForm.Bloom : PetForm.Nature;
        }

        if (battle >= PureThreshold)
        {
            return currentForm == PetForm.Battle ? PetForm.Berserker : PetForm.Battle;
        }

        if (build >= PureThreshold)
        {
            return currentForm == PetForm.Build ? PetForm.Sentinel : PetForm.Build;
        }

        return currentForm;
    }

    public static LifestyleType GetDominantLifestyle(PetLifestylePoints points)
    {
        int nature = points.nature;
        int battle = points.battle;
        int build = points.build;

        if (nature >= battle && nature >= build)
        {
            return LifestyleType.Nature;
        }

        if (battle >= nature && battle >= build)
        {
            return LifestyleType.Battle;
        }

        return LifestyleType.Build;
    }
}
