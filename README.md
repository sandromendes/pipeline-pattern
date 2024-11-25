
# Media Processing Pipeline

Este repositório demonstra a implementação do **Pipeline Pattern** em C# através de um exemplo de processamento de mídias, como imagens. A abordagem apresentada é genérica, flexível e escalável, permitindo a execução sequencial de operações em objetos de maneira fluente.

---

## **Pipeline Pattern**

O **Pipeline Pattern** é um padrão de design que organiza uma sequência de operações, chamadas de *steps*, aplicadas a um objeto. Cada operação (ou *step*) realiza uma transformação ou executa uma lógica sobre o objeto e passa o resultado para a próxima etapa do pipeline. 

### **Benefícios:**

1. **Modularidade**: Cada *step* é uma unidade isolada e independente.
2. **Extensibilidade**: É fácil adicionar, remover ou reorganizar etapas no pipeline.
3. **Reutilização**: Passos genéricos podem ser reutilizados em diferentes pipelines.
4. **Legibilidade**: O uso de interfaces fluentes melhora a clareza do código.

---

## **Command Pattern**

O **Command Pattern** é um padrão de design que encapsula uma solicitação como um objeto, permitindo parametrizar outros objetos com diferentes solicitações, enfileirar ou fazer log dessas solicitações, e suportar operações reversíveis.

No contexto do use case, esse padrão organiza a lógica de aplicação em uma única classe responsável por coordenar os processos necessários para atender a uma solicitação. A lógica do use case fica isolada e independente, seguindo o princípio da responsabilidade única.

### **Benefícios:**
1. **Isolamento da Lógica**: A lógica de negócios fica encapsulada no use case, promovendo modularidade.
2. **Reutilização**: Um use case genérico pode ser usado para diversos cenários ao configurar entradas e saídas específicas.
3. **Facilidade de Teste**: O encapsulamento permite testar cada use case de forma independente, simulando diferentes entradas e cenários.
4. **Extensibilidade**: Adicionar novas operações ou modificar a lógica do fluxo é simples e seguro, já que o pipeline e os steps são desacoplados.
5. **Consistência**: Garante que a sequência de operações seja padronizada, com execução previsível.

O uso combinado do Command Pattern com o Pipeline Pattern reforça a separação de responsabilidades, tornando a arquitetura mais organizada, escalável e fácil de manter.

---

## **Exemplos de Uso**

### **Fluxo de Execução**

Neste exemplo, usamos o **Pipeline Pattern** para processar uma imagem, realizando as seguintes operações:

1. **Download da mídia** de uma localização remota.
2. **Rotação da imagem** em 90 graus.
3. **Adição de metadados** (ex.: autor e descrição).
4. **Aplicação de um filtro** para melhorar a aparência.
5. **Salvando a imagem** em um diretório temporário.
6. **Upload da mídia** processada para um armazenamento remoto.

**PS.:** Lembrando que não há um processamento efetivo das mídias, mas apenas um delay para simular o comportamento de cada um dos steps, apenas para fins ilustrativos.

### **Exemplo de Código**

### **Construção do Pipeline**

```csharp
var pipeline = MediaPipelineBuilder<ImageProcessingContext>
    .Create()
    .AddStep(new DownloadMediaStep(cloudStorageService))   // Baixa a mídia
    .AddStep(new RotateImageStep(imageProcessingService))  // Rotaciona a imagem
    .AddStep(new AddMetadataStep())                        // Adiciona metadados
    .AddStep(new ApplyFilterStep())                        // Aplica filtro
    .AddStep(new SaveImageStep())                          // Salva no disco
    .AddStep(new UploadMediaStep(cloudStorageService));    // Faz upload da imagem
    .ProcessAsync(context);                                // Executa o pipeline
```

### **Criação de um Use Case**
Caso seja escolhido criar um Use Case, podemos encapsular um pipeline predefinido no construtor e construir as regras, validações, exceções, dentre outras tarefas, na execução do Use Case.

```csharp
// Este Use Case segue o padrão de design Command, encapsulando toda a lógica necessária para remasterizar uma mídia.
// A interface genérica (IUseCase<TRequest, TResponse>) permite reutilização do mesmo padrão para diferentes casos de uso.
// TRequest representa os dados de entrada e TResponse os de saída, promovendo flexibilidade e consistência.

public class RemasterCloudMediaUseCase : IUseCase<RemasterCloudImageRequest, RemasterCloudMediaResponse>
{
    private readonly IMediaPipeline<ImageProcessingContext> _pipeline;

    // O pipeline é configurado no construtor, utilizando a abordagem de MediaPipelineBuilder.
    // Este builder implementa o padrão Fluent Interface, facilitando a leitura e criação de pipelines complexos.
    public RemasterCloudMediaUseCase(ICloudStorageService cloudStorageService, IGenerativeAiRemasteringService aiRemasteringService)
    {
        _pipeline = MediaPipelineBuilder<ImageProcessingContext>
            .Create()
            .AddStep(new DownloadMediaStep(cloudStorageService)) // Step para baixar a mídia.
            .AddStep(new RemasterImageStep(aiRemasteringService)) // Step que aplica a remasterização usando IA.
            .AddStep(new UploadMediaStep(cloudStorageService))    // Step que faz o upload da mídia processada.
            .Build(); // Finaliza a construção do pipeline.
    }

    // Este método executa o Use Case. Segue o padrão de design Template Method,
    // pois a sequência de operações do pipeline é fixa e definida no construtor.
    public async Task<RemasterCloudMediaResponse> ExecuteAsync(RemasterCloudImageRequest request)
    {
        // Criação do contexto inicial para o pipeline.
        // O contexto carrega todas as informações necessárias para os Steps.
        var context = new ImageProcessingContext
        {
            CloudImagePath = request.CloudImagePath, // Caminho inicial da mídia na nuvem.
            TempFolderPath = request.TempFolderPath, // Diretório temporário para salvar arquivos.
            Image = request.Image                   // Objeto de imagem a ser processado.
        };

        try
        {
            // Executa o pipeline. Cada Step processa e transforma o contexto.
            await _pipeline.ProcessAsync(context);

            // Resposta bem-sucedida com o caminho atualizado da mídia na nuvem.
            return new RemasterCloudMediaResponse
            {
                Success = true,
                RemasteredCloudImagePath = context.CloudPath
            };
        }
        catch (Exception ex)
        {
            // Em caso de falha, encapsula o erro na resposta para que o chamador possa tratá-lo.
            return new RemasterCloudMediaResponse
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }
}
```  

---

## **Como Executar o Projeto**

### Pré-requisitos
- [.NET 7.0+ SDK](https://dotnet.microsoft.com/download)

### Passos
1. Clone o repositório:
   ```bash
   git clone https://github.com/sandromendes/pipeline-pattern-usecase.git
   cd pipeline-pattern-usecase
   ```

2. Restaure as dependências:
   ```bash
   dotnet restore
   ```

3. Execute o programa:
   ```bash
   dotnet run
   ```

---

## **Principais Classes**

### **1. MediaPipelineBuilder<T>**
A classe principal que permite configurar e executar o pipeline.

### **2. IAsyncPipelineStep<T>**
Interface que define a estrutura de um *step* no pipeline. Todos os passos implementam essa interface.
