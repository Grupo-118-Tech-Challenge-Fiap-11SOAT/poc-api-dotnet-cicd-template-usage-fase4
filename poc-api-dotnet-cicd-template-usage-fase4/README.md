# Variable Substitution Configuration Guide

This repository serves as a guide for configuring variable substitution in the deployment pipeline.

## Steps to Configure Variable Substitution

### Step 0: Add Support for Environment Variables
**File:** `Program.cs`

Add support for environment variables in your ASP.NET application:

```csharp
builder.Configuration.AddEnvironmentVariables();
```

This enables the application to read configuration from environment variables and override the values from `appsettings.json`.

### Step 1: Map Secrets to Helm Variables
**File:** `.github/workflows/cd.yml`

Map the GitHub secrets to Helm variables in the format that the API expects:

```yaml
secrets_mapping: |
    {
        "DOCKER_REGISTRY": "DOCKER_REGISTRY",
        "ACR_USERNAME": "ACR_USERNAME",
        "ACR_PASSWORD": "ACR_PASSWORD",
        "DatabaseConfigurations__Host": "GS_POC_DATABASE_HOST",
        "DatabaseConfigurations__Port": "GS_POC_DATABASE_PORT",
        "DatabaseConfigurations__Name": "GS_POC_DATABASE_NAME",
        "ApiConfigurations__Key": "GS_POC_API_KEY"
    }
```

### Step 2: Create Secret Keys with Mapped Values
**File:** `helm/values-production.yaml`

Create the keys in the Kubernetes secret with the values that were mapped from the GitHub secrets:

```yaml
secret:
  enabled: true
  data:
    poc-db-host: "${DatabaseConfigurations__Host}"
    poc-db-port: "${DatabaseConfigurations__Port}"
    poc-db-name: "${DatabaseConfigurations__Name}"
    poc-api-key: "${ApiConfigurations__Key}"
```

### Step 3: Map Secret Keys to Environment Variables
**File:** `helm/values-production.yaml`

Map the secret keys to environment variables in the format that ASP.NET recognizes to override the JSON configuration:

```yaml
secret:
  envVars:
    - name: DatabaseConfigurations__Host
      secretKey: poc-db-host
    - name: DatabaseConfigurations__Port
      secretKey: poc-db-port
    - name: DatabaseConfigurations__Name
      secretKey: poc-db-name
    - name: ApiConfigurations__Key
      secretKey: poc-api-key
```

## How It Works

The variable substitution flow works as follows:

1. **GitHub Secrets** → Mapped to Helm variables in the CD workflow
2. **Helm Variables** → Substituted into Kubernetes secret values
3. **Kubernetes Secrets** → Injected as environment variables into the pod
4. **Environment Variables** → Override ASP.NET configuration from `appsettings.json`

The key is using the ASP.NET configuration format with double underscores (`__`) to represent nested configuration sections (e.g., `DatabaseConfigurations__Host` maps to `DatabaseConfigurations:Host` in JSON).

