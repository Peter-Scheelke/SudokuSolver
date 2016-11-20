//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Sudoku;

    /// <summary>
    /// The main entry point of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point of the program
        /// </summary>
        /// <param name="args">An array of command line arguments</param>
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Error: Missing file paths");
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine($"Error: Could not find file {args[0]}");
            }
            else
            {
                try
                {
                    SudokuFiler filer = new SudokuFiler(args[0]);
                    SudokuPuzzle puzzleToSolve = filer.CreatePuzzle();
                    SudokuSolver.SudokuSolver solver = new SudokuSolver.SudokuSolver();
                    List<SudokuPuzzle> solutions = solver.Solve(puzzleToSolve);
                    OutputSolutions(args[1], filer, puzzleToSolve, solutions);
                }
                catch (Exception)
                {
                    string[] sudokuFile = File.ReadAllLines(args[0]);
                    List<string> lines = new List<string>();
                    foreach (string line in sudokuFile)
                    {
                        lines.Add(line);
                    }

                    lines.Add(string.Empty);
                    lines.Add("Bad Puzzle");
                    WriteToFile(args[1], lines);
                }
            }
        }

        /// <summary>
        /// Output the solutions to the given <see cref="SudokuPuzzle"/> to the given file path
        /// </summary>
        /// <param name="filepath">The file to which the solutions will be written</param>
        /// <param name="filer">Used to convert <see cref="SudokuPuzzle"/>s into lists of strings</param>
        /// <param name="puzzleToSolve">The <see cref="SudokuPuzzle"/> being solved</param>
        /// <param name="solutions">A list of the solutions to the <see cref="SudokuPuzzle"/> being solved</param>
        private static void OutputSolutions(string filepath, SudokuFiler filer, SudokuPuzzle puzzleToSolve, List<SudokuPuzzle> solutions)
        {
            if (solutions.Count == 0)
            {
                List<string> unsolvablePuzzle = filer.ConvertToStrings(puzzleToSolve);
                unsolvablePuzzle.Add(string.Empty);
                unsolvablePuzzle.Add("Unsolvable Puzzle");
                WriteToFile(filepath, unsolvablePuzzle);
            }
            else if (solutions.Count == 1)
            {
                List<string> puzzleOutput = filer.ConvertToStrings(puzzleToSolve);
                List<string> solution = filer.ConvertToStrings(solutions[0]);
                puzzleOutput.Add(string.Empty);
                puzzleOutput.Add("Solved");
                puzzleOutput.AddRange(solution);
                WriteToFile(filepath, puzzleOutput);
            }
            else
            {
                List<string> puzzleOutput = filer.ConvertToStrings(puzzleToSolve);
                puzzleOutput.Add(string.Empty);
                puzzleOutput.Add("Multiple Solutions");

                foreach (SudokuPuzzle solution in solutions)
                {
                    puzzleOutput.Add(string.Empty);
                    puzzleOutput.AddRange(filer.ConvertToStrings(solution));
                }

                WriteToFile(filepath, puzzleOutput);
            }
        }

        /// <summary>
        /// Write the given list of strings to the given file path
        /// </summary>
        /// <param name="filepath">The file to which the strings are being written</param>
        /// <param name="lines">The strings being written to the file</param>
        private static void WriteToFile(string filepath, List<string> lines)
        {
            try
            {
                File.WriteAllLines(filepath, lines.ToArray());
            }
            catch (Exception)
            {
                Console.WriteLine($"Error: Could not write to file {filepath}");
            }
        }
    }
}