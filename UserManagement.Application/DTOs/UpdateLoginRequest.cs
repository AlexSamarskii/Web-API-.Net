namespace UserManagement.Application.DTOs
{
    public class UpdateLoginRequest
    {
        public string OldLogin { get; set; } = string.Empty;
        public string NewLogin { get; set; } = string.Empty;
        public string Requester { get; set; } = string.Empty;
    }
}
