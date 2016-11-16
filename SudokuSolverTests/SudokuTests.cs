using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Sudoku;
using System.Collections.Generic;
using System.IO;
using SudokuSolver.SudokuStrategies;

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

            // Test adding an allowed value to a full cell
            bool exceptionWasThrown = false;
            for (int i = 1; i < 10; ++i)
            {
                try
                {
                    cell.AddAllowedValue(i);
                }
                catch (Exception)
                {
                    exceptionWasThrown = true;
                }

                Assert.IsTrue(exceptionWasThrown);
                exceptionWasThrown = false;
            }

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

            // Make sure removing something that isn't there throws an exception
            try
            {
                cell.RemoveAllowedValue(4);
            }
            catch (Exception)
            {
                exceptionWasThrown = true;
            }

            Assert.IsTrue(exceptionWasThrown);
            exceptionWasThrown = false;

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
        /// Test the <see cref="SudokuFiler"/>
        /// </summary>
        [TestMethod]
        public void TestSudokuFiler()
        {
            string inputFilePath = "TestPuzzleInput.txt";
            string outputFilePath = "TestPuzzleOutput.txt";
            SudokuPuzzle puzzle = SudokuFiler.Read(inputFilePath);

            SudokuFiler.Write(outputFilePath, puzzle);

            string[] inputFile = File.ReadAllLines(inputFilePath);
            string[] outputFile = File.ReadAllLines(outputFilePath);

            Assert.IsTrue(inputFile.Length == outputFile.Length);

            for (int i = 1; i < outputFile.Length; ++i)
            {
                Assert.IsTrue(outputFile[i].Length == puzzle.Dimension * puzzle.Dimension * 2 - 1);
            }

            for (int i = 0; i < inputFile.Length; ++i)
            {
                Assert.IsTrue(inputFile[i] == outputFile[i]);
            }

            // Try a bad file path
            try
            {
                SudokuFiler.Read("notafilepath.txt");
            }
            catch (Exception)
            {
                return;
            }

            Assert.Fail();
        }

        [TestMethod]
        public void TestClone()
        {
            string inputFilePath = "TestPuzzleInput.txt";
            SudokuPuzzle puzzle = SudokuFiler.Read(inputFilePath);

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
        /// Test the <see cref="OnlyChoiceStrategy"/>
        /// </summary>
        [TestMethod]
        public void TestOnlyChoiceStrategy()
        {
            int dimension = 3;
            List<int> cellValues = new List<int>();

            // Test a row
            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            for (int i = 0; i < dimension * dimension - 1; ++i)
            {
                cellValues[i] = i + 1;
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);

            OnlyChoiceStrategy strategy = new OnlyChoiceStrategy();
            Assert.IsTrue(strategy.AdvancePuzzle(puzzle));

            for (int i = 0; i < puzzle.Rows[0].Count; ++i)
            {
                Assert.IsTrue(puzzle.Rows[0][i].Count == 1);
                Assert.IsTrue(puzzle.Rows[0][i].AllowedValue == i + 1);
            }

            // Test a column
            cellValues.Clear();
            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            for (int i = 0; i < dimension * dimension - 1; ++i)
            {
                cellValues[i * dimension * dimension] = i + 1;
            }

            
            puzzle = new SudokuPuzzle(dimension, cellValues);
            Assert.IsTrue(strategy.AdvancePuzzle(puzzle));

            for (int i = 0; i < puzzle.Columns[0].Count; ++i)
            {
                Assert.IsTrue(puzzle.Columns[0][i].Count == 1);
                Assert.IsTrue(puzzle.Columns[0][i].AllowedValue == i + 1);
            }

            // Test a block
            cellValues.Clear();
            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            int index = 0;
            for (int i = 0; i < dimension * dimension - 1; ++i)
            {
                cellValues[index] = i + 1;
                ++index;

                if ((i + 1) % dimension == 0)
                {
                    index += dimension * dimension - dimension;
                }

            }

            puzzle = new SudokuPuzzle(dimension, cellValues);
            Assert.IsTrue(strategy.AdvancePuzzle(puzzle));

            for (int i = 0; i < puzzle.Blocks[0].Count; ++i)
            {
                Assert.IsTrue(puzzle.Blocks[0][i].Count == 1);
                Assert.IsTrue(puzzle.Blocks[0][i].AllowedValue == i + 1);
            }

            // Try one that can't be advanced
            cellValues.Clear();
            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            puzzle = new SudokuPuzzle(dimension, cellValues);
            Assert.IsFalse(strategy.AdvancePuzzle(puzzle));
        }

        /// <summary>
        /// Test the <see cref="SinglePossibilityStrategy"/>
        /// </summary>
        [TestMethod]
        public void TestSinglePossibilityStrategy()
        {
            int dimension = 3;
            List<int> cellValues = new List<int>();

            // Test a row
            for (int i = 0; i < dimension * dimension * dimension * dimension; ++i)
            {
                cellValues.Add(0);
            }

            SudokuPuzzle puzzle = new SudokuPuzzle(dimension, cellValues);
            SinglePossibilityStrategy strategy = new SinglePossibilityStrategy();

            Cell changeCell = puzzle.Rows[0][0];

            Assert.IsTrue(CheckCells(changeCell, 1));
            

            for (int i = 0; i < changeCell.Block.Count - 1; ++i)
            {
                changeCell = changeCell.Block[i];
                changeCell.RemoveAllExcept(i + 1);
                Assert.IsTrue(strategy.AdvancePuzzle(puzzle));
                Assert.IsFalse(CheckCells(changeCell, i + 1));
            }
        }

        private static bool CheckCells(Cell changeCell, int value)
        {
            bool succeeded = true;
            foreach (Cell cell in changeCell.Row)
            {
                if (object.ReferenceEquals(cell, changeCell))
                {
                    continue;
                }
                else
                {
                    if (!cell.AllowedValues.ContainsKey(value))
                    {
                        succeeded = false;
                    }
                }
            }

            foreach (Cell cell in changeCell.Column)
            {
                if (object.ReferenceEquals(cell, changeCell))
                {
                    continue;
                }
                else
                {
                    if (!cell.AllowedValues.ContainsKey(value))
                    {
                        succeeded = false;
                    }
                }
            }

            foreach (Cell cell in changeCell.Block)
            {
                if (object.ReferenceEquals(cell, changeCell))
                {
                    continue;
                }
                else
                {
                    if (!cell.AllowedValues.ContainsKey(value))
                    {
                        succeeded = false;
                    }
                }
            }

            return succeeded;
        }
    }
}
