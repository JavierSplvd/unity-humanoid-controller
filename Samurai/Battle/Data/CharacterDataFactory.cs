namespace Numian
{
    public class CharacterDataFactory
    {
        public static CharacterData GetData(Numian.CharacterPreset preset)
        {
            CharacterData data = null;
            if (preset.Equals(CharacterPreset.EnemySamurai))
            {
                data = new CharacterDataBuilder()
                                .WithBaseAttack(10)
                                .WithBaseDefense(10)
                                .WithMaxHealth(300)
                                .WithMaxStamina(5)
                            .Build();
            }
            if (preset.Equals(CharacterPreset.Samurai))
            {
                data = new CharacterDataBuilder()
                                .WithBaseAttack(10)
                                .WithBaseDefense(10)
                                .WithMaxHealth(100)
                                .WithMaxStamina(5)
                            .Build();
            }
            return data;
        }
    }
}