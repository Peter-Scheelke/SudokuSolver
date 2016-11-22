SudokuSolver.exe is a command line program. To run it, just run it on the command line
with the input file path and output file path as parameters (in that order).

To test it on a large selection of puzzles, run the following unit tests:

- TestSolvingSpecificPuzzles
- TestSudokuSolver

The first one makes sure that individual puzzles of each output type get solved correctly
The second test makes sure that all test puzzles create output

The only patterns used in this project were the template-method pattern (SudokuStrategy)
and the factory pattern (SudokuFiler).