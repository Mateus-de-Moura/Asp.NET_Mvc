const Input = document.getElementById('Inpt');
const tabela = document.getElementById('Itens');
const situacao = "pago";

Input.addEventListener('keyup', () => {
    let expressao = Input.value.toLowerCase();
    let linhas = tabela.getElementsByTagName('tr');
    for (let posicao in linhas) {
        if (true == isNaN(posicao)) {
            continue;
        }
        let conteudo = linhas[posicao].innerHTML.toLowerCase();

        if (true == conteudo.includes(expressao)) {
            linhas[posicao].style.display = '';

        } else {
            linhas[posicao].style.display = 'none';
        }
    }
    console.log(expressao, linhas);
});

