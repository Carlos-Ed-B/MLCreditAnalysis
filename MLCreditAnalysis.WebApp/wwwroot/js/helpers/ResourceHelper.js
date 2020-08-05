var Culture;
(function (Culture) {
    Culture["ptBR"] = "pt-BR";
    Culture["enUS"] = "en-US";
})(Culture || (Culture = {}));
var ResourceNames = (function () {
    function ResourceNames() {
    }
    ResourceNames.loginSucesso = 'loginSucesso';
    ResourceNames.solicitarSenha = 'solicitarSenha';
    ResourceNames.senhaAlterada = 'senhaAlterada';
    ResourceNames.passageiroInvalido = 'passageiroInvalido';
    ResourceNames.passageiroObrigatorio = 'passageiroObrigatorio';
    ResourceNames.validadeCartao = 'validadeCartao';
    ResourceNames.usuarioSalvo = 'usuarioSalvo';
    ResourceNames.usuarioSalvoAtivacao = 'usuarioSalvoAtivacao';
    ResourceNames.tituloPassageiros = 'tituloPassageiros';
    ResourceNames.tituloPagamento = 'tituloPagamento';
    ResourceNames.tituloProduto = 'tituloProduto';
    ResourceNames.tituloUsuario = 'tituloUsuario';
    return ResourceNames;
}());
var ResourceHelper = (function () {
    function ResourceHelper() {
    }
    ResourceHelper.getResource = function (resourceName) {
        switch (this.getCulture()) {
            case Culture.enUS:
                switch (resourceName) {
                    case ResourceNames.loginSucesso:
                        return 'Login successful, you are being redirected...';
                    case ResourceNames.solicitarSenha:
                        return 'Password change successfully requested, check your email inbox and follow the instructions.';
                    case ResourceNames.senhaAlterada:
                        return 'Password successfully changed.';
                    case ResourceNames.passageiroInvalido:
                        return 'Every declared passenger must have Name and Document informed.';
                    case ResourceNames.passageiroObrigatorio:
                        return 'It is necessary to declare at least one passenger.';
                    case ResourceNames.validadeCartao:
                        return 'Expiration date must be greater than or equal to the current date.';
                    case ResourceNames.usuarioSalvo:
                        return 'User saved successfully.';
                    case ResourceNames.usuarioSalvoAtivacao:
                        return 'Registration successful, an activation email has been sent to your e-mail account, please follow the instructions to activate your account and log in to the System';
                    case ResourceNames.tituloPassageiros:
                        return 'Passengers';
                    case ResourceNames.tituloPagamento:
                        return 'Payment';
                    case ResourceNames.tituloProduto:
                        return 'Product';
                    case ResourceNames.tituloUsuario:
                        return 'User';
                    default:
                        return 'Undefined message: ' + resourceName.toString();
                }
            case Culture.ptBR:
            default:
                switch (resourceName) {
                    case ResourceNames.loginSucesso:
                        return 'Login efetuado com sucesso, você está sendo redirecionado...';
                    case ResourceNames.solicitarSenha:
                        return 'Solicitação de troca de senha realizada com sucesso, verifique sua caixa de e-mail e siga as instruções.';
                    case ResourceNames.senhaAlterada:
                        return 'Senha alterada com sucesso.';
                    case ResourceNames.passageiroInvalido:
                        return 'Todos os passageiros devem ser informados com Nome e Documento.';
                    case ResourceNames.passageiroObrigatorio:
                        return 'É necessário informar ao menos um passageiro.';
                    case ResourceNames.validadeCartao:
                        return 'Data de validade deve ser maior ou igual à data atual.';
                    case ResourceNames.usuarioSalvo:
                        return 'Usuário salvo com sucesso.';
                    case ResourceNames.usuarioSalvoAtivacao:
                        return 'Cadastro efetuado com sucesso, foi enviado um e-mail de ativação para a sua conta de e-email, por favor siga as instruções para ativar sua conta e logar no Sistema.';
                    case ResourceNames.tituloPassageiros:
                        return 'Passageiros';
                    case ResourceNames.tituloPagamento:
                        return 'Pagamento';
                    case ResourceNames.tituloProduto:
                        return 'Produto';
                    case ResourceNames.tituloUsuario:
                        return 'Usuário';
                    default:
                        return 'Mensagem não definida: ' + resourceName.toString();
                }
        }
    };
    ResourceHelper.setCulture = function (culture) {
        localStorage.setItem("userCulture", culture);
    };
    ResourceHelper.getCulture = function () {
        var result = localStorage.getItem("userCulture");
        if (result != null)
            return result;
        return Culture.ptBR;
    };
    return ResourceHelper;
}());
//# sourceMappingURL=ResourceHelper.js.map