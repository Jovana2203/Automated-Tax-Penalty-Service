using System;
using System.Collections.Generic;

// Enum defines the possible states of a taxpayer's compliance
public enum ComplianceStatus { Compliant, UnderReview, Delinquent }

// BASE CLASS: Demonstrates Encapsulation
public class Taxpayer
{
    public string Name { get; set; }
    public string TaxID { get; set; }
    public double Balance { get; set; }
    public DateTime DueDate { get; set; } 
    public ComplianceStatus Status { get; set; }

    // Constructor to initialize essential taxpayer data
    public Taxpayer(string name, string id, double balance, DateTime dueDate)
    {
        Name = name;
        TaxID = id;
        Balance = balance;
        DueDate = dueDate;
        Status = ComplianceStatus.Compliant; // Initial default status
    }

    // Logic to calculate elapsed time since the deadline
    public int GetDaysOverdue()
    {
        if (DateTime.Now <= DueDate) return 0;
        
        // Using TimeSpan to extract the total days difference
        return (DateTime.Now - DueDate).Days;
    }
}

// DERIVED CLASS: Demonstrates Inheritance
public class BusinessTaxpayer : Taxpayer
{
    public double AnnualRevenue { get; set; }
    public int EmployeeCount { get; set; }

    // Computed Property: Automatically determines category based on business rules
    public bool IsLargeCorporation 
    { 
        get 
        { 
            return AnnualRevenue > 10000000 || EmployeeCount > 250; 
        } 
    }

    // Constructor calling 'base' to initialize parent properties
    public BusinessTaxpayer(string name, string id, double balance, DateTime dueDate, double revenue, int employees) 
        : base(name, id, balance, dueDate)
    {
        AnnualRevenue = revenue;
        EmployeeCount = employees;
    }
}

// SERVICE LAYER: Handles business logic and state transitions
public class ComplianceService
{
    public void EvaluateTaxpayer(Taxpayer tp)
    {
        int daysLate = tp.GetDaysOverdue();

        // RULE 1: Critical delinquency (Over 30 days late)
        if (daysLate > 30 && tp.Balance > 0)
        {
            // Idempotency check: Apply penalty only if it hasn't been applied yet
            if (tp.Status != ComplianceStatus.Delinquent)
            {
                // Polymorphic check to apply different penalty rates
                if (tp is BusinessTaxpayer biz && biz.IsLargeCorporation)
                {
                    tp.Balance *= 1.20; // 20% penalty for large corporations
                }
                else
                {
                    tp.Balance *= 1.10; // 10% penalty for individuals or small businesses
                }
            }
            tp.Status = ComplianceStatus.Delinquent;
        }
        // RULE 2: Warning status (1-30 days late)
        else if (daysLate > 0 && tp.Balance > 0 && tp.Status != ComplianceStatus.Delinquent)
        {
            tp.Status = ComplianceStatus.UnderReview;
        }
        // RULE 3: Account is compliant
        else if (tp.Status != ComplianceStatus.Delinquent && tp.Status != ComplianceStatus.UnderReview)
        {
            tp.Status = ComplianceStatus.Compliant;
        }
    }
}

public class Program
{
    public static void Main()
    {
        ComplianceService service = new ComplianceService();
        List<Taxpayer> taxFleet = new List<Taxpayer>();

        // Adding various test scenarios to the fleet
        taxFleet.Add(new Taxpayer("Jovana M", "TX-001", 0, new DateTime(2026, 4, 15)));
        taxFleet.Add(new Taxpayer("Future Corp", "TX-002", 1000.0, DateTime.Now.AddDays(-5)));
        taxFleet.Add(new Taxpayer("Bad Payee", "TX-003", 5000.0, DateTime.Now.AddDays(-45)));
		
        // Creating a large business entity
        BusinessTaxpayer bigCorp = new BusinessTaxpayer("Global Tech", "999-BC", 10000, DateTime.Now.AddDays(-40), 15000000, 500);
		
        // Processing business entity and adding to the fleet for reporting
        taxFleet.Add(bigCorp);

        Console.WriteLine("\n--- TAX COMPLIANCE REPORT ---");
        Console.WriteLine("NAME".PadRight(15) + "| STATUS".PadRight(15) + "| BALANCE".PadRight(12) + "| DAYS LATE");
        Console.WriteLine(new string('-', 55));

        foreach (var tp in taxFleet)
        {
            // The service encapsulates the decision-making logic
            service.EvaluateTaxpayer(tp); 

            Console.WriteLine(
                tp.Name.PadRight(15) + "| " + 
                tp.Status.ToString().PadRight(13) + "| " + 
                tp.Balance.ToString("F2").PadRight(10) + "| " + 
                tp.GetDaysOverdue()
            );
        }
    }
}
