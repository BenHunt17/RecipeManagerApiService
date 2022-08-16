using RecipeManagerWebApi.Types.Models;

namespace RecipeManagerWebApi.Types.DomainObjects
{
    public class Instruction
    {
        public Instruction() { }

        public Instruction(InstructionModel instructionModel)
        {
            InstructionNumber = instructionModel.InstructionNumber;
            InstructionText = instructionModel.InstructionText;
        }

        public int InstructionNumber { get; set; }

        public string InstructionText { get; set; }
    }
}
