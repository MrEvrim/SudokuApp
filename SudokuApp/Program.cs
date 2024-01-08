using SudokuApp.Core.Helpers;

namespace SudokuApp
{
  internal static class Program
  {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      ApplicationConfiguration.Initialize();

      AppServiceProvider.Initialize();

      Application.Run(new Sudoku());
    }
  }
}