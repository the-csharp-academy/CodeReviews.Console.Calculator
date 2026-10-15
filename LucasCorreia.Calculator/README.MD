# Smart Calculator Challenge
 
A C# console calculator built from the official Microsoft tutorial: [Tutorial: Create a .NET console application using Visual Studio](https://learn.microsoft.com/en-us/visualstudio/get-started/csharp/tutorial-console?view=vs-2022).
 
The project has two versions: a text-based calculator and a voice-controlled calculator that uses the Azure AI Speech service.
 
## Features
 
- [x] Counter that tracks how many times the calculator has been used.
- [x] History of recent calculations, with an option to clear the list.
- [x] Reuse of results from the history in new calculations.
- [x] Additional operations: square root, 10x, power, and trigonometric functions.
## Prerequisites
 
- [.NET SDK](https://dotnet.microsoft.com/download) installed.
- For the voice version: a Microsoft account with access to Azure.
## How to Run
 
### Text calculator
 
```bash
cd calculadora
dotnet run
```
 
### Voice calculator
 
This version uses the Azure AI Speech service, so you need to create the resource and get its credentials before running it.
 
1. Create a Microsoft account if you don't have one.
2. Go to the [Azure portal](https://portal.azure.com) and sign in.
3. Activate the Azure free account.
4. In the portal, search for the **Speech** service (Azure AI Speech).
5. Create a new resource for that service.
6. Open the **Keys and Endpoint** section of the resource and copy **Key 1** and the **Location/Region** (for example, `brazilsouth`).
7. In the code, fill in the fields with the copied values:
```csharp
// Speech service API key
readonly static string _speechKey = "YOUR_KEY";
 
// Resource region (e.g. brazilsouth)
readonly static string _endpoint = "YOUR_REGION";
```
 
8. Run the project:
```bash
cd calculadora-ia
dotnet run
```
