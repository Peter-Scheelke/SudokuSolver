//-----------------------------------------------------------------------
// <copyright file="SudokuFiler.cs" company="Peter Scheelke">
//      Copyright (c) Peter Scheelke. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SudokuSolver.Sudoku
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Reads in <see cref="SudokuPuzzle"/>s from a file or writes them
    /// out to a file
    /// </summary>
    public static class SudokuFiler
    {
        /// <summary>
        /// Reads in the <see cref="SudokuPuzzle"/> contained in the given file
        /// and returns it
        /// </summary>
        /// <param name="filePath">The path to the file containing the <see cref="SudokuPuzzle"/></param>
        /// <returns>The <see cref="SudokuPuzzle"/> created from the contents of the given file</returns>
        public static SudokuPuzzle Read(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"Error: Coud not find file {filePath}");
            }
            else
            {
                string[] lines = File.ReadAllLines(filePath);
                int dimension = Convert.ToInt32(Math.Sqrt(Convert.ToInt32(lines[0])));

                List<int> cellValues = new List<int>();

                // Ignore the first two lines (one is the dimension and the other
                // is just a list of the column numbers)
                for (int i = 2; i < lines.Length; ++i)
                {
                    string[] symbols = lines[i].Split(' ');
                    foreach (string symbol in symbols)
                    {
                        cellValues.Add(GetIntValue(symbol[0]));
                    }
                }

                return new SudokuPuzzle(dimension, cellValues);
            }
        }

        /// <summary>
        /// Writes the given <see cref="SudokuPuzzle"/> to a file
        /// </summary>
        /// <param name="filePath">The file to which the <see cref="SudokuPuzzle"/> should be written</param>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being written to the file</param>
        public static void Write(string filePath, SudokuPuzzle puzzle)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            List<string> linesToWrite = new List<string>();
            linesToWrite.Add((puzzle.Dimension * puzzle.Dimension).ToString());

            StringBuilder columnLabels = new StringBuilder();
            for (int i = 1; i <= puzzle.Dimension * puzzle.Dimension; ++i)
            {
                if (columnLabels.Length > 0)
                {
                    columnLabels.Append(' ');
                }

                columnLabels.Append(GetCharValue(i));
            }

            linesToWrite.Add(columnLabels.ToString());

            foreach (var row in puzzle.Rows)
            {
                StringBuilder builder = new StringBuilder();
                foreach (Cell cell in row)
                {
                    int symbol = 0;
                    if (cell.AllowedValues.Count == 1)
                    {
                        foreach (int value in cell.AllowedValues.Values)
                        {
                            symbol = value;
                        }
                    }

                    char symbolForFile = GetCharValue(symbol);
                    if (builder.Length > 0)
                    {
                        builder.Append(' ');
                    }

                    builder.Append(symbolForFile);
                }

                linesToWrite.Add(builder.ToString());
            }

            File.WriteAllLines(filePath, linesToWrite.ToArray());
        }

        /// <summary>
        /// Converts a sudoku symbol into an integer
        /// </summary>
        /// <param name="symbol">The symbol being converted to an integer</param>
        /// <returns>The integer version of the given symbol</returns>
        private static int GetIntValue(char symbol)
        {
            if (symbol == '-')
            {
                return 0;
            }
            else if (char.IsDigit(symbol))
            {
                return Convert.ToInt32(char.GetNumericValue(symbol));
            }
            else
            {
                return Convert.ToInt32(symbol - 'A' + 10);
            }
        }

        /// <summary>
        /// Converts an integer into a sudoku symbol
        /// </summary>
        /// <param name="symbol">The integer being converted to a symbol</param>
        /// <returns>The character symbol of the given integer</returns>
        private static char GetCharValue(int symbol)
        {
            if (symbol == 0)
            {
                return '-';
            }
            else if (symbol <= 9 && symbol >= 1)
            {
                return Convert.ToChar(symbol.ToString()[0]);
            }
            else
            {
                return Convert.ToChar('A' + symbol - 10);
            }
        }
    }
}
