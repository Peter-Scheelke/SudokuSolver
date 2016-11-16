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
    /// The <see cref="SinglePossibilityStrategy"/> finds <see cref="Cells"/> in blocks, rows, and columns in which
    /// the values of all other cells have been found. The final cell then gets
    /// </summary>
    public class SinglePossibilityStrategy : SudokuStrategy
    {
        /// <summary>
        /// Find all the <see cref="Cell"/>s that are in a region whose other <see cref="Cells"/>
        /// have all been solved
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being solved</param>
        /// <returns>A list of <see cref="Cell"/>s that could be solved this way</returns>
        protected override List<Cell> GetCandidateCells(SudokuPuzzle puzzle)
        {
            List<Cell> candidates = new List<Cell>();

            foreach (var row in puzzle.Rows)
            {
                CheckRegion(puzzle, candidates, row);
            }

            foreach (var column in puzzle.Columns)
            {
                CheckRegion(puzzle, candidates, column);
            }

            foreach (var block in puzzle.Blocks)
            {
                CheckRegion(puzzle, candidates, block);
            }

            return candidates;
        }

        /// <summary>
        /// Attempt to advance the <see cref="SudokuPuzzle"/> by finding regions (i.e. blocks, rows, or columns)
        /// where only one <see cref="Cell"/> has not been solved, then solving that <see cref="Cell"/>
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <param name="candidateCells">A list of <see cref="Cell"/>s, each of which is the single unsolved cell in a region</param>
        /// <returns>Whether the <see cref="SudokuPuzzle"/> ends up being advanced</returns>
        protected override bool MessWithPuzzle(SudokuPuzzle puzzle, List<Cell> candidateCells)
        {
            bool hasAdvanced = false;
            for (int i = 0; i < candidateCells.Count; ++i)
            {
                if (this.SolveCell(candidateCells[i], puzzle.GetAllAllowedValues()))
                {
                    hasAdvanced = true;
                }
            }

            return hasAdvanced;
        }

        /// <summary>
        /// Checks the region (i.e. a row, column, or block) to see if a cell is the only remaining one.
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced by the <see cref="SinglePossibilityStrategy"/></param>
        /// <param name="candidates">The <see cref="Cell"/>s that are relevant to the <see cref="SinglePossibilityStrategy"/></param>
        /// <param name="region">The row, block, or column being checked</param>
        private static void CheckRegion(SudokuPuzzle puzzle, List<Cell> candidates, List<Cell> region)
        {
            int finishedCellCount = 0;
            Cell candidate = null;
            foreach (Cell cell in region)
            {
                if (cell.Count == 1)
                {
                    ++finishedCellCount;
                }
                else
                {
                    candidate = cell;
                }
            }

            if (finishedCellCount == puzzle.Dimension * puzzle.Dimension - 1)
            {
                candidates.Add(candidate);
            }
        }

        /// <summary>
        /// Check the given region to find the correct value for the candidate <see cref="Cell"/>
        /// </summary>
        /// <param name="candidate">The <see cref="Cell"/> whose value is being found</param>
        /// <param name="allowedValues">The values a cell can have</param>
        /// <param name="region">One of the regions (i.e. row, column, or block) containing the <see cref="Cell"/></param>
        /// <returns>Whether the <see cref="Cell"/> value was found</returns>
        private static bool SolveRegion(Cell candidate, List<int> allowedValues, List<Cell> region)
        {
            foreach (Cell cell in region)
            {
                if (object.ReferenceEquals(cell, candidate))
                {
                    continue;
                }
                else
                {
                    allowedValues.Remove(cell.AllowedValue);
                }

                if (allowedValues.Count == 1)
                {
                    candidate.RemoveAllExcept(allowedValues[0]);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Find the only remaining value for the given <see cref="Cell"/>
        /// </summary>
        /// <param name="candidate">The <see cref="Cell"/> whose value is being solved</param>
        /// <param name="allowedValues">A list of the values a <see cref="Cell"/> can have</param>
        /// <returns>Whether the puzzle was advanced</returns>
        private bool SolveCell(Cell candidate, List<int> allowedValues)
        {
            if (SolveRegion(candidate, allowedValues, candidate.Row))
            {
                return true;
            }

            if (SolveRegion(candidate, allowedValues, candidate.Column))
            {
                return true;
            }

            if (SolveRegion(candidate, allowedValues, candidate.Block))
            {
                return true;
            }

            return false;
        }
    }
}
