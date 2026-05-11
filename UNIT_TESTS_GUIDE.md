# DotDev Unit Tests Guide

## Overview

This guide covers the comprehensive unit test suite for the DotDev Core business logic. Tests are written using **xUnit** with **Moq** for mocking, following the project's established patterns.

## Test Files Created

### 1. **HoneycombTests.cs**
Tests the core hex collection management system.

**Key Test Coverage:**
- `AddItem()` with empty hex map
- Ghost vs. non-ghost replacement logic
  - Ghosts cannot replace existing items
  - Non-ghosts replace ghosts
  - Non-ghosts cannot replace non-ghosts
  - Ghosts cannot replace ghosts
- Root addition (`AddRoot()`)
- Ghost discovery and enabling (`AddGhosts()`, `EnableGhosts()`)
- Event handling (`HoneycombChanged`)
- Multiple item management with proper indexing

**Critical Business Logic Tested:**
```csharp
// Non-ghost items replace ghosts, but ghosts never replace anything
if (item.IsGhost)
{
    return false; // Ghosts can't replace
}

var existing = hexMap[item.GridIndex];
if (existing.IsGhost)
{
    hexMap[item.GridIndex] = item; // Non-ghost replaces ghost
    HoneycombChanged?.Invoke(this, new EventArgs());
    return true;
}
return false; // Non-ghost can't replace non-ghost
```

**12 test cases** covering all code paths and edge cases.

### 2. **HexLocationTests.cs**
Tests coordinate and movement logic for hex grid positions.

**Key Test Coverage:**
- Location initialization with GridIndex, Row, Column
- Location equality
- Movement in hexagon directions (6 directions)
- Unsigned integer coordinate handling
- Edge cases (zero and maximum values)

**8 test cases** ensuring coordinate operations are reliable.

### 3. **HexStyleTests.cs**
Tests the geometric styling and SVG rendering properties.

**Key Test Coverage:**
- Constant size generation (`Size = "100"`)
- SVG viewBox generation
- Hexagon vertex point calculation (6 vertices)
- Coordinate parsing and validation
- CSS max-size properties
- Record equality semantics
- Ghost property management

**11 test cases** validating geometric calculations and styling.

### 4. **HexItemTests.cs**
Tests the hex item hierarchy: IntroHex, StatusHex, AboutHex, BlankItem.

**StatusHex Tests (7 test cases):**
- Enable/disable ghost state
- Default style and location
- Location and style initialization

**AboutHex Tests (8 test cases):**
- Enable/disable ghost state
- Default style, location, and text
- Console text presence when enabled

**IntroHex Tests (8 test cases):**
- Non-ghost initialization
- Default properties (style, location, text)
- Unlock configuration (About + Status)
- Location and style setup

**BlankItem Tests (3 test cases):**
- Basic initialization
- Location tracking
- GridIndex consistency

**Total: 26 test cases** for the hex item hierarchy.

## Test Statistics

| Test File | Test Cases | Focus Area |
|-----------|-----------|-----------|
| HoneycombTests.cs | 12 | Hex collection management |
| HexLocationTests.cs | 8 | Coordinate operations |
| HexStyleTests.cs | 11 | Geometric styling |
| HexItemTests.cs | 26 | Hex item types |
| **SquareModelTests.cs** | 15 | Element/server status (existing) |
| **Other existing tests** | ~20 | Navigation, disposal, etc. |
| **Total** | **92+** | Comprehensive coverage |

## Running the Tests

### Command Line

Run all tests:
```bash
dotnet test Core.Tests
```

Run specific test file:
```bash
dotnet test Core.Tests --filter "HoneycombTests"
```

Run with verbose output:
```bash
dotnet test Core.Tests -v detailed
```

Run with coverage report:
```bash
dotnet test Core.Tests --collect:"XPlat Code Coverage"
```

### Visual Studio

1. **Test Explorer**: View > Test Explorer (Ctrl+E, T)
2. **Run All**: Click "Run All Tests in View"
3. **Run Single**: Right-click test → Run
4. **Debug**: Right-click test → Debug

### Visual Studio Code

1. **C# Dev Kit Extension** (recommended)
2. **Test Explorer** icon in activity bar
3. Click test name to run or debug

## Key Testing Patterns

### 1. Arrange-Act-Assert (AAA)
```csharp
[Fact]
public void AddItem_WithEmptyHexMap_AddsItemSuccessfully()
{
    // Arrange
    var location = new HexLocation { GridIndex = 1, Row = 1, Column = 1 };
    var item = new StatusHex(location, enable: true);

    // Act
    var result = _honeycomb.AddItem(item);

    // Assert
    Assert.True(result);
    Assert.Contains(item, _honeycomb.GetItems());
}
```

### 2. Event Testing
```csharp
var eventRaised = false;
_honeycomb.HoneycombChanged += (_, _) => eventRaised = true;
_honeycomb.AddItem(item);
Assert.True(eventRaised);
```

### 3. State Verification
```csharp
var ghostItem = new StatusHex(location, enable: false);
var nonGhostItem = new StatusHex(location, enable: true);
_honeycomb.AddItem(ghostItem);
_honeycomb.AddItem(nonGhostItem);
Assert.DoesNotContain(ghostItem, _honeycomb.GetItems());
Assert.Contains(nonGhostItem, _honeycomb.GetItems());
```

## Test Organization

```
Core.Tests/
├── HoneycombTests.cs
├── HexLocationTests.cs
├── HexStyleTests.cs
├── HexItemTests.cs
├── SquareModelTests.cs (existing)
├── ServerInfoStatusTests.cs (existing)
└── ... (other existing tests)
```

## Coverage Goals

- **Honeycomb**: 100% (all code paths, edge cases)
- **HexItem Hierarchy**: 95%+ (initialization and state)
- **HexLocation**: 90%+ (coordinate operations)
- **HexStyle**: 90%+ (geometric calculations)

## Next Steps

1. **Run tests locally** to verify they pass in your environment
2. **Add integration tests** for Honeycomb + HexItem interactions
3. **Test async operations** (AddGhosts, EnableGhosts, UnlockAsync)
4. **Performance tests** for large hex collections
5. **Event serialization tests** for real-time updates

## Dependencies

- **xUnit 2.9.3** - Test framework
- **Moq 4.20.72** - Mocking library (for future mock-based tests)
- **coverlet.collector 6.0.4** - Code coverage

All dependencies are already in `Core.Tests.csproj`.

## Continuous Integration

Add to your CI/CD pipeline:
```bash
dotnet test Core.Tests --logger "json" --collect:"XPlat Code Coverage"
```

This enables:
- Test result tracking
- Code coverage reporting
- Failure notifications
