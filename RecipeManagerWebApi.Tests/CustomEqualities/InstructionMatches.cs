using RecipeManagerWebApi.Types.DomainObjects;

namespace RecipeManagerWebApi.Tests.CustomEqualities
{
    public static class InstructionMatches
    {
        public static bool Matches(this Instruction instruction, Instruction other)
        {
            return
                instruction.InstructionNumber == other.InstructionNumber &&
                instruction.InstructionText == other.InstructionText;
        }
    }
}
