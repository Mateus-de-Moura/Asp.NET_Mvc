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

// evento ativado quando  o select for selecionado 

const select = document.getElementById('select');
select.addEventListener('change', function () {
    var mes = select.value;       
            $.ajax(
                {
                    type: "POST",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    url: "Home/ListarPormes",
                    data: JSON.stringify({
                        Id: $("#select").val()
                    }),
                    success: function (msg) {
                        Update()
                    },
                    error: function () {

                    }
                });  
    
})


