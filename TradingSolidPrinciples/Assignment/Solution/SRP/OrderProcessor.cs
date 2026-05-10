// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 1: SRP
//
//  Single Responsibility: orchestrate the processing workflow only.
//  Reason to change: only if the workflow steps change.
//
//  This class delegates every detail to a specialist class.
//  It knows WHAT to do, not HOW to do it.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.SRP;

public class OrderProcessor
{
    private readonly OrderValidator  _validator;
    private readonly OrderStorage    _storage;
    private readonly OrderEmailSender _emailSender;

    public OrderProcessor(
        OrderValidator   validator,
        OrderStorage     storage,
        OrderEmailSender emailSender)
    {
        _validator   = validator;
        _storage     = storage;
        _emailSender = emailSender;
    }

    public void Process(Order order)
    {
        if (!_validator.IsValid(order, out var reason))
        {
            Console.WriteLine($"[SKIP] {reason}");
            return;
        }

        _storage.Save(order);
        _emailSender.SendConfirmation(order);
    }
}
