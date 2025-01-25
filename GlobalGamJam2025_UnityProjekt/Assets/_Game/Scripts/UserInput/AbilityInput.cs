namespace Game.Input
{
    public class AbilityInput : AUserInput
    {
        public AAbility ability;

        public AbilityInput(AAbility ability)
        {
            this.ability = ability;
        }
    }
}