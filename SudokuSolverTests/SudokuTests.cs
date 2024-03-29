﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Sudoku;
using System.Collections.Generic;
using System.IO;
using SudokuSolver.SudokuStrategies;
using SudokuSolver.SudokuSolver;
using SudokuSolver;

namespace SudokuSolverTests
{
    [TestClass]
    public class SudokuTests
    {
        /// <summary>
        /// Make sure that <see cref="Cell"/>s are created correctly and that they
        /// throw exceptions correctly when created incorrectly
        /// </summary>
        [TestMethod]
        public void TestCellCreation()
        {
            // Test the default constructor
            Cell cell = new Cell();
            Assert.IsTrue(cell.AllowedValues.Count == 9);

            // Test the non default constructor with valid input
            for (int i = 1; i <= 9; ++i)
            {
                Assert.IsTrue(cell.AllowedValues[i] == i);
            }

            List<int> values = new List<int>() { 2, 4, 6, 8 };
            cell = new Cell(values, 1, 9);
            Assert.IsTrue(cell.AllowedValues.Count == 4);

            values = null;

            // Test the non default constructor with a null list
            bool exceptionWasThrown = false;
            try
            {
                cell = new Cell(values, 1, 9);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);

            // Test the non default constructor with an empty list
            exceptionWasThrown = false;
            values = new List<int>();
            try
            {
                cell = new Cell(values, 1, 9);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);

            // Test the non default constructor with a list containing a value less than 1
            exceptionWasThrown = false;
            values = new List<int>();
            values.Add(0);
            try
            {
                cell = new Cell(values, 1, 9);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);

            // Test the non default constructor with a list containing a value greater than 9
            exceptionWasThrown = false;
            values = new List<int>();
            values.Add(10);
            try
            {
                cell = new Cell(values, 1, 9);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);

            // Test the non default constructor with a list containing duplicates
            exceptionWasThrown = false;
            values = new List<int>();
            values.Add(8);
            values.Add(8);
            try
            {
                cell = new Cell(values, 1, 9);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
        }

        /// <summary>
        /// Make sure allowed values can be added to and removed from a <see cref="Cell"/>
        /// </summary>
        [TestMethod]
        public void TestCellAdd()
        {
            Cell cell = new Cell();

            bool exceptionWasThrown = false;

            // Test adding an allowed value that is too small
            List<int> values = new List<int>();
            values.Add(5);
            cell = new Cell(values, 1, 9);

            exceptionWasThrown = false;
            try
            {
                cell.AddAllowedValue(0);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            Assert.IsTrue(cell.Count == 1);

            // Test adding an allowed value that is too large
            exceptionWasThrown = false;
            try
            {
                cell.AddAllowedValue(10);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            Assert.IsTrue(cell.AllowedValues.Count == 1);

            // Make sure you can actually add a correct value
            cell.AddAllowedValue(3);
            Assert.IsTrue(cell.AllowedValues.Count == 2);

            cell.AddAllowedValue(4);
            Assert.IsTrue(cell.AllowedValues.Count == 3);
        }

        /// <summary>
        /// Test to make sure that allowed values can be removed from a <see cref="Cell"/>
        /// </summary>
        [TestMethod]
        public void TestCellRemove()
        {
            Cell cell = new Cell();

            bool exceptionWasThrown = false;

            // Try removing a value that is too small
            try
            {
                cell.RemoveAllowedValue(0);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            exceptionWasThrown = false;

            // Try removing a value that is too large
            try
            {
                cell.RemoveAllowedValue(10);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            exceptionWasThrown = false;

            // Make sure a proper removal works
            cell.RemoveAllowedValue(4);
            Assert.IsTrue(cell.AllowedValues.Count == 8);
            Assert.IsFalse(cell.AllowedValues.ContainsKey(4));

            // Make sure that trying to remove the last allowed value of a cell throws an exception
            cell = new Cell();
            for (int i = 1; i < 9; ++i)
            {
                cell.RemoveAllowedValue(i);
            }

            Assert.IsTrue(cell.AllowedValues.Count == 1);
            Assert.IsTrue(cell.AllowedValues.ContainsKey(9));

            try
            {
                cell.RemoveAllowedValue(9);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            exceptionWasThrown = false;

            // Test remove all except
            cell = new Cell();
            try
            {
                cell.RemoveAllExcept(0);
            }
            catch (Exception e)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            exceptionWasThrown = false;

            try
            {
                cell.RemoveAllExcept(10);
            }
            catch (Exception e)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            exceptionWasThrown = false;

            cell.RemoveAllExcept(9);
            Assert.IsTrue(cell.AllowedValue == 9);
            for (int i = 1; i < 9; ++i)
            {
                Assert.IsFalse(cell.AllowedValues.ContainsKey(i));
            }

            Assert.IsTrue(cell.AllowedValues.ContainsKey(9));
            try
            {
                cell.RemoveAllExcept(5);
            }
            catch (Exception e)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            exceptionWasThrown = false;
        }

        /// <summary>
        /// Make sure that <see cref="Cell"/>s correctly update themselves
        /// with EliminateValues
        /// </summary>
        [TestMethod]
        public void TestEliminateValues()
        {
            List<int> cellValues = new List<int>();
            int dim = 3;
            for (int i = 0; i < dim * dim * dim * dim; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dim, cellValues);

            Cell testCell = puzzle.Rows[0][0];

            // Let one of the cells in the same column have a value of 5
            // testCell should return true with EliminateValues once,
            // then the next time it shouldn't
            testCell.Column[dim * dim - 1].RemoveAllExcept(5);
            Assert.IsTrue(testCell.Count == dim * dim);
            Assert.IsTrue(testCell.EliminateValues());
            Assert.IsFalse(testCell.EliminateValues());

            // Make sure testCell has the correct value
            Assert.IsFalse(testCell.AllowedValues.ContainsKey(5));
            Assert.IsTrue(testCell.Count == dim * dim - 1);

            // Test testCell's row
            testCell.Row[dim * dim - 1].RemoveAllExcept(6);
            Assert.IsTrue(testCell.EliminateValues());
            Assert.IsFalse(testCell.EliminateValues());
            Assert.IsFalse(testCell.AllowedValues.ContainsKey(6));
            Assert.IsTrue(testCell.Count == dim * dim - 2);

            // Test testCell's block
            testCell.Block[dim * dim - 1].RemoveAllExcept(7);
            Assert.IsTrue(testCell.EliminateValues());
            Assert.IsFalse(testCell.EliminateValues());
            Assert.IsFalse(testCell.AllowedValues.ContainsKey(7));
            Assert.IsTrue(testCell.Count == dim * dim - 3);
        }

        /// <summary>
        /// Make sure that <see cref="SudokuPuzzle"/>s are properly constructed
        /// </summary>
        [TestMethod]
        public void TestSudokuPuzzle()
        {
            List<int> cellValues = new List<int>();
            int dim = 3;
            for (int i = 0; i < dim * dim * dim * dim; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dim, cellValues);

            // Make sure the puzzle got constructed such that all values are
            // allowed in each cell
            foreach (var row in puzzle.Rows)
            {
                foreach (Cell cell in row)
                {
                    Assert.IsTrue(cell.AllowedValues.Count == dim * dim);
                }
            }

            foreach (var col in puzzle.Columns)
            {
                foreach (Cell cell in col)
                {
                    Assert.IsTrue(cell.AllowedValues.Count == dim * dim);
                }
            }

            foreach (var block in puzzle.Blocks)
            {
                foreach (Cell cell in block)
                {
                    Assert.IsTrue(cell.AllowedValues.Count == dim * dim);
                }
            }

            // Make sure that the cells are correctly stored in the rows, columns, and blocks.

            // Test the top left cell
            Cell testCell = puzzle.Rows[0][0];
            Assert.IsTrue(object.ReferenceEquals(testCell.Row, puzzle.Rows[0]));
            Assert.IsTrue(object.ReferenceEquals(testCell.Column, puzzle.Columns[0]));
            Assert.IsTrue(object.ReferenceEquals(testCell.Block, puzzle.Blocks[0]));
            Assert.IsTrue(object.ReferenceEquals(testCell, puzzle.Columns[0][0]));
            Assert.IsTrue(object.ReferenceEquals(testCell, puzzle.Blocks[0][0]));

            // Test the top right cell
            testCell = puzzle.Rows[0][dim * dim - 1];
            Assert.IsTrue(object.ReferenceEquals(testCell.Row, puzzle.Rows[0]));
            Assert.IsTrue(object.ReferenceEquals(testCell.Column, puzzle.Columns[dim * dim - 1]));
            Assert.IsTrue(object.ReferenceEquals(testCell.Block, puzzle.Blocks[dim - 1]));
            Assert.IsTrue(object.ReferenceEquals(testCell, puzzle.Columns[dim * dim - 1][0]));
            Assert.IsTrue(object.ReferenceEquals(testCell, puzzle.Blocks[dim - 1][dim - 1]));

            // Test the bottom left cell
            testCell = puzzle.Rows[dim * dim - 1][0];
            Assert.IsTrue(object.ReferenceEquals(testCell.Row, puzzle.Rows[dim * dim - 1]));
            Assert.IsTrue(object.ReferenceEquals(testCell.Column, puzzle.Columns[0]));
            Assert.IsTrue(object.ReferenceEquals(testCell.Block, puzzle.Blocks[dim * dim - dim]));
            Assert.IsTrue(object.ReferenceEquals(testCell, puzzle.Columns[0][dim * dim - 1]));
            Assert.IsTrue(object.ReferenceEquals(testCell, puzzle.Blocks[dim * dim - dim][dim * dim - dim]));

            // Test the bottom right cell
            testCell = puzzle.Rows[dim * dim  - 1][dim * dim - 1];
            Assert.IsTrue(object.ReferenceEquals(testCell.Row, puzzle.Rows[dim * dim - 1]));
            Assert.IsTrue(object.ReferenceEquals(testCell.Column, puzzle.Columns[dim * dim - 1]));
            Assert.IsTrue(object.ReferenceEquals(testCell.Block, puzzle.Blocks[dim * dim - 1]));
            Assert.IsTrue(object.ReferenceEquals(testCell, puzzle.Columns[dim * dim - 1][dim * dim - 1]));
            Assert.IsTrue(object.ReferenceEquals(testCell, puzzle.Blocks[dim * dim - 1][dim * dim - 1]));            
        }

        /// <summary>
        /// Test SudokuPuzzle creation errors
        /// </summary>
        [TestMethod]
        public void TestSudokuPuzzleErrors()
        {
            int dim = 1;
            List<int> badCellValues = new List<int>();
            badCellValues.Add(-1);

            // Value is too low
            bool exceptionWasThrown = false;
            try
            {
                SudokuPuzzle puzzle = new SudokuPuzzle(dim, badCellValues);
            }
            catch (Exception e)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);

            // Value is too high
            exceptionWasThrown = false;
            badCellValues.Clear();
            badCellValues.Add(3);
            try
            {
                SudokuPuzzle puzzle = new SudokuPuzzle(dim, badCellValues);
            }
            catch (Exception e)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);

            exceptionWasThrown = false;
            badCellValues.Clear();
            badCellValues.Add(1);
            badCellValues.Add(1);
            try
            {
                SudokuPuzzle puzzle = new SudokuPuzzle(dim, badCellValues);
            }
            catch (Exception e)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);

            // Try creating a valid one with a given value to start out
            dim = 2;
            List<int> cellValues = new List<int>();
            for (int i = 0; i < dim * dim * dim * dim; ++i)
            {
                cellValues.Add(0);
            }

            cellValues[dim] = 1;
            SudokuPuzzle outerPuzzle = new SudokuPuzzle(dim, cellValues);
            Cell testCell = outerPuzzle.Rows[0][dim];
            foreach (var row in outerPuzzle.Rows)
            {
                foreach (Cell cell in row)
                {
                    if (!object.ReferenceEquals(testCell, cell))
                    {
                        Assert.IsTrue(cell.AllowedValues.Count == dim * dim);
                    }
                    else
                    {
                        Assert.IsTrue(cell.AllowedValues.Count == 1 && cell.AllowedValues.ContainsKey(1));
                    }
                }
            }

            // Make sure it's possible to edit rows, columns, and blocks
            outerPuzzle.Rows = new List<List<Cell>>();
            outerPuzzle.Columns = new List<List<Cell>>();
            outerPuzzle.Blocks = new List<List<Cell>>();
        }

        /// <summary>
        /// Make sure the <see cref="SudokuPuzzle"/>'s Refresh method works
        /// </summary>
        [TestMethod]
        public void TestPuzzleRefresh()
        {
            int dimension = 3;
            List<int> cellValues = new List<int>();

            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);
            Assert.IsFalse(puzzle.Refresh());

            puzzle.Rows[0][0].RemoveAllExcept(1);
            Assert.IsTrue(puzzle.Refresh());
            Assert.IsFalse(puzzle.Refresh());
            foreach (Cell cell in puzzle.Rows[0])
            {
                if (cell.Count != 1)
                {
                    Assert.IsFalse(cell.AllowedValues.ContainsKey(1));
                }
            }

            puzzle.Columns[1][1].RemoveAllExcept(2);
            Assert.IsTrue(puzzle.Refresh());
            Assert.IsFalse(puzzle.Refresh());
            foreach (Cell cell in puzzle.Columns[1])
            {
                if (cell.Count != 1)
                {
                    Assert.IsFalse(cell.AllowedValues.ContainsKey(2));
                }
            }

            puzzle.Blocks[2][2].RemoveAllExcept(3);
            Assert.IsTrue(puzzle.Refresh());
            Assert.IsFalse(puzzle.Refresh());
            foreach (Cell cell in puzzle.Blocks[2])
            {
                if (cell.Count != 1)
                {
                    Assert.IsFalse(cell.AllowedValues.ContainsKey(3));
                }
            }
        }

        /// <summary>
        /// Test the <see cref="SudokuFiler"/>
        /// </summary>
        [TestMethod]
        public void TestSudokuFiler()
        {
            string inputFilePath = "TestPuzzleInput.txt";
            SudokuFiler filer = new SudokuFiler(inputFilePath);
            SudokuPuzzle puzzle = filer.CreatePuzzle();

            List<string> stringEquivalent = filer.ConvertToStrings();
            string[] fileData = File.ReadAllLines(inputFilePath);

            Assert.IsTrue(stringEquivalent.Count == fileData.Length);
            for (int i = 0; i < fileData.Length; ++i)
            {
                Assert.IsTrue(stringEquivalent[i] == fileData[i]);
            }

            stringEquivalent = filer.ConvertToStrings(puzzle);
            Assert.IsTrue(stringEquivalent.Count == fileData.Length);
            for (int i = 0; i < fileData.Length; ++i)
            {
                Assert.IsTrue(stringEquivalent[i] == fileData[i]);
            }

            bool exceptionWasThrown = false;

            // Try a bad file path
            try
            {
                SudokuFiler badFiler = new SudokuFiler("notafilepath.txt");
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            exceptionWasThrown = false;

            File.Create("thisisanemptyfile.txt");
            try
            {
                SudokuFiler badFiler = new SudokuFiler("thisisanemptyfile.txt");
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
        }

        [TestMethod]
        public void TestClone()
        {
            string inputFilePath = "TestPuzzleInput.txt";
            SudokuFiler filer = new SudokuFiler(inputFilePath);
            SudokuPuzzle puzzle = filer.CreatePuzzle();

            for (int i = 0; i < puzzle.Dimension * puzzle.Dimension; ++i)
            {
                if (puzzle.Rows[0][i].Count == 1)
                {
                    int allowedValue = puzzle.Rows[0][i].AllowedValue;
                    if (allowedValue != 1)
                    {
                        allowedValue = 1;
                    }
                    else
                    {
                        ++allowedValue;
                    }

                    puzzle.Rows[0][i].AddAllowedValue(allowedValue);
                    break;
                }
            }

            SudokuPuzzle clonedPuzzle = puzzle.Clone();

            for (int i = 0; i < puzzle.Dimension * puzzle.Dimension; ++i)
            {
                for (int k = 0; k < puzzle.Dimension * puzzle.Dimension; ++k)
                {
                    Assert.IsTrue(puzzle.Rows[i][k].Count == clonedPuzzle.Rows[i][k].Count);
                    Assert.IsTrue(puzzle.Columns[i][k].Count == clonedPuzzle.Columns[i][k].Count);
                    Assert.IsTrue(puzzle.Blocks[i][k].Count == clonedPuzzle.Blocks[i][k].Count);
                    foreach (int allowedValue in puzzle.Rows[i][k].AllowedValues.Keys)
                    {
                        Assert.IsTrue(clonedPuzzle.Rows[i][k].AllowedValues.ContainsKey(allowedValue));
                    }
                }
            }
        }

        /// <summary>
        /// Make sure a <see cref="SudokuPuzzle"/> can create a list of allowed <see cref="Cell"/> values correctly
        /// </summary>
        [TestMethod]
        public void TestAllowedValues()
        {
            int dimension = 3;
            List<int> cellValues = new List<int>();

            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);

            List<int> allowedValues = puzzle.GetAllAllowedValues();

            for (int i = 0; i < dimension * dimension; ++i)
            {
                Assert.IsTrue(allowedValues[i] == i + 1);
            }
        }

        /// <summary>
        /// Make sure the IsValid method of <see cref="SudokuPuzzle"/>
        /// is working correctly
        /// </summary>
        [TestMethod]
        public void TestPuzzleValidity()
        {
            int dimension = 3;
            List<int> cellValues = new List<int>();

            // Test a row
            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);

            Assert.IsTrue(puzzle.IsValid());

            // Test a column having the same value twice
            for (int i = 0; i < dimension * dimension; ++i)
            {
                puzzle.Rows[0][i].RemoveAllExcept(i + 1);
            }

            Assert.IsTrue(puzzle.IsValid());

            puzzle.Rows[dimension + 1][0].RemoveAllExcept(1);
            Assert.IsFalse(puzzle.IsValid());

            // Test a block having the same value twice
            puzzle = new SudokuPuzzle(dimension, cellValues);
            for (int i = 0; i < dimension * dimension; ++i)
            {
                puzzle.Rows[0][i].RemoveAllExcept(i + 1);
            }

            puzzle.Rows[1][1].RemoveAllExcept(1);
            Assert.IsFalse(puzzle.IsValid());

            // Test a row having the same value twice
            puzzle = new SudokuPuzzle(dimension, cellValues);
            for (int i = 0; i < dimension * dimension; ++i)
            {
                puzzle.Columns[0][i].RemoveAllExcept(i + 1);
            }

            Assert.IsTrue(puzzle.IsValid());
            puzzle.Rows[0][1].RemoveAllExcept(1);
            Assert.IsFalse(puzzle.IsValid());
        }

        /// <summary>
        /// Make sure the IsSolved method works for puzzles
        /// </summary>
        [TestMethod]
        public void TestPuzzleIsSolved()
        {
            int dimension = 3;
            List<int> cellValues = new List<int>();

            // Test a row
            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);

            Assert.IsFalse(puzzle.IsSolved());

            for (int i = 0; i < puzzle.Rows.Count; ++i)
            {
                for (int k = 0; k < puzzle.Rows[i].Count; ++k)
                {
                    puzzle.Rows[i][k].RemoveAllExcept(1);
                }
            }

            Assert.IsTrue(puzzle.IsSolved());
        }

        /// <summary>
        /// Make sure the <see cref="BasicStrategy"/> advances the puzzle correctly
        /// </summary>
        [TestMethod]
        public void TestBasicStrategy()
        {
            BasicStrategy strategy = new BasicStrategy();

            Assert.IsTrue(strategy.ToString() == "Basic Strategy");

            int dimension = 3;
            List<int> cellValues = new List<int>();

            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);

            Assert.IsFalse(strategy.AdvancePuzzle(puzzle));
            puzzle.Rows[0][0].RemoveAllExcept(1);
            int count = 0;
            while (strategy.AdvancePuzzle(puzzle))
            {
                ++count;
            }

            Assert.IsTrue(count > 0);
            Assert.IsFalse(strategy.AdvancePuzzle(puzzle));
        }

