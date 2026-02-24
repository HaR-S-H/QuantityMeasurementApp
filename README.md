# QuantityMeasurementApp

## 📌 Project Overview
QuantityMeasurementApp is a domain-driven C# application developed incrementally using a hybrid development approach that combines:

- Design–Develop–Test (DDT)
- Test-Driven Development (TDD)

Each use case introduces new functionality in a structured, maintainable, and scalable way.

---

# 🚀 Use Case 2 (UC2) – Feet and Inches Measurement Equality

## Description
Use Case 2 extends UC1 by adding equality comparison support for Inches measurement along with Feet measurement.

### ⚠ Important Notes
- Feet and Inches are treated as separate value objects.
- Cross-unit comparison is not supported in this stage (e.g., 1 ft is not equal to 12 inches).
- Equality comparison works only between measurements of the same unit type.

## ✅ Features Implemented in UC2
- Immutable Feet value object.
- Immutable Inches value object.
- Proper implementation of `Equals()` and `GetHashCode()`.
- `IEquatable<T>` implementation for both classes.
- Static equality methods in `QuantityMeasurementService`.
- MSTest unit tests for Feet and Inches.
- Clean architecture with separate source and test layers.

---

# 🚀 Use Case 3 (UC3) – QuantityLength with Unit Conversion (DRY)

## Description
Use Case 3 refactors the duplicated logic in Feet and Inches into a single `QuantityLength` class using a `LengthUnit` enum. This removes redundancy and improves maintainability.

## Problems Solved
- Removes code duplication between Feet and Inches.
- Supports cross-unit comparison (e.g., 1 ft equals 12 inches) using a shared conversion mechanism.
- Makes it easier to add new units without creating separate classes.

## ✅ Features Implemented in UC3
- `LengthUnit` enum with conversion factors.
- Unit-aware `QuantityLength` value object.
- Cross-unit equality through conversion to a base unit (feet).
- Updated `QuantityMeasurementService` to support QuantityLength.
- New unit tests for both same-unit and cross-unit comparisons.

## 🧠 Concepts Demonstrated in UC3
- DRY principle (single class for multiple units)
- Conversion logic abstraction
- Polymorphism using enum
- Value-based equality with unit conversion
- Scalability for adding new units

---

# 🚀 Use Case 4 (UC4) – Extended Unit Support (Yards & Centimeters)

## Description
Use Case 4 expands UC3 by adding Yards and Centimeters as additional length units in the `QuantityLength` class. This demonstrates the scalability of the generic design without introducing new classes.

## Problems Solved
- Validates the DRY design by supporting new units with minimal changes.
- Shows that adding units requires only enum modification.
- Enables complex multi-unit comparisons (yards ↔ feet ↔ inches ↔ centimeters).

## ✅ Features Implemented in UC4
- Extended `LengthUnit` enum with Yards and Centimeters.
- Conversion factors:
  - 1 Yard = 3 Feet
  - 1 Centimeter = 1/30.48 Feet
- No modification required in `QuantityLength`.
- 32 additional test cases for yard, centimeter, and multi-unit comparisons.
- Full backward compatibility with UC1, UC2, and UC3.

## 🧠 Concepts Demonstrated in UC4
- Scalable generic design
- Centralized conversion factor management
- Unit conversion through a common base unit
- Enum extensibility
- Mathematical precision in conversions
- DRY principle validation
- Backward compatibility

---

# 🧠 Core Concepts Demonstrated
- Equality contract principles:
  - Reflexive
  - Symmetric
  - Transitive
  - Consistent
  - Null handling
- Type safety
- Value-based equality
- Floating-point comparison using `double.Compare()`
- Encapsulation and immutability
- SOLID principles
- Clean project architecture
- Professional Git workflow

---

# 🛠 Tech Stack
- .NET 8
- C#
- MSTest
- Git

---

# 📂 Project Structure

QuantityMeasurementApp.sln

src/
└── QuantityMeasurementApp
    ├── Program.cs
    ├── Models/
    │   ├── Feet.cs
    │   ├── Inches.cs
    │   ├── LengthUnit.cs
    │   └── Quantity.cs
    └── Services/
        └── QuantityMeasurementService.cs

tests/
└── QuantityMeasurementApp.Tests
    ├── FeetTests.cs
    ├── InchesTests.cs
    └── QuantityTests.cs

---

# ▶ How to Run the Application

1. Clone the repository:
   git clone <repository-url>

2. Navigate to the project folder:
   cd QuantityMeasurementApp

3. Build the project:
   dotnet build

4. Run the application:
   cd src/QuantityMeasurementApp
   dotnet run

## Expected Output

Input: 1.0 inch and 1.0 inch  
Output: Equal (True)

Input: 1.0 ft and 1.0 ft  
Output: Equal (True)

### UC3 Example
Input: Quantity(1.0, Feet) and Quantity(12.0, Inches)  
Output: Equal (True)

### UC4 Examples
Input: Quantity(1.0, Yards) and Quantity(3.0, Feet)  
Output: Equal (True)

Input: Quantity(1.0, Yards) and Quantity(36.0, Inches)  
Output: Equal (True)

Input: Quantity(2.0, Centimeters) and Quantity(2.0, Centimeters)  
Output: Equal (True)

Input: Quantity(1.0, Centimeters) and Quantity(0.393701, Inches)  
Output: Equal (True)

---

# 🧪 How to Run Unit Tests

From the solution root directory:

dotnet test

All test cases must pass before merging changes into the develop or main branch.