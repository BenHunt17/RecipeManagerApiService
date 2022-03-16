using RecipeSchedulerApiService.Models;

namespace RecipeSchedulerApiService.Types
{
    public class Instruction
    {
        public Instruction(InstructionModel instructionModel)
        {
            Id = instructionModel.Id;
            InstructionText = instructionModel.InstructionText;
            InstructionNumber = instructionModel.InstructionNumber;
        }

        public int Id { get; set; }

        public string InstructionText { get; set; }

        public int InstructionNumber { get; set; }
    }
}