        /// <summary>
        /// Test the <see cref="SingleCellStrategy"/>
        /// </summary>
        [TestMethod]
        public void TestSingleCellStrategy()
        {
            SingleCellStrategy strategy = new SingleCellStrategy();

            Assert.IsTrue(strategy.ToString() == "Single Cell Strategy");

            int dimension = 3;
            List<int> cellValues = new List<int>();

            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);

            Assert.IsFalse(strategy.AdvancePuzzle(puzzle));

            for (int i = 0; i < puzzle.Rows[0].Count - 1; ++i)
            {
                puzzle.Rows[0][i].RemoveAllowedValue(1);
            }

            Assert.IsTrue(puzzle.Rows[0][dimension * dimension - 1].Count == dimension * dimension);
            Assert.IsTrue(strategy.AdvancePuzzle(puzzle));
            Assert.IsTrue(puzzle.Rows[0][dimension * dimension - 1].AllowedValues.ContainsKey(1));
            Assert.IsTrue(puzzle.Rows[0][dimension * dimension - 1].Count == 1);
        }

        /// <summary>
        /// Make sure the <see cref="SharedSubgroupStrategy"/> works correctly
        /// </summary>
        [TestMethod]
        public void TestSharedSubgroupStrategy()
        {
            SharedSubgroupStrategy strategy = new SharedSubgroupStrategy();

            Assert.IsTrue(strategy.ToString() == "Shared Subgroup Strategy");

            int dimension = 3;
            List<int> cellValues = new List<int>();

            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);

