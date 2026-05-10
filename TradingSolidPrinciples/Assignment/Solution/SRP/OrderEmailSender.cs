// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 1: SRP
//
//  Single Responsibility: send notification emails only.
//  Reason to change: only if the email format or transport changes.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.SRP;

public class OrderEmailSender
{
    public void SendConfirmation(Order order) =>
        Console.WriteLine(
            $"[EMAIL] To:{order.CustomerEmail} — Order {order.Id} confirmed.");
}
