﻿using RecipeSchedulerApiService.Types.Inputs;

namespace RecipeSchedulerApiService.Models
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

        public string InstructionText { get; set; }

        public int InstructionNumber { get; set; }

        public bool CompareInput(InstructionModel instructionModel)
        {
            //Same thing as the recipeIngredientModel's compare method

            return InstructionText == instructionModel.InstructionText && InstructionNumber == instructionModel.InstructionNumber;
        }
    }
}