using RecipeManagerWebApi.Types.Inputs;

namespace RecipeManagerWebApi.Types.Models
{
    public class InstructionModel
    {
        public InstructionModel() { }

        public InstructionModel(InstructionInput instructionInput)
        {
            InstructionText = instructionInput.InstructionText;
            InstructionNumber = instructionInput.InstructionNumber;
        }

        public int Id { get; set; }

        public int InstructionNumber { get; set; }

        public string InstructionText { get; set; }
    }
}
