function Sucesso(data) {
    Swal.fire(
        'Sucesso',
        data.msg,
        'Sucess'
      )
}

function Falha(){
    Swal.fire(
        'Falha',
        'Ocorreu um erro inesperado !',
        'error'
    );
}