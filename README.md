Project Overview

This repository contains a core logic engine for a Tax Compliance Management System. It simulates how a government or financial entity processes taxpayers, categorizes business entities, and automatically applies penalties based on payment deadlines.

The project was built to demonstrate robust Object-Oriented Programming (OOP) principles and Business Logic Validation, specifically focusing on common scenarios found in ERP systems like GenTax.

Key Technical Features

* Automated Penalty Calculation: The system distinguishes between minor delays (Grace Period) and serious delinquency (30+ days).

* Smart Business Classification: Uses Computed Properties to automatically classify companies as "Large Corporations" based on Revenue and Employee count without manual flags.

* Idempotency Protection: Implemented logic to prevent "Double Penalization" if the processing service runs multiple times on the same record.

* State Management: A dedicated ComplianceService handles transitions between Compliant, UnderReview, and Delinquent statuses.

OOP Concepts Applied

* Inheritance: BusinessTaxpayer inherits from the base Taxpayer class, extending it with corporate-specific data.

* Encapsulation: Taxpayer data is protected, and logic like GetDaysOverdue() is contained within the object it belongs to.

* Polymorphism: The system processes a List<Taxpayer> that can contain both individuals and businesses, identifying the specific type at runtime to apply correct tax laws.

* Abstraction: Complex date arithmetic (using TimeSpan) is hidden behind simple method calls.
