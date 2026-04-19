# Configuração AWS ECS - DesafioMirante

## ✅ Problemas Resolvidos

### 1. Erro "InternalError" na Build
O erro estava ocorrendo porque:
1. ❌ O buildspec.yml estava tentando fazer build de `./registroContrato` que não existe
2. ❌ O Dockerfile tinha `USER $APP_UID` que causava problemas no AWS
3. ❌ O caminho do Dockerfile estava incorreto

**Status:** ✅ **CORRIGIDO**

### 2. Erro de Deploy no ECS
O deploy falhava porque:
1. ❌ O nome do container no `imagedefinitions.json` não correspondia à Task Definition
2. ❌ buildspec.yml usava `"desafio-mirante-web"` mas a Task Definition tinha `"Main"`

**Status:** ✅ **CORRIGIDO**

## Mudanças Realizadas

### 1. buildspec.yml criado e corrigido
- ✅ Build correto: `docker build -f DesafioMiranteWeb/Dockerfile .`
- ✅ Context na raiz do projeto (necessário para copiar todos os projetos)
- ✅ **Nome do container corrigido para "Main"** no imagedefinitions.json
- ✅ Geração correta do `imagedefinitions.json`

### 2. Dockerfile corrigido
- ✅ Removido `USER $APP_UID` que causava problemas no ECS
- ✅ Mantida toda a estrutura multi-stage build

## 🎯 Container Name = "Main"

**IMPORTANTE:** O nome do container está configurado como **"Main"** no buildspec.yml porque esse é o nome definido na Task Definition do ECS.

Se você precisar alterar, certifique-se de que o nome em `buildspec.yml` corresponda EXATAMENTE ao nome na Task Definition:

```yaml
# Linha 25 do buildspec.yml - já configurada corretamente
- printf '[{"name":"Main","imageUri":"%s"}]' $REPOSITORY_URI:$IMAGE_TAG > imagedefinitions.json
```

## Configuração AWS Necessária

### CodeBuild - Variáveis de Ambiente
Certifique-se de que as seguintes variáveis estão configuradas no CodeBuild:

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `AWS_DEFAULT_REGION` | Região AWS | `us-east-1` |
| `AWS_ACCOUNT_ID` | ID da conta AWS | `123456789012` |
| `IMAGE_REPO_NAME` | Nome do repositório ECR | `desafio-mirante-web` |

### Permissões IAM Necessárias

O role do CodeBuild precisa das seguintes permissões:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ecr:GetAuthorizationToken",
        "ecr:BatchCheckLayerAvailability",
        "ecr:GetDownloadUrlForLayer",
        "ecr:BatchGetImage",
        "ecr:PutImage",
        "ecr:InitiateLayerUpload",
        "ecr:UploadLayerPart",
        "ecr:CompleteLayerUpload"
      ],
      "Resource": "*"
    }
  ]
}
```

O role do CodePipeline/ECS também precisa:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ecs:UpdateService",
        "ecs:DescribeServices",
        "ecs:DescribeTaskDefinition",
        "ecs:DescribeTasks",
        "ecs:ListTasks",
        "ecs:RegisterTaskDefinition",
        "iam:PassRole"
      ],
      "Resource": "*"
    }
  ]
}
```

## Estrutura do Projeto

```
DesafioMirante/
├── buildspec.yml              # ✅ NOVO - Configuração CodeBuild
├── DesafioMiranteWeb/
│   ├── Dockerfile            # ✅ CORRIGIDO - Removido USER $APP_UID
│   └── DesafioMiranteWeb.csproj
├── Appllication/
│   └── Application.csproj
├── Domain/
│   └── Domain.csproj
└── Infrastructure/
    └── Infrastructure.csproj
```

## Próximos Passos

1. ✅ Verifique o nome do container na Task Definition
2. ✅ Atualize o buildspec.yml com o nome correto
3. ✅ Commit e push as mudanças
4. ✅ Execute o pipeline novamente no CodePipeline
5. ✅ Monitore os logs no CloudWatch

## Banco de Dados

⚠️ **IMPORTANTE**: O docker-compose.yml usa SQL Server em container, mas no AWS você precisará:

1. **Criar RDS SQL Server** ou usar SQL Server em EC2
2. **Configurar a connection string** via:
   - AWS Secrets Manager (recomendado)
   - AWS Systems Manager Parameter Store
   - Variáveis de ambiente na Task Definition

Exemplo de configuração na Task Definition:
```json
{
  "environment": [
    {
      "name": "ConnectionStrings__DefaultConnection",
      "value": "Server=seu-rds-endpoint.rds.amazonaws.com;Database=DesafioMiranteDb;User Id=admin;Password=SuaSenha;"
    }
  ]
}
```

Ou usando Secrets Manager (recomendado):
```json
{
  "secrets": [
    {
      "name": "ConnectionStrings__DefaultConnection",
      "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789012:secret:db-connection-string"
    }
  ]
}
```

## Troubleshooting

### Se o erro persistir:

1. **Verifique CloudWatch Logs**:
   - CodeBuild logs: `/aws/codebuild/seu-projeto`
   - ECS logs: `/ecs/sua-task-definition`

2. **Verifique se a imagem foi enviada ao ECR**:
   ```bash
   aws ecr describe-images --repository-name seu-repo --region us-east-1
   ```

3. **Teste o build localmente**:
   ```bash
   docker build -f DesafioMiranteWeb/Dockerfile -t desafio-mirante-web:test .
   docker run -p 8080:8080 desafio-mirante-web:test
   ```

4. **Verifique a Task Definition**:
   - Memória suficiente? (mínimo 512 MB recomendado)
   - CPU suficiente? (mínimo 256 unidades)
   - Porta correta mapeada? (80 ou 8080)

## Contato

Se o problema persistir, compartilhe:
- Logs completos do CodeBuild
- Logs do CloudWatch do ECS
- Configuração da Task Definition
