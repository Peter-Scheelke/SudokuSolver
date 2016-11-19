//-----------------------------------------------------------------------
// <copyright file="BasicStrategy.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.SudokuStrategies
{
    using System.Collections.Generic;
    using Sudoku;

    /// <summary>
    /// A <see cref="BasicStrategy"/> iterates over a <see cref="SudokuPuzzle"/>
    /// an eliminates possible values from <see cref="Cell"/>s based on the values of other
    /// <see cref="Cell"/>s in their region.
    /// </summary>
    public class BasicStrategy : SudokuStrategy
    {
        /// <summary>
        /// Finds all of the <see cref="Cell"/>s in the given <see cref="SudokuPuzzle"/>
        /// that have not been solved yet
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being examined</param>
        /// <returns>The unsolved <see cref="Cell"/>s in the given <see cref="SudokuPuzzle"/></returns>
        protected override List<Cell> GetCandidateCells(SudokuPuzzle puzzle)
        {
            List<Cell> candidates = new List<Cell>();

            foreach (var row in puzzle.Rows)
            {
                foreach (Cell cell in row)
                {
                    if (!(cell.Count == 1))
                    {
                        candidates.Add(cell);
                    }
                }
            }

            return candidates;
        }

        /// <summary>
        /// Messes with the given <see cref="SudokuPuzzle"/> in order to advance it (via the
        /// <see cref="BasicStrategy"/> of just eliminating values from <see cref="Cell"/>s
        ///  based on their rows/columns
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being messed with</param>
        /// <param name="candidateCells">The <see cref="Cell"/>s that the strategy will mess with</param>
        /// <param name="candidateRegions">Not used in this case</param>
        /// <returns>Whether the <see cref="SudokuPuzzle"/> was advanced</returns>
        protected override bool MessWithPuzzle(SudokuPuzzle puzzle, List<Cell> candidateCells, List<List<Cell>> candidateRegions)
        {
            bool puzzleChanged = false;

            for (int i = 0; i < candidateCells.Count; ++i)
            {
                    if (candidateCells[i].EliminateValues())
                    {
                        puzzleChanged = true;
                    }
            }

            return puzzleChanged;
        }
    }
}
