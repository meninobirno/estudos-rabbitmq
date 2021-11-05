# estudos-rabbitmq
Repositório contendo as classes utilizadas para o tutorial iniciante do RabbitMQ que pode ser visto clicando [aqui](https://www.rabbitmq.com/getstarted.html).
Montei o projeto passo a passo com o tutorial, e nesse readme vou explicar brevemente a função de cada projeto e classe, mas primeiro, vamos esclarecer o que é o RabbitMQ.

## O que é esse tal "coelho"?
O  RabbitMQ, ou para os íntinmos apenas *rabbit*, é um serviço que gerencia filas, produtores e consumidores. Basicamente funciona como um serviço de entregas de mensagens,
assim como um carteiro, por exemplo. Seguindo essa analogia, temos os quatro componentes para se enviar uma carta:

* A carta: É o que está sendo enviado, para o rabbit, é uma **message**.
* O remetente: É quem ou o que escreveu aquela carta e enviou para os correios. Chamaremos de **producer**.
* O destinatário: É quem ou o que recebe e lê a carta, vamos chamar de **consumer**.
* O correio: É quem recebe a carta do remetente e faz todo o processo para que ela chegue ao destinatário. O nome mais *chique* é **queue**.

Agora que esclarecemos uma coisa que todo mundo já sabia, vamos linkar isso ao rabbit de uma forma mais *técnica*.
Basicamente, o rabbit contém as queues, um producer produz uma message*(que pode ser qualquer objeto)* e a envia para uma queue, esta por sua vez vai encaminhar a message
para um consumer, que lerá essa message e executará a lógica estabelecida no código.
E esta meus amigos, é a explicação mais simples de todas. Agora que você já está por dentro do rabbit, vamos ao código.

**IMPORTANTE**: Para rodar a aplicação e realizar os testes, é necessário que você tenha o rabbitmq instalado e rodando no *localhost* na porta **5672**, para instalar o
rabbit, voc~e pode seguir a documentação encontrada [aqui](https://www.rabbitmq.com/download.html).

## Hello World!
(o código para esta seção pode ser encontrado [aqui](https://github.com/meninobirno/estudos-rabbitmq/tree/master/HelloWorldProducer) e [aqui](https://github.com/meninobirno/estudos-rabbitmq/tree/master/HelloWorldConsumer)).

Neste projeto, o intuito é bem simples:
* vamos criar um producer para escrever uma message contendo um "hello world!"; 
* enviar para uma queue chamada "hello-world";
* criar um consumer para ouvira queue e printar a message no console.

Você pode rodar o projeto usando o powershell executando o comando **dotnet run** dentro do cmd.

Se tudo ocorreu conforme o planejado, o producer logou a message enviada e o consumer logou a message recebida. Agora vamos complicar um pouquinho mais as coisas, hehehe..
