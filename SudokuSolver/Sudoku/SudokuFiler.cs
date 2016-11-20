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
    public class SudokuFiler
    {
        /// <summary>
        /// The file path to the file containing the <see cref="SudokuPuzzle"/>
        /// </summary>
        private string filepath;

        /// <summary>
        /// A dictionary used to convert character symbols into integers
        /// </summary>
        private Dictionary<char, int> symbolToIntMap;

        /// <summary>
        /// A dictionary used to convert integers into character symbols
        /// </summary>
        private Dictionary<int, char> intToSymbolMap;

        /// <summary>
        /// The dimension of the <see cref="SudokuPuzzle"/>
        /// </summary>
        private int dimension;

        /// <summary>
        /// A list of the values in the file (converted to integers)
        /// </summary>
        private List<int> cellValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="SudokuFiler"/> class
        /// </summary>
        /// <param name="filepath">A file containing a <see cref="SudokuPuzzle"/></param>
        public SudokuFiler(string filepath)
        {
            this.filepath = filepath;
            this.intToSymbolMap = new Dictionary<int, char>();
            this.symbolToIntMap = new Dictionary<char, int>();
            this.cellValues = new List<int>();

            this.intToSymbolMap.Add(0, '-');
            this.symbolToIntMap.Add('-', 0);

            if (!File.Exists(filepath))
            {
                throw new Exception($"Error: Could not find file {filepath}");
            }
            else
            {
                try
                {
                    string[] lines = File.ReadAllLines(filepath);
                    this.dimension = Convert.ToInt32(Math.Sqrt(Convert.ToInt32(lines[0])));
                    string[] symbols = lines[1].Split(' ');

                    for (int i = 0; i < symbols.Length; ++i)
                    {
                        this.symbolToIntMap.Add(Convert.ToChar(symbols[i]), i + 1);
                        this.intToSymbolMap.Add(i + 1, Convert.ToChar(symbols[i]));
                    }

                    for (int i = 2; i < lines.Length; ++i)
                    {
                        symbols = lines[i].Split(' ');
                        foreach (string symbol in symbols)
                        {
                            if (symbol.Length > 0)
                            {
                                this.cellValues.Add(this.symbolToIntMap[Convert.ToChar(symbol[0])]);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Error: Could not convert file {filepath} into a sudoku puzzle.\n{e.ToString()}");
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="SudokuPuzzle"/> from the file given when the
        /// <see cref="SudokuFiler"/> was created
        /// </summary>
        /// <returns>A new <see cref="SudokuPuzzle"/> created from the <see cref="SudokuFiler"/> file</returns>
        public SudokuPuzzle CreatePuzzle()
        {
            return new SudokuPuzzle(this.dimension, this.cellValues);
        }

        /// <summary>
        /// Converts the <see cref="SudokuPuzzle"/> into a list of strings
        /// </summary>
        /// <returns>A list of strings containing the data the <see cref="SudokuFiler"/> read from the file</returns>
        public List<string> ConvertToStrings()
        {
            List<string> stringEquivalent = new List<string>();

            stringEquivalent.Add((this.dimension * this.dimension).ToString());
            StringBuilder symbolLineBuilder = new StringBuilder();
            for (int i = 1; i < this.intToSymbolMap.Count; ++i)
            {
                if (symbolLineBuilder.Length > 0)
                {
                    symbolLineBuilder.Append(' ');
                }

                symbolLineBuilder.Append(this.intToSymbolMap[i]);
            }

            stringEquivalent.Add(symbolLineBuilder.ToString());

            symbolLineBuilder.Clear();
            foreach (int cellValue in this.cellValues)
            {
                if (symbolLineBuilder.Length > 0)
                {
                    symbolLineBuilder.Append(' ');
                }

                symbolLineBuilder.Append(this.intToSymbolMap[cellValue]);

                if ((symbolLineBuilder.Length + 1) / 2 == this.dimension * this.dimension)
                {
                    stringEquivalent.Add(symbolLineBuilder.ToString());
                    symbolLineBuilder.Clear();
                }
            }

            return stringEquivalent;
        }

        /// <summary>
        /// Converts the given <see cref="SudokuPuzzle"/> into a list of strings
        /// </summary>
        /// <param name="puzzle">The <see cref="SudokuPuzzle"/> being converted into a list of strings</param>
        /// <returns>A list of strings containing the <see cref="SudokuPuzzle"/></returns>
        public List<string> ConvertToStrings(SudokuPuzzle puzzle)
        {
            List<string> stringEquivalent = new List<string>();

            stringEquivalent.Add((puzzle.Dimension * puzzle.Dimension).ToString());
            StringBuilder symbolLineBuilder = new StringBuilder();
            for (int i = puzzle.CellMin; i <= puzzle.CellMax; ++i)
            {
                if (symbolLineBuilder.Length > 0)
                {
                    symbolLineBuilder.Append(' ');
                }

                symbolLineBuilder.Append(this.intToSymbolMap[i]);
            }

            stringEquivalent.Add(symbolLineBuilder.ToString());

            symbolLineBuilder.Clear();
            foreach (var row in puzzle.Rows)
            {
                foreach (Cell cell in row)
                {
                    if (symbolLineBuilder.Length > 0)
                    {
                        symbolLineBuilder.Append(' ');
                    }

                    int cellValue = 0;
                    if (cell.Count == 1)
                    {
                        cellValue = cell.AllowedValue;
                    }

                    symbolLineBuilder.Append(this.intToSymbolMap[cellValue]);
                }

                stringEquivalent.Add(symbolLineBuilder.ToString());
                symbolLineBuilder.Clear();
            }

            return stringEquivalent;
        }
    }
}
