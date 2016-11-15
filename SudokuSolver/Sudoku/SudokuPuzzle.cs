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
                    this.rows[i][rows[i].Count - 1].Row = i;
                    this.columns[k].Add(allCells[(dimension * dimension * i) + k]);
                    this.columns[k][columns[k].Count - 1].Column = k;

                    int blockIndex = currentBlock + (k / dimension);
                    this.blocks[blockIndex].Add(allCells[(dimension * dimension * i) + k]);
                    this.blocks[blockIndex][blocks[blockIndex].Count - 1].Block = blockIndex;

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
    }
}
