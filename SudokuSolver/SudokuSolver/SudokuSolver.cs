//-----------------------------------------------------------------------
// <copyright file="SudokuSolver.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.SudokuSolver
{
    using System;
    using System.Collections.Generic;
    using Sudoku;
    using SudokuStrategies;

    /// <summary>
    /// Solves <see cref="SudokuPuzzle"/>s using the various <see cref="SudokuStrategy"/>s
    /// </summary>
    public class SudokuSolver
    {
        /// <summary>
        /// The <see cref="SudokuStrategy"/>s that the <see cref="SudokuSolver"/> will
        /// use to solve <see cref="SudokuPuzzle"/>s
        /// </summary>
        private List<SudokuStrategy> strategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuSolver"/> class
        /// </summary>
        public SudokuSolver()
        {
            this.strategies = new List<SudokuStrategy>();
            this.strategies.Add(new BasicStrategy());
            this.strategies.Add(new SingleCellStrategy());
            this.strategies.Add(new SharedSubgroupStrategy());
        }

        /// <summary>
        /// Solves the given <see cref="SudokuPuzzle"/>
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being solved</param>
        /// <returns>A list of <see cref="SudokuPuzzle"/>s that are solutions to the given <see cref="SudokuPuzzle"/></returns>
        public List<SudokuPuzzle> Solve(SudokuPuzzle puzzle)
        {
            List<SudokuPuzzle> solutions = new List<SudokuPuzzle>();
            Stack<SudokuPuzzle> puzzleStack = new Stack<SudokuPuzzle>();
            puzzleStack.Push(puzzle);

            while (puzzleStack.Count > 0)
            {
                SudokuPuzzle puzzleToSolve = puzzleStack.Pop();
                if (puzzle.IsValid())
                {
                    bool puzzleAdvanced = false;
                    do
                    {
                        puzzleAdvanced = false;
                        for (int i = 0; i < this.strategies.Count; ++i)
                        {
                            while (this.strategies[i].AdvancePuzzle(puzzleToSolve))
                            {
                                puzzleAdvanced = true;
                            }
                        }
                    }
                    while (puzzleAdvanced);

                    if (puzzleToSolve.IsSolved())
                    {
                        solutions.Add(puzzleToSolve);
                        if (solutions.Count > 1)
                        {
                            break;
                        }
                    }
                    else
                    {
                        foreach (SudokuPuzzle guess in this.Guess(puzzleToSolve))
                        {
                            puzzleStack.Push(guess);
                        }
                    }
                }
            }

            return solutions;
        }

        /// <summary>
        /// Finds a <see cref="Cell"/> with the least possible values (other than solved <see cref="Cell"/>s),
        /// and tries each possible puzzle using those values in that cell
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being solved</param>
        /// <returns>The puzzle solutions found from guessing</returns>
        private List<SudokuPuzzle> Guess(SudokuPuzzle puzzle)
        {
            Cell cellToGuessOn = null;
            int x = -1;
            int y = -1;

            for (int i = 0; i < puzzle.Rows.Count; ++i)
            {
                for (int k = 0; k < puzzle.Rows[i].Count; ++k)
                {
                    Cell cell = puzzle.Rows[i][k];
                    if (cell.Count > 1)
                    {
                        if (cellToGuessOn == null)
                        {
                            cellToGuessOn = cell;
                            x = i;
                            y = k;
                        }
                        else if (cellToGuessOn.Count > cell.Count)
                        {
                            cellToGuessOn = cell;
                            x = i;
                            y = k;
                        }
                    }
                }
            }

            List<SudokuPuzzle> guesses = new List<SudokuPuzzle>();

            if (cellToGuessOn != null)
            {
                foreach (int allowedValue in cellToGuessOn.AllowedValues.Keys)
                {
                    SudokuPuzzle guess = puzzle.Clone();
                    guess.Rows[x][y].RemoveAllExcept(allowedValue);
                    guesses.Add(guess);
                }
            }

            return guesses;
        }
    }
}
