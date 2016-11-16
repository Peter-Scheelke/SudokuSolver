//-----------------------------------------------------------------------
// <copyright file="OnlyChoiceStrategy.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.SudokuStrategies
{
    using System;
    using System.Collections.Generic;
    using Sudoku;

    public class OnlyChoiceStrategy : SudokuStrategy
    {
        protected override List<Cell> GetCandidateCells(SudokuPuzzle puzzle)
        {
            throw new NotImplementedException();
        }

        protected override bool MessWithPuzzle(SudokuPuzzle puzzle, List<Cell> candidateCells)
        {
            throw new NotImplementedException();
        }
    }
}
