# Calculator

## Running it locally

To use the Azure Speech Service we need a key to that service, in-order to not expose the secret key I used:
`cd ./Microsoft.Calculator`
`dotnet user-secrets set "SpeechService:key" "key_here"`
`dotnet user-secrets set "SpeechService:region" "region_here"`

Run the actual project from `Microsoft.Calculator`, this uses the `Microsoft.Calculator.CalculatorLibrary` for doing the logging/calculation logic.
`cd ./Microsoft.Calculator`
`dotnet run`
