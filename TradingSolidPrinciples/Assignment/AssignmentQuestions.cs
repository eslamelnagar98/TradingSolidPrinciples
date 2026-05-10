// ╔══════════════════════════════════════════════════════════════════════╗
//
//         S O L I D   P R I N C I P L E S   —   A S S I G N M E N T
//                      Domain: Order Processing System
//                      Time allowed: 30 minutes
//
//  Instructions:
//    Open OrderProcessor_Messy.cs.
//    It contains a single class that violates ALL 5 SOLID principles.
//    For each task below:
//      1. Identify WHERE and WHY the principle is violated.
//      2. Refactor the code to fix the violation.
//      3. You should end up with multiple small, focused classes.
//
//  You are NOT allowed to delete business logic — only reorganise it.
//
// ╚══════════════════════════════════════════════════════════════════════╝

namespace TradingSolidPrinciples.Assignment;

/*
 ════════════════════════════════════════════════════════════════════════
  TASK 1 — Single Responsibility Principle                    (5 minutes)
 ════════════════════════════════════════════════════════════════════════

  The Problem:
    OrderProcessor currently does ALL of the following:
      • Validates the order
      • Calculates the discount
      • Saves the order to the database
      • Sends a confirmation email
      • Generates a sales report
      • Exports orders to CSV

  Your Task:
    Split OrderProcessor into separate classes, each with ONE responsibility.

  Hint:
    Ask yourself: "Why would this code change?"
    If you can give two different reasons, it belongs in two different classes.

  Expected Result:
    ✔ OrderValidator      — only validates orders
    ✔ OrderStorage        — only saves orders
    ✔ OrderEmailSender    — only sends emails
    ✔ OrderReportService  — only generates reports / exports
    ✔ OrderProcessor      — only orchestrates: validate → discount → save → notify

 ════════════════════════════════════════════════════════════════════════
  TASK 2 — Open/Closed Principle                              (5 minutes)
 ════════════════════════════════════════════════════════════════════════

  The Problem:
    The GetDiscount() method uses an if/else chain on OrderType string.
    Adding a new order type (e.g. "VIP") requires editing this method.
    That means the class is OPEN for modification — which violates OCP.

  Your Task:
    Replace the if/else chain with a strategy pattern so that:
      • Adding a new discount type = adding a NEW class, not editing existing ones.

  Hint:
    Create an IDiscountStrategy interface with one method:
      decimal GetDiscount(Order order);
    Then create one class per order type that implements it.

  Expected Result:
    ✔ IDiscountStrategy       — the closed contract
    ✔ StandardDiscountStrategy — 0% discount
    ✔ PremiumDiscountStrategy  — 10% discount
    ✔ BulkDiscountStrategy     — 20% discount
    ✔ OrderProcessor           — receives IDiscountStrategy, never changes for new types

 ════════════════════════════════════════════════════════════════════════
  TASK 3 — Liskov Substitution Principle                      (5 minutes)
 ════════════════════════════════════════════════════════════════════════

  The Problem:
    ArchiveOrderStorage extends SqlOrderStorage but throws
    NotSupportedException inside Save().
    Any caller using SqlOrderStorage will crash if ArchiveOrderStorage
    is substituted in — it breaks the base class contract.

  Your Task:
    Fix the hierarchy so that every type can be safely substituted
    without throwing unexpected exceptions.

  Hint:
    Ask: "Does ArchiveOrderStorage share the SAME contract as SqlOrderStorage?"
    Answer: No — it can only READ, not WRITE.
    Solution: Split into two focused interfaces instead of one base class.

  Expected Result:
    ✔ IOrderWriter           — contract: Save(Order order)
    ✔ IOrderReader           — contract: IEnumerable<Order> GetAll()
    ✔ SqlOrderStorage        — implements IOrderWriter + IOrderReader
    ✔ ArchiveOrderStorage    — implements IOrderReader ONLY (no forced Save)

 ════════════════════════════════════════════════════════════════════════
  TASK 4 — Interface Segregation Principle                    (5 minutes)
 ════════════════════════════════════════════════════════════════════════

  The Problem:
    IOrderService has 4 methods: ProcessOrder, SendConfirmationEmail,
    GenerateReport, ExportToCsv.
    A class that only needs to send emails is still forced to implement
    ProcessOrder, GenerateReport, and ExportToCsv — methods it does not need.

  Your Task:
    Split IOrderService into smaller, focused interfaces.

  Hint:
    Group by "who uses this?"
      Processing logic → IOrderProcessor
      Notification logic → IOrderNotifier
      Reporting logic → IOrderReporter

  Expected Result:
    ✔ IOrderProcessor  — ProcessOrder(Order order)
    ✔ IOrderNotifier   — SendConfirmationEmail(Order order)
    ✔ IOrderReporter   — GenerateReport(...) + ExportToCsv(...)
    ✔ Each implementation class only implements the interface it needs

 ════════════════════════════════════════════════════════════════════════
  TASK 5 — Dependency Inversion Principle                     (10 minutes)
 ════════════════════════════════════════════════════════════════════════

  The Problem:
    OrderProcessor uses new() to create its own dependencies:
      private readonly SqlOrderStorage  _storage = new SqlOrderStorage();
      private readonly SmtpEmailSender  _emailer = new SmtpEmailSender();
      private readonly FileOrderLogger  _logger  = new FileOrderLogger();

    This means:
      • To swap SQL → MongoDB you must edit OrderProcessor.
      • To swap SMTP → SendGrid you must edit OrderProcessor.
      • You cannot unit-test OrderProcessor without a real DB and email server.

  Your Task (3 parts):

    Part A — Apply DIP:
      Replace all new() calls with interface dependencies.
      Create: IOrderStorage, IOrderEmailSender, IOrderLogger interfaces.

    Part B — Apply Constructor Injection:
      Receive all interfaces via the constructor.
      OrderProcessor should never call new() for any dependency.

    Part C — Wire it up (IoC Container style):
      In a static Setup() method, manually wire all the dependencies
      and create an OrderProcessor — simulating what an IoC container does.

  Hint — DIP vs DI reminder:
    DIP  = the RULE:      "Depend on interfaces, not concrete classes."
    DI   = the TECHNIQUE: "Push those interfaces in from outside."
    They are related but NOT the same thing.

  Expected Result:
    ✔ IOrderStorage      — abstracts where orders are saved
    ✔ IOrderEmailSender  — abstracts how emails are sent
    ✔ IOrderLogger       — abstracts where logs go
    ✔ SqlOrderStorage    — implements IOrderStorage
    ✔ SmtpEmailSender    — implements IOrderEmailSender
    ✔ ConsoleOrderLogger — implements IOrderLogger
    ✔ OrderProcessor     — receives all 3 via constructor, zero new() calls

 ════════════════════════════════════════════════════════════════════════
  BONUS — Can you do all 5 together?                          (remaining time)
 ════════════════════════════════════════════════════════════════════════

  Combine all your fixes into a single clean design where:
    ✔ Every class has ONE reason to change              (SRP)
    ✔ Adding a new discount type = new class only       (OCP)
    ✔ Every implementation is safely substitutable      (LSP)
    ✔ No class implements methods it does not need      (ISP)
    ✔ OrderProcessor depends only on interfaces         (DIP)

*/

// This file is intentionally left without executable code.
// It is the student question sheet — see Solution/ folder for answers.
public static class AssignmentQuestions { }
