using Prism.Mvvm;

namespace LB_Gestor.Models
{
    public class Usuario: BindableBase
    {
        private string _login = string.Empty;
        public string Login { get { return _login; } set { SetProperty(ref _login, value); } }
        private string _senha = string.Empty;
        public string Senha { get { return _senha; } set { SetProperty(ref _senha, value); } }
        private string _cnpj = string.Empty;
        public string Cnpj { get { return _cnpj; } set { SetProperty(ref _cnpj, value); } }
    }
}
