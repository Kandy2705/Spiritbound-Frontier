using System;

[Serializable]
public struct PetLifestylePoints
{
    public int nature;
    public int battle;
    public int build;

    public int Total => nature + battle + build;

    public float NatureRatio => Total == 0 ? 0f : (float)nature / Total;
    public float BattleRatio => Total == 0 ? 0f : (float)battle / Total;
    public float BuildRatio => Total == 0 ? 0f : (float)build / Total;

    public void Add(LifestyleType type, int amount)
    {
        switch (type)
        {
            case LifestyleType.Nature:
                nature += amount;
                break;
            case LifestyleType.Battle:
                battle += amount;
                break;
            case LifestyleType.Build:
                build += amount;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown lifestyle type.");
        }
    }
}
