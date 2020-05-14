namespace Numian
{
    public class AttackData
    {
        private int attackValue;
        private Stances stance;

        public AttackData(int a, Stances s)
        {
            attackValue = a;
            stance = s;
        }

        public int GetAttackValue() => attackValue;
        public Stances GetStance() => stance;
    }
}