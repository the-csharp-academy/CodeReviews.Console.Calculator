using CalculatorLibrary.Controllers;
using CalculatorLibrary.Services;
using CalculatorLibrary.Views;

namespace CalculatorLibrary;

public class Calculator
{
    private readonly CalculatorService _calculatorService;
    private readonly ExpressionParserService _expressionParserService;
    private readonly HistoryService _historyService;
    private readonly LogService _logService;
    private readonly CalculatorView _calculatorView;
    private readonly CalculatorController _calculatorController;

    public Calculator()
    {
        _calculatorService = new CalculatorService();
        _historyService = new HistoryService();
        _expressionParserService = new ExpressionParserService(_historyService);
        _logService = new LogService();
        _calculatorView = new CalculatorView();
        _calculatorController = new CalculatorController(_calculatorService, _expressionParserService, _historyService, _logService, _calculatorView);
    }
    public void Run()
    {
        _calculatorController.Run();
    }
}
