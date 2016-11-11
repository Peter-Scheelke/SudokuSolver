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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class
        /// </summary>
        /// <param name="allowedValues">The possible values the <see cref="Cell"/> can have</param>
        public Cell(List<int> allowedValues)
        {
            this.allowedValues = new Dictionary<int, int>();
            if (allowedValues == null || allowedValues.Count == 0)
            {
                throw new Exception("Error: Given list of values was either null or empty.");
            }

            foreach (int value in allowedValues)
            {
                if (value <= 9 && value >= 1)
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
                else
                {
                    throw new Exception($"Error: {value} is a duplicate");
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

            if (value > 9 || value < 1)
            {
                throw new Exception($"Error: {value} is not between 1 and 9");
            }
            else
            {
                if (this.allowedValues.ContainsKey(value))
                {
                    this.allowedValues.Remove(value);
                }
                else
                {
                    throw new Exception($"Error: {value} is not an allowed value");
                }
            }
        }
    }
}