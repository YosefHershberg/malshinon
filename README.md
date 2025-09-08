# Malshinon — Community Intel Reporting System

## Project Overview
Malshinon is a simulated intelligence platform for collecting and analyzing community reports. Civilians (tattletales) submit reports about others (targets), and the system identifies potential informants, high-risk individuals, and suspicious activity patterns.

## Features
- **Intel Reporting Workflow**: Submit reports, identify reporter/target by name or secret code, auto-create new people.
- **Secret Code Management**: Retrieve or generate secret codes for individuals.
- **CSV Import**: Bulk import reports from CSV files.
- **Analysis Dashboard**: View potential recruits, dangerous targets, and triggered alerts.
- **Logging**: All key activities and errors are logged.

## Setup Instructions
1. **MySQL Setup**:
   - Run `mysql_schema.sql` to create the database tables.
2. **Configure Connection**:
   - Update the connection string in the app to match your MySQL server.
3. **Build & Run**:
   - `dotnet build`
   - `dotnet run`

## Usage
- **Submit Report**: Follow CLI prompts to log in, select target, and submit a report.
 - **Import CSV**: Use the import option, then enter a path to a CSV file or press Enter to use `./sample_import.csv`.
 - **Dashboard**: View analytics and alerts.

## Analysis Logic
- **Potential Recruits**: ≥10 reports, avg. report length ≥100 chars.
- **Dangerous Targets**: ≥20 mentions or ≥3 mentions in 15 minutes.
- **Alerts**: Triggered on status change or burst of reports.

## Example CSV
See `sample_import.csv` for format.

## Author
Yosef Hershberg
