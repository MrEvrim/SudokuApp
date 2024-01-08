using Microsoft.Extensions.DependencyInjection;
using SudokuApp.Core.Contexes;
using SudokuApp.Core.Helpers;
using SudokuApp.Core.Services;
using System.Reflection;

namespace SudokuApp
{
    public partial class Sudoku : Form
    {
        private readonly SudokuContext _dbContext;
        private readonly SolutionService _solutionService;

        public Sudoku()
        {
            InitializeComponent();

            _dbContext = AppServiceProvider.ServiceProvider.GetRequiredService<SudokuContext>();
            _solutionService = AppServiceProvider.ServiceProvider.GetRequiredService<SolutionService>();
        }

        ResultObject result;

        private void Sudoku_Load(object sender, EventArgs e)
        {
            result = new();

            Form_Init();
        }
        private void Form_Init()
        {
            string opc = $"{MethodBase.GetCurrentMethod().DeclaringType.FullName}.{MethodBase.GetCurrentMethod().Name}";
            string title = "Form_Init";

            try
            {
                _dbContext.Database.EnsureCreated();

                _solutionService.FindSolution(out result);

            }
            catch (Exception ex)
            {
                ShowMessage.ShowResultMb(SystemMessageManager.MethodExepcionResultObject(title, opc, ex));
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            _solutionService.FindSolution(out result, 10000, 1000000);

        }
    }
}
