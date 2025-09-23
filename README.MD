# PO SK Process Framework

## Description

A .NET-based framework for processing purchase orders using Semantic Kernel and Azure AI agents. This project provides a modular, step-based approach to handling purchase order workflows with AI-powered decision making and processing capabilities.

## Features

- **Modular Processing Steps**: Configurable steps for intake, processing, validation, and completion of purchase orders
- **AI Integration**: Leverages Azure AI and Semantic Kernel for intelligent processing
- **Extensible Architecture**: Easy to add new steps and customize workflows
- **Purchase Order Management**: Comprehensive handling of purchase order data and states

## Installation

1. **Prerequisites**
   - .NET 8.0 SDK
   - Azure subscription (for AI services)

2. **Clone the Repository**
   ```bash
   git clone https://github.com/ckriutz/po_skprocessframework.git
   cd po_skprocessframework
   ```

3. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

4. **Build the Project**
   ```bash
   dotnet build
   ```

## Usage

1. **Configure Azure AI Services**
   - Set up your Azure AI resources
   - Update configuration with your API keys and endpoints

2. **Run the Application**
   ```bash
   dotnet run --project src/SkProcessTest.csproj
   ```

3. **Customize Workflows**
   - Modify steps in the `src/Steps/` directory
   - Update process logic in `src/Processes/`

## Project Structure

- `src/Program.cs` - Main application entry point
- `src/PurchaseOrder.cs` - Purchase order data model
- `src/Processes/` - Process definitions
- `src/Steps/` - Individual processing steps
- `src/PurchaseOrders/` - Sample purchase order data

## Dependencies

- .NET 8.0
- Azure.AI.Agents.Persistent
- Microsoft.SemanticKernel
- Azure.AI.OpenAI
- Azure.Core
- And more (see `src/SkProcessTest.csproj`)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

See [LICENSE](LICENSE) file for details.