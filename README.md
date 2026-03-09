# Quantity Measurement Application

## UC13: Centralized Arithmetic Logic (DRY Principle)

### Motivation
Prior to UC13, arithmetic operations (add, subtract, divide) in the Quantity<U> class repeated validation and conversion logic, violating the DRY (Don't Repeat Yourself) principle. UC13 refactors the code to centralize all arithmetic logic, improving maintainability, consistency, and scalability.

### Key Improvements
- **Centralized Helper Method**: All arithmetic operations now delegate to a single private helper that performs validation, base-unit conversion, and the arithmetic operation.
- **Enum-Based Operation Dispatch**: An internal enum (ArithmeticOperation) is used to cleanly dispatch between addition, subtraction, and division, making it easy to add new operations in the future.
- **Consistent Validation**: Null checks, category compatibility, and value finiteness are validated in one place, ensuring uniform error handling and messaging.
- **Immutability**: All arithmetic methods return new Quantity<U> objects, preserving the original operands.
- **Extensibility**: Adding new operations (e.g., multiplication) requires minimal code changes.

### Example
```csharp
// Addition
Quantity<LengthUnit>.Add(q1, q2);
// Subtraction
Quantity<LengthUnit>.Subtract(q1, q2);
// Division
Quantity<LengthUnit>.Divide(q1, q2);
```
All of these methods now use the same centralized logic for validation and conversion.

### Benefits
- Eliminates code duplication across arithmetic methods
- Ensures all operations behave consistently
- Simplifies bug fixes and future enhancements
- Follows SOLID and DRY principles

### Error Handling
All validation errors (null operands, cross-category, invalid values, division by zero) are handled in the centralized helper, providing clear and consistent error messages.

---
