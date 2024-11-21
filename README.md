
# Media Processing Pipeline

Este repositório demonstra a implementação do **Pipeline Pattern** em C# através de um exemplo de processamento de mídia, como imagens. A abordagem apresentada é genérica, flexível e escalável, permitindo a execução sequencial de operações em objetos de maneira fluente.

---

## **Pipeline Pattern**

O **Pipeline Pattern** é um padrão de design que organiza uma sequência de operações, chamadas de *steps*, aplicadas a um objeto. Cada operação (ou *step*) realiza uma transformação ou executa uma lógica sobre o objeto e passa o resultado para a próxima etapa do pipeline. 

### **Benefícios:**

1. **Modularidade**: Cada *step* é uma unidade isolada e independente.
2. **Extensibilidade**: É fácil adicionar, remover ou reorganizar etapas no pipeline.
3. **Reutilização**: Passos genéricos podem ser reutilizados em diferentes pipelines.
4. **Legibilidade**: O uso de interfaces fluentes melhora a clareza do código.

---

## **Exemplo de Uso**

### **Fluxo de Execução**

Neste exemplo, usamos o **Pipeline Pattern** para processar uma imagem, realizando as seguintes operações:

1. **Download da mídia** de uma localização remota.
2. **Rotação da imagem** em 90 graus.
3. **Adição de metadados** (ex.: autor e descrição).
4. **Aplicação de um filtro** para melhorar a aparência.
5. **Salvamento da imagem** em um diretório temporário.
6. **Upload da mídia** processada para um armazenamento remoto.

**PS.:** Lembrando que não há um processamento efetivo das mídias, mas apenas um delay para simular o comportamento de cada um dos steps, apenas para fins ilustrativos.

### **Exemplo de Código**

```csharp
await MediaPipelineBuilder<Media>
    .Create()
    .AddStep(new DownloadMediaStep(downloadService, tempFolder)) // Baixa a mídia
    .AddStep(new RotateImageStep(90))                            // Rota a imagem
    .AddStep(new AddMetadataStep("Author: Alice | Description: Sunset Landscape")) // Adiciona metadados
    .AddStep(new ApplyFilterStep())                              // Aplica filtro
    .AddStep(new SaveImageStep(tempFolder))                      // Salva no disco
    .AddStep(new UploadMediaStep(downloadService))               // Faz upload da imagem
    .ProcessAsync(photo);                                        // Executa o pipeline
```

---

### **Etapas do Pipeline**

#### 1. **DownloadMediaStep**
Baixa a mídia de uma localização remota e armazena-a em um diretório temporário.

#### 2. **RotateImageStep**
Rota a imagem em um ângulo especificado (neste caso, 90 graus).

#### 3. **AddMetadataStep**
Adiciona informações relevantes à mídia, como autor e descrição.

#### 4. **ApplyFilterStep**
Aplica um filtro para melhorar a aparência da imagem (ex.: ajuste de cores ou nitidez).

#### 5. **SaveImageStep**
Salva a mídia processada em um local especificado no sistema de arquivos.

#### 6. **UploadMediaStep**
Faz o upload da mídia processada para um serviço de armazenamento remoto.

---

## **Como Executar o Projeto**

### Pré-requisitos
- [.NET 7.0+ SDK](https://dotnet.microsoft.com/download)

### Passos
1. Clone o repositório:
   ```bash
   git clone [https://github.com/usuario/media-pipeline.git](https://github.com/sandromendes/pipeline-pattern.git)
   cd pipeline-pattern
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

## **Licença**

Este projeto é licenciado sob a [MIT License](LICENSE).