            Assert.IsFalse(strategy.AdvancePuzzle(puzzle));

            for (int i = 0; i < dimension * dimension - dimension; ++i)
            {
                puzzle.Columns[0][i].RemoveAllExcept(i + 1);
            }

            Assert.IsTrue(strategy.AdvancePuzzle(puzzle));

            List<Cell> block = puzzle.Blocks[dimension * dimension - dimension];
            foreach (Cell cell in block)
            {
                if (!object.ReferenceEquals(cell.Column, puzzle.Columns[0]))
                {
                    for (int i = dimension * dimension - dimension + 1; i < dimension * dimension + 1; ++i)
                    {
                        Assert.IsFalse(cell.AllowedValues.ContainsKey(i));
                    }
                }
                else
                {
                    for (int i = dimension * dimension - dimension + 1; i < dimension * dimension + 1; ++i)
                    {
                        Assert.IsTrue(cell.AllowedValues.ContainsKey(i));
                    }
                }
            }

            for (int i = 0; i < dimension * dimension - dimension; ++i)
            {
                puzzle.Rows[0][i].RemoveAllExcept(i + 1);
            }

            Assert.IsTrue(strategy.AdvancePuzzle(puzzle));
            block = puzzle.Blocks[dimension - 1];

            foreach (Cell cell in block)
            {
                if (!object.ReferenceEquals(cell.Row, puzzle.Rows[0]))
                {
                    for (int i = dimension * dimension - dimension + 1; i < dimension * dimension + 1; ++i)
                    {
                        Assert.IsFalse(cell.AllowedValues.ContainsKey(i));
                    }
                }
                else
                {
                    for (int i = dimension * dimension - dimension + 1; i < dimension * dimension + 1; ++i)
                    {
                        Assert.IsTrue(cell.AllowedValues.ContainsKey(i));
                    }
                }
            }
        }

        /// <summary>
        /// Make sure the <see cref="SudokuSolver.SudokuSolver.SudokuSolver"/> is working
        /// </summary>
        [TestMethod]
        public void TestSudokuSolver()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Input");

            foreach (string file in files)
            {
                try
                {
                    SudokuFiler filer = new SudokuFiler(file);
                    SudokuSolver.SudokuSolver.SudokuSolver solver = new SudokuSolver.SudokuSolver.SudokuSolver();
                    List<SudokuPuzzle> puzzles = solver.Solve(filer.CreatePuzzle());
                    Assert.IsTrue(puzzles.Count == 1);
                }
                catch (Exception e)
                {

                }
            }
        }

        /// <summary>
        /// Make sure the main program runs without breaking and also that it creates files
        /// </summary>
        [TestMethod]
        public void TestProgram()
        {
            System.IO.DirectoryInfo directory;
            string outputDirectory = "ProgramTestOutput";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            else
            {
                directory = new DirectoryInfo(outputDirectory);
                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo subdirectory in directory.GetDirectories())
                {
                    subdirectory.Delete(true);
                }
            }

            Directory.CreateDirectory(outputDirectory);

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Input");
            foreach (string inputPath in files)
            {
                string outputPath = $@"{outputDirectory}\{Path.GetFileName(inputPath)}";
                string[] args = { inputPath, outputPath };
                Program.Main(args);
            }

            Assert.IsTrue(Directory.Exists(outputDirectory));
            directory = new DirectoryInfo(outputDirectory);
            Assert.IsTrue(directory.GetFiles().Length == files.Length);
        }

        /// <summary>
        /// Make sure the program doesn't crash with bad input
        /// </summary>
        [TestMethod]
        public void TestProgramWithBadInput()
        {
            string badInput = "<>You can't have these symbols!?<>.txt";
            string badOutput = "<>This one is just as bad!<>";

            string[] badInputArray1 = { badInput, badOutput };
            Program.Main(badInputArray1);

            string[] badInputArray2 = { badInput };
            Program.Main(badInputArray2);

            string[] badInputArray3 = { @"Input\Puzzle-4x4-0001.txt", badOutput };
            Program.Main(badInputArray3);

            string[] badInputArray4 = { badInput, @"TestOutput.txt" };
            Program.Main(badInputArray4);

            Assert.IsFalse(File.Exists("TestOutput.txt"));
        }

        /// <summary>
        /// Make sure the <see cref="SudokuSolver.SudokuSolver.SudokuSolver"/>
        /// gets the correct result when solving puzzles
        /// </summary>
        [TestMethod]
        public void TestSolvingSpecificPuzzles()
        {
            string badPuzzlePath = @"Input\Puzzle-25x25-0902.txt";
            string multipleSolutionsPuzzlePath = @"Input\Puzzle-16x16-0902.txt";
            string goodPuzzlePath = @"Input\Puzzle-16x16-0301.txt";

            SudokuFiler filer = null;
            try
            {
                filer = new SudokuFiler(badPuzzlePath);
            }
            catch(Exception)
            {
            }

            Assert.IsTrue(filer == null);

            filer = new SudokuFiler(multipleSolutionsPuzzlePath);
            SudokuPuzzle puzzle = filer.CreatePuzzle();
            SudokuSolver.SudokuSolver.SudokuSolver solver = new SudokuSolver.SudokuSolver.SudokuSolver();
            Assert.IsTrue(solver.Solve(puzzle).Count == 2);

            filer = new SudokuFiler(goodPuzzlePath);
            puzzle = filer.CreatePuzzle();
            solver = new SudokuSolver.SudokuSolver.SudokuSolver();
            Assert.IsTrue(solver.Solve(puzzle).Count == 1);
        }
    }
}
