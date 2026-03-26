CREATE TABLE Taxpayers (
    TaxID VARCHAR(20) PRIMARY KEY,
    Name NVARCHAR(100),
    Balance DECIMAL(18, 2),
    DueDate DATE,
    Status NVARCHAR(20),
    AnnualRevenue DECIMAL(18, 2),
    EmployeeCount INT
);

INSERT INTO Taxpayers (TaxID, Name, Balance, DueDate, Status, AnnualRevenue, EmployeeCount)
VALUES 
('TX-001', 'Jovana M', 0, '2026-04-15', 'Compliant', 0, 0),
('TX-002', 'Future Corp', 1000.00, DATEADD(day, -5, GETDATE()), 'Compliant', 500000, 10),
('TX-003', 'Bad Payee', 5000.00, DATEADD(day, -45, GETDATE()), 'Compliant', 0, 0),
('999-BC', 'Global Tech', 10000.00, DATEADD(day, -40, GETDATE()), 'Compliant', 15000000, 500);
