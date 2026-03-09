# Quantity Measurement Application

## Overview

The Quantity Measurement Application is a generic, extensible .NET system for comparing, converting, and performing arithmetic on physical quantities across multiple measurement categories. It supports length, weight, volume, and temperature, with a focus on type safety, SOLID principles, and maintainability.

## Features

- **Generic Quantity<U> Class**: Supports any measurable unit type (length, weight, volume, temperature).
- **Unit Conversion**: Accurate conversion between units within the same category (e.g., feet to inches, kilograms to grams, Celsius to Fahrenheit).
- **Equality Comparison**: Quantities are compared by converting to a common base unit, supporting cross-unit equality (e.g., 0°C == 32°F).
- **Arithmetic Operations**: Addition, subtraction, and division for supported categories (length, weight, volume). Centralized logic ensures DRY and consistent validation.
- **Selective Arithmetic Support**: Temperature supports only equality and conversion; arithmetic operations (add, subtract, divide) are explicitly disallowed and throw clear exceptions.
- **Extensible Design**: Adding new categories or units requires minimal code changes.
- **Comprehensive Error Handling**: All operations validate input, unit compatibility, and operation support, with meaningful error messages.

## UC14: Temperature Measurement & Selective Arithmetic

### Motivation

Temperature measurements (Celsius, Fahrenheit) differ from other categories: arithmetic operations are not meaningful for absolute temperatures. UC14 introduces temperature support and refactors the system to allow categories to opt out of arithmetic operations.

### Key Changes

- **IMeasurable Interface Refactored**: Now includes default methods for operation support and validation. Units can override to restrict operations (e.g., temperature disables arithmetic).
- **TemperatureUnit Enum**: Implements conversion between Celsius and Fahrenheit. Throws `NotSupportedException` for arithmetic operations.
- **Quantity<U> Enhancements**: All arithmetic methods check if the unit supports the operation before proceeding. Attempts to add, subtract, or divide temperatures result in clear exceptions.
- **Backward Compatibility**: All previous features and tests for length, weight, and volume remain unaffected.

### Example Usage

#### Temperature Equality & Conversion

```
new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius).Equals(new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit)) // true
new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius).ConvertTo(TemperatureUnit.Fahrenheit) // Quantity(212.0, Fahrenheit)
```

#### Unsupported Operations

```
new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius).Add(new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius))
// Throws NotSupportedException: Operation 'add' is not supported for temperature measurements.
```

#### Supported Arithmetic (Other Categories)

```
new Quantity<LengthUnit>(10.0, LengthUnit.Feet).Add(new Quantity<LengthUnit>(6.0, LengthUnit.Inch)) // Quantity(10.5, Feet)
```

## Design Principles

- **SOLID & DRY**: Centralized validation and arithmetic logic. Interface Segregation allows categories to implement only what they support.
- **Type Safety**: Compile-time and runtime checks prevent cross-category operations.
- **Extensibility**: New units or categories can be added with minimal changes.

## Error Handling

- All invalid operations (e.g., arithmetic on temperature, cross-category arithmetic) throw clear, descriptive exceptions.
- All input values are validated for finiteness and compatibility.

## How to Extend

- **Add a New Unit**: Create a new enum and extension methods for conversion. Implement or override operation support as needed.
- **Add a New Category**: Implement the IMeasurable interface and provide conversion logic. Override operation support if arithmetic is not meaningful.

## Example Test Cases

- Temperature equality: 0°C == 32°F, 100°C == 212°F
- Temperature conversion: 100°C → 212°F, 32°F → 0°C
- Arithmetic on temperature: Throws NotSupportedException
- Arithmetic on length/weight/volume: Supported and validated

## License

MIT License

---

For more details, see the source code and test cases.
