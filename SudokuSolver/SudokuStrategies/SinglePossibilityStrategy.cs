//-----------------------------------------------------------------------
// <copyright file="SinglePossibilityStrategy.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.SudokuStrategies
{
    using System.Collections.Generic;
    using Sudoku;

    /// <summary>
    /// Finds <see cref="Cell"/>s that have not yet been solved, and tries to solve them
    /// based on the values of their regions (i.e. rows, columns, and blocks).
    /// </summary>
    public class SinglePossibilityStrategy : SudokuStrategy
    {
        /// <summary>
        /// Gets the <see cref="Cells"/> in the <see cref="SudokuPuzzle"/> that have not yet been solved
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <returns>A list of <see cref="Cell"/>s to try to solve with the <see cref="SinglePossibilityStrategy"/></returns>
        protected override List<Cell> GetCandidateCells(SudokuPuzzle puzzle)
        {
            List<Cell> candidateCells = new List<Cell>();
            foreach (var row in puzzle.Rows)
            {
                foreach (Cell cell in row)
                {
                    if (cell.Count > 1)
                    {
                        candidateCells.Add(cell);
                    }
                }
            }

            return candidateCells;
        }

        /// <summary>
        /// Attempts to advance the <see cref="SudokuPuzzle"/> by checking <see cref="Cell"/>'s regions
        /// to eliminate their possible values.
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <param name="candidateCells">The <see cref="Cell"/>s to be solved with the <see cref="SinglePossibilityStrategy"/></param>
        /// <returns>Whether the <see cref="SudokuPuzzle"/> was advanced</returns>
        protected override bool MessWithPuzzle(SudokuPuzzle puzzle, List<Cell> candidateCells)
        {
            bool puzzleAdvanced = false;
            for (int i = 0; i < candidateCells.Count; ++i)
            {
                if (this.SolveCell(candidateCells[i], puzzle))
                {
                    puzzleAdvanced = true;
                }
            }

            return puzzleAdvanced;
        }

        /// <summary>
        /// Add to the list of disallowed values all solved values of <see cref="Cell"/>s
        /// in the given region (other than the candidate <see cref="Cell"/>).
        /// </summary>
        /// <param name="candidate">The <see cref="Cell"/> being solved</param>
        /// <param name="disallowedValues">The values the <see cref="Cell"/> can't have</param>
        /// <param name="region">One of the regions containing the <see cref="Cell"/></param>
        private static void SolveRegion(Cell candidate, List<int> disallowedValues, List<Cell> region)
        {
            foreach (Cell cell in region)
            {
                if (object.ReferenceEquals(candidate, cell))
                {
                    continue;
                }
                else
                {
                    if (cell.Count == 1)
                    {
                        disallowedValues.Add(cell.AllowedValue);
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to solve the given <see cref="Cell"/> using the <see cref="SinglePossibilityStrategy"/>
        /// </summary>
        /// <param name="candidate">The <see cref="Cell"/> being solved</param>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> containing the candidate <see cref="Cell"/></param>
        /// <returns>Whether the <see cref="SudokuPuzzle"/> was advanced</returns>
        private bool SolveCell(Cell candidate, SudokuPuzzle puzzle)
        {
            List<int> disallowedValues = new List<int>();
            SolveRegion(candidate, disallowedValues, candidate.Row);
            SolveRegion(candidate, disallowedValues, candidate.Column);
            SolveRegion(candidate, disallowedValues, candidate.Block);
            bool puzzleAdvanced = false;

            foreach (int disallowedValue in disallowedValues)
            {
                if (candidate.AllowedValues.ContainsKey(disallowedValue))
                {
                    candidate.RemoveAllowedValue(disallowedValue);
                    puzzleAdvanced = true;
                }
            }

            return puzzleAdvanced;
        }
    }
}
