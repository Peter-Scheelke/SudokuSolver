//-----------------------------------------------------------------------
// <copyright file="Cell.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.Sudoku
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="Cell"/> is a single square in a sudoku puzzle
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// The minimum value allowed in a cell
        /// </summary>
        private int min;

        /// <summary>
        /// The maximum value allowed in a cell
        /// </summary>
        private int max;

        /// <summary>
        /// The values that the <see cref="Cell"/> could currently have
        /// </summary>
        private Dictionary<int, int> allowedValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class
        /// Sets the possible values of the <see cref="Cell"/> to 1 - 9
        /// </summary>
        public Cell()
        {
            this.allowedValues = new Dictionary<int, int>();
            for (int i = 1; i <= 9; ++i)
            {
                this.allowedValues.Add(i, i);
            }

            this.min = 1;
            this.max = 9;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class
        /// </summary>
        /// <param name="allowedValues">The possible values the <see cref="Cell"/> can have</param>
        /// <param name="min">The minimum value the cell can allow</param>
        /// <param name="max">The maximum value the cell can allow</param>
        public Cell(List<int> allowedValues, int min, int max)
        {
            this.min = min;
            this.max = max;
            this.allowedValues = new Dictionary<int, int>();
            if (allowedValues == null || allowedValues.Count == 0)
            {
                throw new Exception("Error: Given list of values was either null or empty.");
            }

            foreach (int value in allowedValues)
            {
                if (value <= max && value >= min)
                {
                    if (!this.allowedValues.ContainsKey(value))
                    {
                        this.allowedValues.Add(value, value);
                    }
                    else
                    {
                        throw new Exception($"Error: {value} is a duplicate");
                    }
                }
                else
                {
                    throw new Exception($"Error: {value} is not between 1 and 9");
                }
            }
        }

        /// <summary>
        /// Gets or sets the block containing the <see cref="Cell"/>
        /// </summary>
        public List<Cell> Block { get; set; }

        /// <summary>
        /// Gets or sets the column containing the <see cref="Cell"/>
        /// </summary>
        public List<Cell> Column { get; set; }

        /// <summary>
        /// Gets or sets the row containing the <see cref="Cell"/>
        /// </summary>
        public List<Cell> Row { get; set; }

        /// <summary>
        /// Gets the number of possible values the cell could have
        /// </summary>
        public int Count
        {
            get
            {
                return this.allowedValues.Count;
            }
        }

        /// <summary>
        /// Gets an allowed value from the <see cref="Cell"/>
        /// If the number of allowed values in the <see cref="Cell"/>
        /// is one, this will get the only allowed value.
        /// </summary>
        public int AllowedValue
        {
            get
            {
                foreach (int allowedValue in this.AllowedValues.Keys)
                {
                    return allowedValue;
                }

                throw new Exception("Error: Cell must have at least one allowed value.");
            }
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey, TValue}"/> containing the values the 
        /// <see cref="Cell"/> can have.
        /// </summary>
        public Dictionary<int, int> AllowedValues
        {
            get
            {
                return this.allowedValues;
            }
        }

        /// <summary>
        /// Adds the given value to the set of values allowed in the <see cref="Cell"/>
        /// </summary>
        /// <param name="value">The value being added</param>
        public void AddAllowedValue(int value)
        {
            if (value > 9 || value < 1)
            {
                throw new Exception($"Error: {value} is not between 1 and 9");
            }
            else
            {
                if (!this.allowedValues.ContainsKey(value))
                {
                    this.allowedValues.Add(value, value);
                }
            }
        }

        /// <summary>
        /// Removes the given value from the set of values allowed in the <see cref="Cell"/>
        /// </summary>
        /// <param name="value">The value being removed</param>
        public void RemoveAllowedValue(int value)
        {
            if (this.allowedValues.Count == 1)
            {
                throw new Exception($"Error: Removal of {value} would cause cell to have no allowed values.");
            }

            if (value > this.max || value < this.min)
            {
                throw new Exception($"Error: {value} is not between 1 and 9");
            }
            else
            {
                if (this.allowedValues.ContainsKey(value))
                {
                    this.allowedValues.Remove(value);
                }
            }
        }

        /// <summary>
        /// Removes all allowed values except for the given one
        /// </summary>
        /// <param name="value">The only remaining value</param>
        public void RemoveAllExcept(int value)
        {
            if (value > this.max || value < this.min)
            {
                throw new Exception($"Error: {value} is not between 1 and 9");
            }
            else
            {
                if (!this.allowedValues.ContainsKey(value))
                {
                    throw new Exception($"Error: Cell does not contain {value}");
                }
                else
                {
                    this.allowedValues.Clear();
                    this.allowedValues.Add(value, value);
                }
            }
        }

        /// <summary>
        /// Looks at all of the determined values in the <see cref="Cell"/>'s
        /// regions and eliminates them from the <see cref="Cell"/>
        /// </summary>
        /// <returns>Whether any values were eliminated</returns>
        public bool EliminateValues()
        {
            if (this.Count == 1)
            {
                return false;
            }

            bool valuesWereEliminated = false;
            List<int> valuesToEliminate = new List<int>();
            foreach (Cell cell in this.Row)
            {
                if (cell.Count == 1)
                {
                    valuesToEliminate.Add(cell.AllowedValue);
                }
            }

            foreach (Cell cell in this.Column)
            {
                if (cell.Count == 1)
                {
                    valuesToEliminate.Add(cell.AllowedValue);
                }
            }

            foreach (Cell cell in this.Block)
            {
                if (cell.Count == 1)
                {
                    valuesToEliminate.Add(cell.AllowedValue);
                }
            }

            foreach (int value in valuesToEliminate)
            {
                if (this.allowedValues.ContainsKey(value))
                {
                    this.RemoveAllowedValue(value);
                    valuesWereEliminated = true;
                }
            }

            return valuesWereEliminated;
        }
    }
}