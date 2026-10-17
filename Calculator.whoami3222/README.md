# Console Calculator

A .NET console application that performs basic arithmetic operations using either manual keyboard input or Azure Speech recognition.

## Overview

This project demonstrates a simple calculator experience in C# with a reusable `CalculatorLibrary` and a console UI. It allows users to:

- add
- subtract
- multiply
- divide

The app can operate in two modes:

1. Manual mode: users type numbers and select an operation from the console.
2. Speech mode: users speak numbers and operation names using Azure Speech.

## Features

- Basic arithmetic calculations
- Console-based interactive workflow
- Optional voice input using Azure Speech
- JSON operation logging
- Repeated calculations until the user exits

## Project Structure

```text
Calculator.whoami3222/
├── Calculator/
│   ├── Program.cs
│   ├── Calculator.csproj
│   ├── speech-key.txt
│   └── calculatorlog.json
├── CalculatorLibrary/
│   ├── CalculatorLibrary.cs
│   └── CalculatorLibrary.csproj
├── README.md
└── .gitignore
```

## Requirements

- .NET SDK 10.0 or later
- Azure Speech subscription key for speech recognition
- Microphone access if using speech input

## Setup

1. Navigate to the project folder.
2. Add your Azure Speech key to `Calculator/speech-key.txt`.
3. Restore dependencies:

```bash
dotnet restore
```

4. Run the application:

```bash
dotnet run --project Calculator/Calculator.csproj
```

## Usage

### Manual mode

When prompted, choose whether to use speech input:

- type `n` for manual mode
- enter the first number
- enter the second number
- choose the operation:
  - `a` = add
  - `s` = subtract
  - `m` = multiply
  - `d` = divide

### Speech mode

When prompted, choose whether to use speech input:

- type `y` for speech mode
- say the first number
- say the second number
- say the operation name such as:
  - "add"
  - "subtract"
  - "multiply"
  - "divide"

## Logging

The calculator writes each operation to a JSON file named `calculatorlog.json` in the `Calculator` folder.

## Notes

- Division by zero is handled safely and shows an error message instead of crashing.
- The app continues to prompt for more calculations until the user chooses to exit.
- If you do not provide a valid Azure Speech key, speech mode will fail with a clear error message.

## License

This project is intended for educational and personal use.
