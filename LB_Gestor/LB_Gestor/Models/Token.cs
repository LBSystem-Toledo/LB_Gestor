namespace LB_Gestor.Models
{
    public class Token
    {
        public string Login { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string PerfilAppGestor { get; set; } = string.Empty;
        public string access_token { get; set; } = string.Empty;
        public int expires_in { get; set; }
    }
}
