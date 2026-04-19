# Troubleshooting: imagedefinitions.json não encontrado

## Erro
```
Did not find the image definition file imageredefinitions.json in the input artifacts ZIP file.
```

## ✅ Correções Já Aplicadas

1. **Nome do arquivo corrigido**: `imagedefinitions.json` (sem "re")
2. **Redirecionamento correto**: Usa `>` ao invés de `&gt;`
3. **Debug adicionado**: Comandos para verificar criação do arquivo

## 🔍 Possíveis Causas do Erro

### 1. CodeBuild usando branch/commit antigo
- **Verificar**: Qual branch o CodePipeline está configurado para usar?
- **Solução**: Certifique-se que o pipeline aponta para o branch correto

### 2. Cache do CodeBuild
- **Problema**: CodeBuild pode estar usando buildspec em cache
- **Solução**: 
  - No CodeBuild, vá em "Build details" > "Edit" > "Source"
  - Desmarque "Use Git submodules" (se marcado) e salve
  - Ou force um rebuild completo

### 3. Buildspec não está na raiz
- **Verificar**: O arquivo `buildspec.yml` está na raiz do repositório?
- **Localização atual**: `/buildspec.yml` ✅

### 4. CodePipeline configurado com buildspec diferente
- **Problema**: Pipeline pode estar usando um buildspec inline ou de outro local
- **Solução**: 
  - Vá em CodePipeline > Edit > Build stage
  - Verifique se "Buildspec name" está vazio (usa buildspec.yml da raiz)
  - Ou aponta para `buildspec.yml`

### 5. Problema com artifacts
- **Problema**: Seção `artifacts` pode não estar funcionando corretamente
- **Teste**: Verifique os logs do build para ver se o arquivo foi criado

## 🛠️ Como Debugar

### Passo 1: Verificar logs do CodeBuild
Procure por estas linhas nos logs:
```
Gerando arquivo imagedefinitions.json...
Conteúdo do arquivo imagedefinitions.json:
[{"name":"Main","imageUri":"..."}]
Verificando se o arquivo foi criado:
-rw-r--r-- 1 root root ... imagedefinitions.json
```

### Passo 2: Verificar seção de artifacts
No final dos logs, procure por:
```
[Container] Date Time Phase complete: POST_BUILD State: SUCCEEDED
[Container] Date Time Phase context status code:  Message: 
[Container] Date Time Entering phase UPLOAD_ARTIFACTS
```

### Passo 3: Verificar S3
- Vá no bucket S3 mencionado no erro
- Navegue até o path do artefato
- Baixe o ZIP e verifique se `imagedefinitions.json` está dentro

## 🔧 Soluções Alternativas

### Solução 1: Path explícito no artifacts
```yaml
artifacts:
  files:
    - imagedefinitions.json
  base-directory: $CODEBUILD_SRC_DIR
```

### Solução 2: Criar arquivo em local específico
```yaml
post_build:
  commands:
    - mkdir -p artifacts
    - printf '[{"name":"Main","imageUri":"%s"}]' $REPOSITORY_URI:$IMAGE_TAG > artifacts/imagedefinitions.json
artifacts:
  files:
    - '**/*'
  base-directory: artifacts
```

### Solução 3: Verificar se CodePipeline está configurado para ECS deploy
```
Action provider: Amazon ECS
Input artifacts: BuildArtifact
Image definitions file: imagedefinitions.json  # ← Deve ser exatamente este nome
```

## 📋 Checklist de Verificação

- [ ] Build está usando o branch correto?
- [ ] Logs mostram que o arquivo foi criado?
- [ ] Logs mostram que o arquivo foi incluído nos artifacts?
- [ ] S3 artifact ZIP contém o arquivo?
- [ ] CodePipeline Deploy stage está configurado para "Amazon ECS"?
- [ ] "Image definitions file" no Deploy stage é `imagedefinitions.json`?
- [ ] Container name no arquivo ("Main") corresponde à Task Definition?

## 🎯 Próximos Passos

1. Execute um novo build no CodeBuild
2. Verifique os logs completos
3. Se o arquivo ainda não for encontrado, compartilhe:
   - Logs completos do CodeBuild
   - Configuração do CodePipeline (Deploy stage)
   - Screenshot do erro
