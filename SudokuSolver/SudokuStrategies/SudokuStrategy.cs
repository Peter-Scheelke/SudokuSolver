//-----------------------------------------------------------------------
// <copyright file="SudokuStrategy.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.SudokuStrategies
{
    using System.Collections.Generic;
    using Sudoku;
    
    /// <summary>
    /// An abstract class that defines a template method for advancing a <see cref="SudokuPuzzle"/>
    /// </summary>
    public abstract class SudokuStrategy
    {
        /// <summary>
        /// Attempts to advance the given <see cref="SudokuPuzzle"/> toward a solution
        /// Returns whether the puzzle was advanced
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <returns>Whether the puzzle was advanced</returns>
        public bool AdvancePuzzle(SudokuPuzzle puzzle)
        {
            List<Cell> candidateCells = this.GetCandidateCells(puzzle);
            return this.MessWithPuzzle(puzzle, candidateCells);
        }

        /// <summary>
        /// Finds the cells that should be messed with in order to advance the <see cref="SudokuPuzzle"/>
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <returns>The cells that should be messed with in order to advance the <see cref="SudokuPuzzle"/></returns>
        protected abstract List<Cell> GetCandidateCells(SudokuPuzzle puzzle);

        /// <summary>
        /// Messes with the given <see cref="SudokuPuzzle"/> to try to advance it toward a solution.
        /// Returns whether the puzzle gets advanced
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <param name="candidateCells">The <see cref="Cell"/>s in the puzzle that are relevant to advancing the puzzle</param>
        /// <returns>Whether the puzzle advances</returns>
        protected abstract bool MessWithPuzzle(SudokuPuzzle puzzle, List<Cell> candidateCells);
    }
}
