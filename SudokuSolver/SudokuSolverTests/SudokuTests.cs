using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Sudoku;
using System.Collections.Generic;

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
            cell = new Cell(values);
            Assert.IsTrue(cell.AllowedValues.Count == 4);

            values = null;

            // Test the non default constructor with a null list
            bool exceptionWasThrown = false;
            try
            {
                cell = new Cell(values);
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
                cell = new Cell(values);
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
                cell = new Cell(values);
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
                cell = new Cell(values);
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
                cell = new Cell(values);
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
            cell = new Cell(values);

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
            Assert.IsTrue(cell.AllowedValues.Count == 1);

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
        }
    }
}
