# User Import/Export WPF Application

A WPF application designed to efficiently import user data from specific file formats, persist it to a database, and export it to various output formats.

## Overview

The application provides a streamlined workflow for handling user data:
- **Import**: Read user data from CSV files with validation
- **Persist**: Store validated data in a SQL Server database using Entity Framework Core
- **Export**: Generate reports in Excel (.xlsx) or XML (.xml) formats

Currently supported formats:
- **Import**: CSV (.csv)
- **Export**: Excel (.xlsx), XML (.xml)

## Technology Stack

| Technology | Purpose |
|------------|---------|
| **WPF** | User Interface Framework |
| **CommunityToolkit.Mvvm** | MVVM pattern implementation with source generators |
| **Entity Framework Core** | Database communication and ORM |
| **EFCore.BulkExtensions** | Efficient bulk database operations |
| **FluentValidation** | Data validation and business rules |
| **Mapster** | Object mapping between DTOs and entities |
| **CsvHelper** | CSV file parsing and import |
| **ClosedXML** | Excel file export (.xlsx) |

## Test dataset

.csv file, which was tested is in the root folder of the project
