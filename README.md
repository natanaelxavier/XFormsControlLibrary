# XFormsControlLibrary

Este projeto consiste em uma biblioteca de componentes personalizados para uso em aplicações Windows Forms, chamada XFormsControlLibrary. Os dois componentes disponíveis são o BotãoXForms e o TextBoxXForms.

## Instalação

Para utilizar esses componentes em seu projeto, siga os seguintes passos:

- Baixe o projeto XFormsControlLibrary e compile-o em um arquivo DLL.
- Adicione a referência da biblioteca compilada em seu projeto Windows Forms.
- Adicione um dos componentes personalizados (BotãoXForms ou TextBoxXForms) à sua janela de formulário no Visual Studio, como faria com qualquer outro componente.
    
## Funcionalidades

#### Botão
O BotãoXForms é uma classe derivada da classe Button padrão do Windows Forms, com adições personalizadas. Ele possui várias propriedades personalizadas, que podem definir tamanho de borda, cor da borda, bordas arredondadas. Além disso, o componente é desenhado de forma personalizada no método BotaoXForms_Paint.

#### TextBox
O TextBoxXForms é uma classe derivada da classe Panel padrão do Windows Forms, que contém um TextBox personalizado. Ele possui várias propriedades personalizadas, incluindo cores de borda, tamanho de borda, estilo de sublinhado, bordas arredondadas, cor e texto de placeholder, alinhamento de texto e capacidade de senha. O componente também é capaz de detectar se o usuário digitou um valor ou se está usando o valor padrão do placeholder, e pode ser limpo programaticamente.


## Uso/Exemplos

```C#
using XFormsControlLibrary.Componentes;

namespace Solucao
{
    public partial class Formulario : Form
    {
        public Formulario()
        {
            InitializeComponent();

            TextBoxXForms textbox = new TextBoxXForms();
            textbox.PlaceholderText = "Digite sua idade";
            textbox.BorderRadius = 20;
            this.Controls.Add(textbox);
            
            BotaoXForms button = new BotaoXForms();
            button.TipoBorda = XFormsControlLibrary.Entidades.Enums.EBordaTipo.Completo;
            this.Controls.Add(button);
        }

    }
}
```


## Contribuindo

Contribuições são sempre bem-vindas!
Para contribuir com este projeto, siga os seguintes passos:

- Faça um fork do projeto.
- Crie um branch para sua contribuição (git checkout -b my-feature).
- Faça suas modificações e escreva testes.
- Certifique-se de que todos os testes passam.
- Envie um pull request.

