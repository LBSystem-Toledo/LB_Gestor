namespace LB_Gestor_API.Models
{
    public class Token
    {
        public string PerfilAppGestor { get; set; } = string.Empty;
        public string access_token { get; set; } = string.Empty;
        public int expires_in { get; set; }
    }
}
