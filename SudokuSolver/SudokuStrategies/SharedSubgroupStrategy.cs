//-----------------------------------------------------------------------
// <copyright file="SharedSubgroupStrategy.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.SudokuStrategies
{
    using System;
    using System.Collections.Generic;
    using Sudoku;    

    /// <summary>
    /// The <see cref="SharedSubgroupStrategy"/> looks at a <see cref="SudokuPuzzle"/>'s blocks. For each
    /// column/row in a block, it checks if there are any values that must be in a particular row/column.
    /// It then eliminates those values from the <see cref="Cell"/>s that are not in that row/column
    /// </summary>
    public class SharedSubgroupStrategy : SudokuStrategy
    {
        /// <summary>
        /// Converts the <see cref="SharedSubgroupStrategy"/> to a string
        /// </summary>
        /// <returns>The string version of the <see cref="SharedSubgroupStrategy"/></returns>
        public override string ToString()
        {
            return "Shared Subgroup Strategy";
        }

        /// <summary>
        /// Gets a list of the regions relevant to the <see cref="SharedSubgroupStrategy"/>
        /// The relevant regions in this case are the blocks of the given <see cref="SudokuPuzzle"/>
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced by the <see cref="SharedSubgroupStrategy"/></param>
        /// <returns>A list of the regions relevant to the <see cref="SharedSubgroupStrategy"/></returns>
        protected override List<List<Cell>> GetCandidateRegions(SudokuPuzzle puzzle)
        {
            List<List<Cell>> candidates = new List<List<Cell>>();

            foreach (var block in puzzle.Blocks)
            {
                candidates.Add(block);
            }

            return candidates;
        }

        /// <summary>
        /// Advances the <see cref="SudokuPuzzle"/> by looking at the blocks of the given <see cref="SudokuPuzzle"/>
        /// It checks the rows/columns in each block to find what values must be in those rows/blocks, then eliminates
        /// those values from the other <see cref="Cell"/>s in the block.
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being advanced</param>
        /// <param name="candidateCells">Not used in this case</param>
        /// <param name="candidateRegions">The blocks of the given <see cref="SudokuPuzzle"/></param>
        /// <returns>Whether the <see cref="SharedSubgroupStrategy"/> advanced the <see cref="SudokuPuzzle"/></returns>
        protected override bool MessWithPuzzle(SudokuPuzzle puzzle, List<Cell> candidateCells, List<List<Cell>> candidateRegions)
        {
            bool puzzleAdvanced = false;
            for (int i = 0; i < candidateRegions.Count; ++i)
            {
                if (this.AdvanceBlock(candidateRegions[i], puzzle.CellMin, puzzle.CellMax))
                {
                    puzzleAdvanced = true;
                }
            }

            return puzzleAdvanced;
        }

        /// <summary>
        /// Advances the given block using the shared subgroup strategy
        /// </summary>
        /// <param name="block">The block being advanced</param>
        /// <param name="cellMin">The minimum value each <see cref="Cell"/> can have</param>
        /// <param name="cellMax">The maximum value each <see cref="Cell"/> can have</param>
        /// <returns>Whether the given block was advanced</returns>
        private bool AdvanceBlock(List<Cell> block, int cellMin, int cellMax)
        {
            bool blockAdvanced = false;
            int blockWidth = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(block.Count)));

            for (int i = 0; i < blockWidth; ++i)
            {
                if (this.AdvanceRegion(block, block[i].Row, cellMin, cellMax))
                {
                    blockAdvanced = true;
                }
            }

            for (int i = 0; i < blockWidth; ++i)
            {
                if (this.AdvanceRegion(block, block[i].Column, cellMin, cellMax))
                {
                    blockAdvanced = true;
                }
            }

            return blockAdvanced;
        }

        /// <summary>
        /// Find the values that must be in the section of the given region within the given block
        /// Use those values to eliminate values from other <see cref="Cell"/>s in the block
        /// </summary>
        /// <param name="block">The block being checked</param>
        /// <param name="region"> 
        /// The row/column whose values outside the given block are being used to advance the block
        /// </param>
        /// <param name="cellMin">The minimum value a <see cref="Cell"/> can have</param>
        /// <param name="cellMax">The maximum value a <see cref="Cell"/> can have</param>
        /// <returns>Whether the region successfully advanced the block</returns>
        private bool AdvanceRegion(List<Cell> block, List<Cell> region, int cellMin, int cellMax)
        {
            bool isRow = false;
            for (int i = 0; i < block.Count; ++i)
            {
                if (object.ReferenceEquals(block[i].Row, region))
                {
                    isRow = true;
                }
            }

            bool blockAdvanced = false;
            List<int> requiredValues = this.GetRequiredValues(region, block, cellMin, cellMax);
            foreach (int value in requiredValues)
            {
                for (int k = 0; k < block.Count; ++k)
                {
                    if (isRow)
                    {
                        if (!object.ReferenceEquals(block[k].Row, region))
                        {
                            if (block[k].AllowedValues.ContainsKey(value) && block[k].Count > 1)
                            {
                                block[k].RemoveAllowedValue(value);
                                blockAdvanced = true;
                            }
                        }
                    }
                    else
                    {
                        if (!object.ReferenceEquals(block[k].Column, region))
                        {
                            if (block[k].AllowedValues.ContainsKey(value) && block[k].Count > 1)
                            {
                                block[k].RemoveAllowedValue(value);
                                blockAdvanced = true;
                            }
                        }
                    }
                }
            }

            return blockAdvanced;
        }

        /// <summary>
        /// Gets the values that must be in the section of the given region that is in the given block
        /// </summary>
        /// <param name="region">
        /// The row/column whose values outside the given block are being removed
        /// from the list of possible values
        /// </param>
        /// <param name="block">The block whose possible values are being found</param>
        /// <param name="cellMin">The minimum value that can be in a <see cref="Cell"/></param>
        /// <param name="cellMax">The maximum value that can be in a <see cref="Cell"/></param>
        /// <returns>The possible values that must be in the section of the given region that is in the given block</returns>
        private List<int> GetRequiredValues(List<Cell> region, List<Cell> block, int cellMin, int cellMax)
        {
            List<int> possibleValues = new List<int>();
            for (int i = cellMin; i <= cellMax; ++i)
            {
                possibleValues.Add(i);
            }

            foreach (Cell cell in region)
            {
                if (!object.ReferenceEquals(cell.Block, block))
                {
                    foreach (int allowedValue in cell.AllowedValues.Keys)
                    {
                        if (possibleValues.Contains(allowedValue))
                        {
                            possibleValues.Remove(allowedValue);
                        }
                    }
                }
            }

            return possibleValues;
        }
    }
}
