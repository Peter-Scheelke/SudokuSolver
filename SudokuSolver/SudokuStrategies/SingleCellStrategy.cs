//-----------------------------------------------------------------------
// <copyright file="SingleCellStrategy.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.SudokuStrategies
{
    using System.Collections.Generic;
    using Sudoku;

    /// <summary>
    /// Look over all the regions of the given <see cref="SudokuPuzzle"/>.
    /// Find a possibility that has been eliminated from all except a single
    /// <see cref="Cell"/> in that region.
    /// </summary>
    public class SingleCellStrategy : SudokuStrategy
    {
        /// <summary>
        /// Converts the <see cref="SingleCellStrategy"/> to a string
        /// </summary>
        /// <returns>The string version of the <see cref="SingleCellStrategy"/></returns>
        public override string ToString()
        {
            return "Single Cell Strategy";
        }

        /// <summary>
        /// Gets the regions of the given <see cref="SudokuPuzzle"/> that are relevant
        /// to solving the <see cref="SudokuPuzzle"/>
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <returns>The regions relevant to the <see cref="SingleCellStrategy"/></returns>
        protected override List<List<Cell>> GetCandidateRegions(SudokuPuzzle puzzle)
        {
            List<List<Cell>> candidateRegions = new List<List<Cell>>();
            for (int i = 0; i < puzzle.Rows.Count; ++i)
            {
                candidateRegions.Add(puzzle.Rows[i]);
                candidateRegions.Add(puzzle.Columns[i]);
                candidateRegions.Add(puzzle.Blocks[i]);
            }

            return candidateRegions;
        }

        /// <summary>
        /// Find regions of the <see cref="SudokuPuzzle"/> that have a possibility eliminated
        /// from all <see cref="Cell"/>s except one. Set that <see cref="Cell"/>'s value equal
        /// to that value
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <param name="candidateCells">Not used in this case</param>
        /// <param name="regions">The regions that will be examined</param>
        /// <returns>Whether the <see cref="SudokuPuzzle"/> advanced</returns>
        protected override bool MessWithPuzzle(SudokuPuzzle puzzle, List<Cell> candidateCells, List<List<Cell>> regions)
        {
            bool puzzleAdvanced = false;

            for (int i = 0; i < regions.Count; ++i)
            {
                if (this.MessWithRegion(regions[i], puzzle.CellMin, puzzle.CellMax)) 
                {
                    puzzleAdvanced = true;
                }
            }

            return puzzleAdvanced;
        }

        /// <summary>
        /// Check if there is a value that has been eliminated from all except for one
        /// of the <see cref="Cell"/>s in the given region. If so, set that <see cref="Cell"/>'s
        /// value equal to that value
        /// </summary>
        /// <param name="region">The region being examined</param>
        /// <param name="cellMin">The minimum value allowed in a <see cref="Cell"/></param>
        /// <param name="cellMax">The maximum value allowed in a <see cref="Cell"/></param>
        /// <returns>Whether the region was altered</returns>
        private bool MessWithRegion(List<Cell> region, int cellMin, int cellMax)
        {
            bool regionAdvanced = false;

            for (int i = cellMin; i <= cellMax; ++i)
            {
                Cell singlePossibility = null;
                int possibleCellCount = 0;
                foreach (Cell cell in region)
                {
                    if (cell.AllowedValues.ContainsKey(i))
                    {
                        singlePossibility = cell;
                        ++possibleCellCount;
                    }
                }

                if (possibleCellCount == 1 && singlePossibility.Count != 1)
                {
                    singlePossibility.RemoveAllExcept(i);
                    regionAdvanced = true;
                }
            }

            return regionAdvanced;
        }
    }
}
