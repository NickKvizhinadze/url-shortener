## Course Notes

### Infrastructure as Code

#### Download Azure CLI
https://learn.microsoft.com/en-us/cli/azure/

#### Log in to Azure
```bash
az login
```

#### Create Resource Group

```bash
az group create --name urlshortener-dev --location westeurope
```

#### Deploy Bicep

#### What if
```bash
az deployment group what-if --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

#### Deploy
```bash
az deployment group create --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

#### Create a User for GH Actions

```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" \
                         --role contributor \
                         --scopes /subscriptions/c09259d3-d160-4e74-a531-6d12d656dfd6 \
                         --sdk-auth
```

#### Apply to Custom Contributor Role

```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" --role 'infra_deploy' --scopes /subscriptions/c09259d3-d160-4e74-a531-6d12d656dfd6 --sdk-auth
```

https://learn.microsoft.com/en-us/azure/role-based-access-control/troubleshooting?tabs=bicep

##### Configure a federated identity credential on an app

https://learn.microsoft.com/en-gb/entra/workload-id/workload-identity-federation-create-trust?pivots=identity-wif-apps-methods-azp#configure-a-federated-identity-credential-on-an-app

### Get Azure Publish Profile

```bash
az webapp deployment list-publishing-profiles --name api-6rujwcaztqjzk --resource-group urlshortener-dev --xml
```

### Get Static Web Apps Deployment Token

```bash
az staticwebapp secrets list --name web-app-piza2nvlxc5jg --query "properties.apiKey"
```


### Utilities

- Base62 converter: https://math.tools/calculator/base/10-62


### GitHub Actions

- https://learn.microsoft.com/en-us/azure/container-apps/tutorial-ci-cd-runners-jobs?tabs=bash&pivots=container-apps-jobs-self-hosted-ci-cd-azure-pipelines
- https://api.github.com/meta
