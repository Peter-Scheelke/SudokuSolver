//-----------------------------------------------------------------------
// <copyright file="SudokuSolver.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.SudokuSolver
{
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
            SudokuPuzzle puzzleToSolve = puzzle.Clone();
            List<SudokuPuzzle> solutions = new List<SudokuPuzzle>();
            List<int> strategyUseCounts = new List<int>();
            foreach (var strategy in this.strategies)
            {
                strategyUseCounts.Add(0);
            }

            bool puzzleAdvanced = false;

            // I have finally used a do while loop. At long last!
            do
            {
                puzzleAdvanced = false;
                for (int i = 0; i < this.strategies.Count; ++i)
                {
                    while (this.strategies[i].AdvancePuzzle(puzzleToSolve))
                    {
                        ++strategyUseCounts[i];
                        puzzleAdvanced = true;
                    }
                }
            }
            while (puzzleAdvanced);

            bool isSolved = puzzleToSolve.IsSolved();
            bool isValid = puzzleToSolve.IsValid();
            if (isValid)
            {
                if (!puzzle.IsSolved())
                {
                    foreach (SudokuPuzzle solution in this.Guess(puzzleToSolve))
                    {
                        solutions.Add(solution);
                    }
                }
                else
                {
                    solutions.Add(puzzleToSolve);
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
            Cell cellToGuessOn = puzzle.Rows[0][0];
            int x = 0;
            int y = 0;
            for (int i = 0; i < puzzle.Rows.Count; ++i)
            {
                for (int k = 0; k < puzzle.Rows[i].Count; ++k)
                {
                    if (puzzle.Rows[i][k].Count < cellToGuessOn.Count)
                    {
                        cellToGuessOn = puzzle.Rows[i][k];
                        x = i;
                        y = k;
                    }
                }
            }

            List<SudokuPuzzle> solutions = new List<SudokuPuzzle>();
            foreach (int allowedValue in cellToGuessOn.AllowedValues.Keys)
            {
                SudokuPuzzle newPuzzle = puzzle.Clone();
                newPuzzle.Rows[x][y].RemoveAllExcept(allowedValue);
                List<SudokuPuzzle> guessedSolutions = this.Solve(newPuzzle);
                foreach (var guessedSolution in guessedSolutions)
                {
                    solutions.Add(guessedSolution);
                }

                if (solutions.Count > 1)
                {
                    break;
                }
            }

            return solutions;
        }
    }
}
