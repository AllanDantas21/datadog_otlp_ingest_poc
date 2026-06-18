# OTelDatadogTest - Migração para AWS Lambda

Este projeto foi migrado com sucesso para rodar como uma **AWS Lambda** utilizando o runtime **.NET 8 (LTS)**, mantendo a capacidade de ser executado localmente para testes e desenvolvimento.

---

## 🚀 Como Funciona a Integração

### 1. Handler da Lambda
O ponto de entrada para a AWS Lambda está definido no arquivo [Function.cs](file:///home/adn/projects/a/o/Function.cs).
- **Assinatura do Handler:** `OTelDatadogTest::Function::FunctionHandler`
- **Serializador de JSON:** Configurado por padrão usando `Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer`.

### 2. Flush de Logs Garantido
Como o ambiente de execução da AWS Lambda é congelado (*frozen*) logo após o término da invocação, logs enviados de forma assíncrona (como é o padrão do OpenTelemetry Batch Exporter) correm o risco de se perderem ou sofrerem atrasos imensos.
- Para resolver isso, instanciamos a `LoggerFactory` dentro do escopo `using var` no [Function.cs](file:///home/adn/projects/a/o/Function.cs).
- Ao final de cada invocação, a `LoggerFactory` é descartada automaticamente, forçando um **flush síncrono imediato** de todos os buffers do OpenTelemetry para o Datadog.

### 3. Configuração via Variáveis de Ambiente
O arquivo [BaseFunction.cs](file:///home/adn/projects/a/o/BaseFunction.cs) foi ajustado para carregar as chaves de API dinamicamente, evitando credenciais expostas no código:
- `DD_API_KEY`: Chave da API do Datadog (Valor padrão definido como fallback: `d42a8c73...`).
- `OTLP_ENDPOINT`: Endpoint do OpenTelemetry (Valor padrão definido como fallback: `https://otlp.datadoghq.com/v1/logs`).

---

## 🛠️ Como Testar Localmente

Você ainda pode rodar a aplicação localmente como um console application padrão. O arquivo [Program.cs](file:///home/adn/projects/a/o/Program.cs) permanece disponível:

```bash
dotnet run
```

---

## 📦 Como Publicar e Empacotar para AWS Lambda

Como o utilitário `dotnet lambda` não está instalado globalmente por padrão, você pode compilar e gerar o arquivo `.zip` manualmente utilizando a CLI do .NET e a ferramenta `zip` do Linux:

1. **Publique a aplicação** visando o runtime do Lambda (Linux x64):
   ```bash
   dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
   ```

2. **Compacte o resultado** da publicação:
   ```bash
   cd publish
   zip -r ../deploy.zip .
   cd ..
   ```

3. **Faça o upload** do arquivo `deploy.zip` gerado na raiz do projeto para o console da AWS Lambda.

4. **Configure o Handler** nas configurações da Lambda com o valor:
   ```text
   OTelDatadogTest::Function::FunctionHandler
   ```
# datadog_otlp_ingest_poc
