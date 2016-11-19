//-----------------------------------------------------------------------
// <copyright file="SudokuPuzzle.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.Sudoku
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains a set of rows, columns, and blocks containing a sudoku puzzle.
    /// </summary>
    public class SudokuPuzzle
    {
        /// <summary>
        /// The sudoku puzzle's rows
        /// </summary>
        private List<List<Cell>> rows;

        /// <summary>
        /// The sudoku puzzle's columns
        /// </summary>
        private List<List<Cell>> columns;

        /// <summary>
        /// The sudoku puzzle's blocks
        /// </summary>
        private List<List<Cell>> blocks;

        /// <summary>
        /// The minimum value each cell can contain
        /// </summary>
        private int cellMin;

        /// <summary>
        /// The maximum value each cell can contain
        /// </summary>
        private int cellMax;

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuPuzzle"/> class
        /// </summary>
        /// <param name="dimension">
        /// The puzzle will be this many blocks wide/tall. Each block will be
        /// this many cells wide/tall
        /// </param>
        /// <param name="cellValues">
        /// A list of the values of the cells in the puzzle
        /// (0 means that no particular value is assigned)
        /// </param>
        public SudokuPuzzle(int dimension, List<int> cellValues)
        {
            this.Dimension = dimension;
            this.cellMin = 1;
            this.cellMax = dimension * dimension;

            this.rows = new List<List<Cell>>();
            this.columns = new List<List<Cell>>();
            this.blocks = new List<List<Cell>>();
            for (int i = 0; i < this.Dimension * this.Dimension; ++i)
            {
                this.rows.Add(new List<Cell>());
                this.columns.Add(new List<Cell>());
                this.blocks.Add(new List<Cell>());
            }

            int cellCount = dimension * dimension * dimension * dimension;
            if (cellValues.Count != cellCount)
            {
                throw new Exception($"Error: List of values has length {cellValues.Count}. Puzzle has {cellCount} cells");
            }

            List<Cell> allCells = new List<Cell>();
            List<int> allowedValues = new List<int>();
            for (int i = this.cellMin; i <= this.cellMax; ++i)
            {
                allowedValues.Add(i);
            }

            foreach (int value in cellValues)
            {
                if (value > this.cellMax || value < 0)
                {
                    throw new Exception($"Error: List contained cell value {value}. Not in range 0 - {this.cellMax}");
                }
                else
                {
                    Cell cell = new Cell(allowedValues, this.cellMin, this.cellMax);
                    if (value != 0)
                    {
                        cell.RemoveAllExcept(value);
                    }

                    allCells.Add(cell);
                }
            }

            int currentBlock = 0;
            for (int i = 0; i < dimension * dimension; ++i)
            {
                for (int k = 0; k < dimension * dimension; ++k)
                {
                    this.rows[i].Add(allCells[(dimension * dimension * i) + k]);
                    this.rows[i][this.rows[i].Count - 1].Row = this.rows[i];
                    this.columns[k].Add(allCells[(dimension * dimension * i) + k]);
                    this.columns[k][this.columns[k].Count - 1].Column = this.columns[k];

                    int blockIndex = currentBlock + (k / dimension);
                    this.blocks[blockIndex].Add(allCells[(dimension * dimension * i) + k]);
                    this.blocks[blockIndex][this.blocks[blockIndex].Count - 1].Block = this.blocks[blockIndex];
                }

                if ((i + 1) % dimension == 0 && i != 0)
                {
                    currentBlock += dimension;
                }
            }
        }

        /// <summary>
        /// Gets the width/height of each block in the puzzle
        /// </summary>
        public int Dimension { get; private set; }

        /// <summary>
        /// Gets or sets the horizontal rows of the puzzle
        /// </summary>
        public List<List<Cell>> Rows
        {
            get
            {
                return this.rows;
            }

            set
            {
                this.rows = value;
            }
        }

        /// <summary>
        /// Gets or sets the vertical columns of the puzzle
        /// </summary>
        public List<List<Cell>> Columns
        {
            get
            {
                return this.columns;
            }

            set
            {
                this.columns = value;
            }
        }

        /// <summary>
        /// Gets or sets the square blocks of the puzzle
        /// </summary>
        public List<List<Cell>> Blocks
        {
            get
            {
                return this.blocks;
            }

            set
            {
                this.blocks = value;
            }
        }

        /// <summary>
        /// Gets the minimum value a <see cref="Cell"/> in the
        /// <see cref="SudokuPuzzle"/> can have
        /// </summary>
        public int CellMin
        {
            get
            {
                return this.cellMin;
            }
        }

        /// <summary>
        /// Gets the maximum value a <see cref="Cell"/> in the
        /// <see cref="SudokuPuzzle"/> can have
        /// </summary>
        public int CellMax
        {
            get
            {
                return this.cellMax;
            }
        }

        /// <summary>
        /// Creates a copy of the <see cref="SudokuPuzzle"/>
        /// </summary>
        /// <returns>A copy of the <see cref="SudokuPuzzle"/></returns>
        public SudokuPuzzle Clone()
        {
            List<int> cellValues = new List<int>();

            for (int i = 0; i < this.Dimension * this.Dimension * this.Dimension * this.Dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle newPuzzle = new SudokuPuzzle(this.Dimension, cellValues);

            for (int i = 0; i < this.Dimension * this.Dimension; ++i)
            {
                for (int k = 0; k < this.Dimension * this.Dimension; ++k)
                {
                    this.SetCellAllowedValues(this.rows[i][k], newPuzzle.rows[i][k]);
                }
            }

            return newPuzzle;                   
        }

        /// <summary>
        /// Creates a list of all the possible values a <see cref="Cell"/> could have
        /// </summary>
        /// <returns>All the possible values a <see cref="Cell"/> could have</returns>
        public List<int> GetAllAllowedValues()
        {
            List<int> allowedValues = new List<int>();
            for (int i = this.cellMin; i <= this.cellMax; ++i)
            {
                allowedValues.Add(i);
            }

            return allowedValues;
        }

        /// <summary>
        /// Refreshes the <see cref="SudokuPuzzle"/> by calling each <see cref="Cell"/>'s
        /// EliminateValues method until the puzzle no longer progresses
        /// </summary>
        /// <returns>Whether the <see cref="SudokuPuzzle"/> progressed</returns>
        public bool Refresh()
        {
            bool puzzleChanged = false;
            bool puzzleChangedDuringLastIteration = true;

            while (puzzleChangedDuringLastIteration)
            {
                puzzleChangedDuringLastIteration = false;
                for (int i = 0; i < this.Rows.Count; ++i)
                {
                    for (int k = 0; k < this.Rows.Count; ++k)
                    {
                        if (this.Rows[i][k].EliminateValues())
                        {
                            puzzleChanged = true;
                            puzzleChangedDuringLastIteration = true;
                        }
                    }
                }
            }

            return puzzleChanged;
        }

        /// <summary>
        /// Checks whether the <see cref="SudokuPuzzle"/> has any value occurring multiple
        /// times in the same block, row, or column
        /// </summary>
        /// <returns>Whether the <see cref="SudokuPuzzle"/> is a valid puzzle</returns>
        public bool IsValid()
        {
            foreach (var row in this.rows)
            {
                if (!this.RegionIsValid(row))
                {
                    return false;
                }
            }

            foreach (var column in this.columns)
            {
                if (!this.RegionIsValid(column))
                {
                    return false;
                }
            }

            foreach (var block in this.blocks)
            {
                if (!this.RegionIsValid(block))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether the <see cref="SudokuPuzzle"/> has been solved
        /// </summary>
        /// <returns>Whether the <see cref="SudokuPuzzle"/> has been solved</returns>
        public bool IsSolved()
        {
            foreach (var row in this.Rows)
            {
                foreach (Cell cell in row)
                {
                    if (cell.Count > 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether the given region (i.e. a row, column, or block) is valid
        /// </summary>
        /// <param name="region">The region being tested</param>
        /// <returns>Whether the given region is valid</returns>
        private bool RegionIsValid(List<Cell> region)
        {
            Dictionary<int, int> testDictionary = new Dictionary<int, int>();
            foreach (Cell cell in region)
            {
                if (cell.Count == 1)
                {
                    if (testDictionary.ContainsKey(cell.AllowedValue))
                    {
                        return false;
                    }
                    else
                    {
                        testDictionary.Add(cell.AllowedValue, cell.AllowedValue);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Makes the allowed values in each <see cref="Cell"/> match
        /// </summary>
        /// <param name="sourceCell">The <see cref="Cell"/> whose allowed values will be matched</param>
        /// <param name="otherCell">The <see cref="Cell"/> whose allowed values will change</param>
        private void SetCellAllowedValues(Cell sourceCell, Cell otherCell)
        {
            if (sourceCell.Count == 1)
            {
                otherCell.RemoveAllExcept(sourceCell.AllowedValue);
            }
            else
            {
                for (int i = this.cellMin; i <= this.cellMax; ++i)
                {
                    if (!sourceCell.AllowedValues.ContainsKey(i))
                    {
                        otherCell.RemoveAllowedValue(i);
                    }
                }
            }
        }
    }
}
